using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ajedrez
{
    /// <summary>
    /// Acceso a la base de datos de Supabase para el registro de partidas de
    /// damas y ajedrez, el guardado del estado (para poder continuar) y el
    /// cálculo/actualización del ELO de los usuarios.
    /// </summary>
    public static class PartidaRepository
    {
        private static string BaseUrl => SupabaseConfig.SupabaseUrl + "/rest/v1";
        private static string Key => SupabaseConfig.SupabaseKey;

        private const double K_FACTOR = 32.0;          // Factor K estándar
        private const double ELO_POR_DEFECTO = 1200.0; // ELO de un invitado sin cuenta
        private const int ELO_MINIMO = 100;

        private static readonly Dictionary<string, int> _idUsuarioCache = new Dictionary<string, int>();

        // Un único HttpClient compartido: evita agotar los sockets creando una
        // conexión nueva en cada petición.
        private static readonly HttpClient _http = new HttpClient();

        static PartidaRepository()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Un fallo de la base de datos no debe dejar la interfaz colgada.
            _http.Timeout = TimeSpan.FromSeconds(15);
        }

        // ---------------- Modelos ----------------
        public class Partida
        {
            public int id_partida { get; set; }
            public string jugador_blancas { get; set; }
            public string jugador_negras { get; set; }
            public string ganador { get; set; }
            public string estado { get; set; }
            public string modo { get; set; }
            public bool soplar { get; set; }
            public string tipo_juego { get; set; }
            public int? id_usuario { get; set; }
            public DateTime? fecha_inicio { get; set; }
            public DateTime? fecha_fin { get; set; }
        }

        public class Movimiento
        {
            public int id_movimiento { get; set; }
            public int id_partida { get; set; }
            public int num_movimientos { get; set; }
            public string turno_final { get; set; }
            public string movimiento_final { get; set; }
            public JToken tablero { get; set; }
        }

        private class UsuarioDatos
        {
            public int id_usuario { get; set; }
            public string nombre_usuario { get; set; }
            public double elo { get; set; }
            public int partidas_jugadas { get; set; }
            public int partidas_ganadas { get; set; }
            public int partidas_empatadas { get; set; }
            public int partidas_perdidas { get; set; }
        }

        // ---------------- Utilidades ----------------
        private static string Esc(string s) => Uri.EscapeDataString(s ?? "");

        /// <summary>
        /// Ejecuta una petición HTTP contra Supabase. Devuelve true solo si la
        /// respuesta fue correcta y deja el texto devuelto en
        /// <paramref name="cuerpo"/>. Nunca lanza excepciones: ante cualquier
        /// fallo (red, TLS, timeout, JSON inesperado...) devuelve false para
        /// que el juego siga funcionando aunque la base de datos falle.
        /// </summary>
        private static bool Enviar(HttpMethod metodo, string url, JObject body, string prefer, out string cuerpo)
        {
            cuerpo = null;
            try
            {
                var req = new HttpRequestMessage(metodo, url);
                req.Headers.Add("apikey", Key);
                req.Headers.Add("Authorization", "Bearer " + Key);
                if (!string.IsNullOrEmpty(prefer)) req.Headers.Add("Prefer", prefer);
                if (body != null)
                    req.Content = new StringContent(body.ToString(Formatting.None), Encoding.UTF8, "application/json");

                var resp = _http.SendAsync(req).GetAwaiter().GetResult();
                if (!resp.IsSuccessStatusCode) return false;

                cuerpo = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>GET que devuelve el resultado deserializado o null si falla.</summary>
        private static T Get<T>(string url) where T : class
        {
            try
            {
                if (!Enviar(HttpMethod.Get, url, null, null, out string json)) return null;
                if (string.IsNullOrWhiteSpace(json)) return null;
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch { return null; }
        }

        /// <summary>PATCH que devuelve true solo si el cambio se aplicó.</summary>
        private static bool Patch(string url, JObject body)
        {
            return Enviar(new HttpMethod("PATCH"), url, body, "return=minimal", out _);
        }

        // ---------------- Partidas ----------------
        /// <summary>Crea una partida nueva y devuelve su id (o -1 si falla).</summary>
        public static int CrearPartida(string tipo, string blancas, string negras, int? idUsuario, bool soplar = false)
        {
            try
            {
                var body = new JObject
                {
                    ["jugador_blancas"] = blancas,
                    ["jugador_negras"] = negras,
                    ["tipo_juego"] = tipo,
                    ["estado"] = "en_curso"
                };
                if (soplar) body["soplar"] = true;
                if (idUsuario.HasValue) body["id_usuario"] = idUsuario.Value;

                string url = BaseUrl + "/partidas";
                bool ok = Enviar(HttpMethod.Post, url, body, "return=representation", out string json);

                // Si la columna 'soplar' todavía no existe en la tabla, se reintenta sin ella
                if (!ok && soplar)
                {
                    body.Remove("soplar");
                    ok = Enviar(HttpMethod.Post, url, body, "return=representation", out json);
                }
                if (!ok || string.IsNullOrWhiteSpace(json)) return -1;

                var arr = JArray.Parse(json);
                if (arr.Count == 0) return -1;
                return (int)arr[0]["id_partida"];
            }
            catch { return -1; }
        }

        /// <summary>Marca la partida como finalizada con su ganador.</summary>
        public static bool FinalizarPartida(int idPartida, string ganador)
        {
            if (idPartida < 0) return false;
            try
            {
                var body = new JObject
                {
                    ["estado"] = "finalizada",
                    ["fecha_fin"] = DateTime.UtcNow.ToString("o")
                };
                // Si no hay ganador (partida abandonada) se deja el campo sin tocar.
                if (!string.IsNullOrWhiteSpace(ganador))
                    body["ganador"] = ganador;
                return Patch(BaseUrl + "/partidas?id_partida=eq." + idPartida, body);
            }
            catch { return false; }
        }

        /// <summary>Partidas del usuario (opcionalmente filtradas por tipo).</summary>
        public static List<Partida> ListarPartidas(string tipo, string usuario)
        {
            var resultado = new List<Partida>();
            if (string.IsNullOrWhiteSpace(usuario)) return resultado;
            try
            {
                string url = BaseUrl + "/partidas?select=*&or=(jugador_blancas.eq." + Esc(usuario)
                             + ",jugador_negras.eq." + Esc(usuario) + ")";
                if (!string.IsNullOrWhiteSpace(tipo)) url += "&tipo_juego=eq." + Esc(tipo);
                url += "&order=fecha_inicio.desc";

                var list = Get<List<Partida>>(url);
                if (list != null) resultado = list;
            }
            catch { }
            return resultado;
        }

        /// <summary>Busca una partida en curso del usuario para poder continuarla.</summary>
        public static Partida BuscarPartidaEnCurso(string tipo, string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario)) return null;
            try
            {
                string url = BaseUrl + "/partidas?select=*&estado=eq.en_curso&or=(jugador_blancas.eq."
                             + Esc(usuario) + ",jugador_negras.eq." + Esc(usuario) + ")";
                if (!string.IsNullOrWhiteSpace(tipo)) url += "&tipo_juego=eq." + Esc(tipo);
                url += "&order=fecha_inicio.desc&limit=1";

                var list = Get<List<Partida>>(url);
                return (list != null && list.Count > 0) ? list[0] : null;
            }
            catch { return null; }
        }

        // ---------------- Estado (continuar partida) ----------------
        public static Movimiento ObtenerMovimiento(int idPartida)
        {
            try
            {
                string url = BaseUrl + "/movimientos?id_partida=eq." + idPartida + "&select=*&limit=1";
                var list = Get<List<Movimiento>>(url);
                return (list != null && list.Count > 0) ? list[0] : null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Igual que ObtenerMovimiento, pero distingue los dos casos que
        /// producen null: "no hay estado guardado" (devuelve true y mov = null)
        /// y "no se pudo consultar" (devuelve false). Así, ante un fallo de red
        /// no se cierra por error una partida que sí se podía continuar.
        /// </summary>
        public static bool ObtenerMovimientoSeguro(int idPartida, out Movimiento mov)
        {
            mov = null;

            string url = BaseUrl + "/movimientos?id_partida=eq." + idPartida + "&select=*&limit=1";
            if (!Enviar(HttpMethod.Get, url, null, null, out string json))
                return false; // no se pudo consultar

            try
            {
                var list = JsonConvert.DeserializeObject<List<Movimiento>>(json);
                mov = (list != null && list.Count > 0) ? list[0] : null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Inserta o actualiza el estado del tablero de una partida.</summary>
        public static bool GuardarEstado(int idPartida, int numMovimientos, string turnoFinal,
                                         string ultimoMovimiento, string tableroJson)
        {
            if (idPartida < 0) return false;
            try
            {
                var body = new JObject
                {
                    ["num_movimientos"] = numMovimientos,
                    ["turno_final"] = turnoFinal ?? "",
                    ["movimiento_final"] = ultimoMovimiento ?? ""
                };
                if (!string.IsNullOrWhiteSpace(tableroJson))
                    body["tablero"] = JToken.Parse(tableroJson);

                if (ObtenerMovimiento(idPartida) != null)
                    return Patch(BaseUrl + "/movimientos?id_partida=eq." + idPartida, body);

                body["id_partida"] = idPartida;
                return Enviar(HttpMethod.Post, BaseUrl + "/movimientos", body, null, out _);
            }
            catch { return false; }
        }

        // ---------------- Usuarios y ELO ----------------
        private static UsuarioDatos ObtenerUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            try
            {
                string url = BaseUrl + "/usuarios?select=id_usuario,nombre_usuario,elo,partidas_jugadas,"
                             + "partidas_ganadas,partidas_empatadas,partidas_perdidas"
                             + "&nombre_usuario=eq." + Esc(nombre) + "&limit=1";
                var list = Get<List<UsuarioDatos>>(url);
                return (list != null && list.Count > 0) ? list[0] : null;
            }
            catch { return null; }
        }

        /// <summary>Devuelve el id_usuario registrado o null si es un invitado.</summary>
        public static int? ObtenerIdUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            if (_idUsuarioCache.TryGetValue(nombre, out int id)) return id;
            var u = ObtenerUsuario(nombre);
            if (u == null) return null;
            _idUsuarioCache[nombre] = u.id_usuario;
            return u.id_usuario;
        }

        /// <summary>
        /// Registra el resultado de la partida y actualiza el ELO y las
        /// estadísticas de los jugadores que tengan cuenta registrada.
        /// <paramref name="ganador"/> puede ser el nombre de A, de B o "empate".
        /// </summary>
        public static void RegistrarResultado(string jugadorA, string jugadorB, string ganador)
        {
            if (string.IsNullOrWhiteSpace(jugadorA) || string.IsNullOrWhiteSpace(jugadorB)) return;
            if (string.Equals(jugadorA, jugadorB, StringComparison.OrdinalIgnoreCase)) return;

            try
            {
                var ua = ObtenerUsuario(jugadorA);
                var ub = ObtenerUsuario(jugadorB);

                double eloA = ua != null ? ua.elo : ELO_POR_DEFECTO;
                double eloB = ub != null ? ub.elo : ELO_POR_DEFECTO;

                double sA, sB;
                if (string.Equals(ganador, "empate", StringComparison.OrdinalIgnoreCase))
                {
                    sA = 0.5; sB = 0.5;
                }
                else if (string.Equals(ganador, jugadorA, StringComparison.OrdinalIgnoreCase))
                {
                    sA = 1; sB = 0;
                }
                else
                {
                    sA = 0; sB = 1;
                }

                if (ua != null) ActualizarUsuario(ua, eloA, eloB, sA);
                if (ub != null) ActualizarUsuario(ub, eloB, eloA, sB);
            }
            catch
            {
                // El ELO es secundario: si falla no debe cerrar el juego.
            }
        }

        private static void ActualizarUsuario(UsuarioDatos u, double propio, double rival, double s)
        {
            double esperado = 1.0 / (1.0 + Math.Pow(10.0, (rival - propio) / 400.0));
            int nuevoElo = (int)Math.Round(propio + K_FACTOR * (s - esperado));
            if (nuevoElo < ELO_MINIMO) nuevoElo = ELO_MINIMO;

            var body = new JObject
            {
                ["elo"] = nuevoElo,
                ["partidas_jugadas"] = u.partidas_jugadas + 1,
                ["partidas_ganadas"] = u.partidas_ganadas + (s == 1.0 ? 1 : 0),
                ["partidas_empatadas"] = u.partidas_empatadas + (s == 0.5 ? 1 : 0),
                ["partidas_perdidas"] = u.partidas_perdidas + (s == 0.0 ? 1 : 0)
            };
            Patch(BaseUrl + "/usuarios?id_usuario=eq." + u.id_usuario, body);
        }
    }
}
