using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ajedrez
{
    /// <summary>Datos de un usuario tal como los devuelve la base de datos.</summary>
    public class UsuarioNube
    {
        public int id_usuario { get; set; }
        public string nombre_usuario { get; set; }
        public double elo { get; set; }
        public int partidas_jugadas { get; set; }
        public int partidas_ganadas { get; set; }
        public int partidas_empatadas { get; set; }
        public int partidas_perdidas { get; set; }
    }

    /// <summary>Usuario con todas sus columnas, para el panel administrativo.</summary>
    public class UsuarioCompleto
    {
        public int id_usuario { get; set; }
        public string nombre_usuario { get; set; }
        public string email { get; set; }
        public string contrasena { get; set; }
        public DateTime? fecha_registro { get; set; }
        public double elo { get; set; }
        public int partidas_jugadas { get; set; }
        public int partidas_ganadas { get; set; }
        public int partidas_empatadas { get; set; }
        public int partidas_perdidas { get; set; }
        public bool activo { get; set; }
        public string imagen_url { get; set; }
    }

    /// <summary>
    /// Acceso directo a Supabase (PostgREST). Es la única clase que habla por
    /// HTTP; el resto de la app trabaja contra la base local. Ninguna operación
    /// lanza excepciones: si falla la red devuelve null/false para que la app
    /// siga funcionando sin conexión.
    /// </summary>
    public static class Nube
    {
        private static string BaseUrl { get { return SupabaseConfig.SupabaseUrl + "/rest/v1"; } }
        private static string Key { get { return SupabaseConfig.SupabaseKey; } }

        // Un único HttpClient compartido: evita agotar los sockets creando una
        // conexión nueva en cada petición.
        private static readonly HttpClient _http = new HttpClient();

        static Nube()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _http.Timeout = TimeSpan.FromSeconds(15);
        }

        private static string Esc(string s)
        {
            return Uri.EscapeDataString(s ?? "");
        }

        /// <summary>
        /// Ejecuta una petición HTTP contra Supabase. Devuelve true solo si la
        /// respuesta fue correcta y deja el texto devuelto en
        /// <paramref name="cuerpo"/>. Nunca lanza excepciones.
        /// </summary>
        internal static bool Enviar(HttpMethod metodo, string url, JObject body, string prefer, out string cuerpo)
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

        private static T Get<T>(string url) where T : class
        {
            try
            {
                string json;
                if (!Enviar(HttpMethod.Get, url, null, null, out json)) return null;
                if (string.IsNullOrWhiteSpace(json)) return null;
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch { return null; }
        }

        private static bool Patch(string url, JObject body)
        {
            return Enviar(new HttpMethod("PATCH"), url, body, "return=minimal", out _);
        }

        // ---------------- Partidas ----------------

        /// <summary>Inserta una partida en Supabase y devuelve su id (o -1).</summary>
        public static int InsertarPartida(PartidaLocal p)
        {
            try
            {
                var body = new JObject
                {
                    ["jugador_blancas"] = p.JugadorBlancas,
                    ["jugador_negras"] = p.JugadorNegras,
                    ["tipo_juego"] = p.TipoJuego,
                    ["estado"] = string.IsNullOrWhiteSpace(p.Estado) ? "en_curso" : p.Estado
                };
                if (p.Soplar) body["soplar"] = true;
                if (p.IdUsuario.HasValue) body["id_usuario"] = p.IdUsuario.Value;
                if (p.FechaInicio.HasValue) body["fecha_inicio"] = p.FechaInicio.Value.ToUniversalTime().ToString("o");
                if (!string.IsNullOrWhiteSpace(p.Ganador)) body["ganador"] = p.Ganador;
                if (p.FechaFin.HasValue) body["fecha_fin"] = p.FechaFin.Value.ToUniversalTime().ToString("o");

                string url = BaseUrl + "/partidas";
                string json;
                bool ok = Enviar(HttpMethod.Post, url, body, "return=representation", out json);

                // Si la columna 'soplar' todavía no existe, se reintenta sin ella.
                if (!ok && p.Soplar)
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

        /// <summary>Actualiza en Supabase la partida ya insertada.</summary>
        public static bool ActualizarPartida(int serverId, PartidaLocal p)
        {
            if (serverId <= 0) return false;
            try
            {
                var body = new JObject
                {
                    ["estado"] = string.IsNullOrWhiteSpace(p.Estado) ? "en_curso" : p.Estado
                };
                if (p.Soplar) body["soplar"] = true;
                if (!string.IsNullOrWhiteSpace(p.Ganador)) body["ganador"] = p.Ganador;
                if (p.FechaFin.HasValue) body["fecha_fin"] = p.FechaFin.Value.ToUniversalTime().ToString("o");

                return Patch(BaseUrl + "/partidas?id_partida=eq." + serverId, body);
            }
            catch { return false; }
        }

        /// <summary>Partidas del usuario (opcionalmente filtradas por tipo).</summary>
        public static List<PartidaRepository.Partida> ListarPartidas(string tipo, string usuario)
        {
            var resultado = new List<PartidaRepository.Partida>();
            if (string.IsNullOrWhiteSpace(usuario)) return resultado;
            try
            {
                string url = BaseUrl + "/partidas?select=*&or=(jugador_blancas.eq." + Esc(usuario)
                             + ",jugador_negras.eq." + Esc(usuario) + ")";
                if (!string.IsNullOrWhiteSpace(tipo)) url += "&tipo_juego=eq." + Esc(tipo);
                url += "&order=fecha_inicio.desc";

                var list = Get<List<PartidaRepository.Partida>>(url);
                if (list != null) resultado = list;
            }
            catch { }
            return resultado;
        }

        // ---------------- Movimientos ----------------

        public static int InsertarMovimiento(int serverIdPartida, MovimientoLocal m)
        {
            if (serverIdPartida <= 0) return -1;
            try
            {
                var body = new JObject
                {
                    ["id_partida"] = serverIdPartida,
                    ["num_movimientos"] = m.NumMovimientos,
                    ["turno_final"] = m.TurnoFinal ?? "",
                    ["movimiento_final"] = m.MovimientoFinal ?? ""
                };
                if (!string.IsNullOrWhiteSpace(m.Tablero))
                    body["tablero"] = JToken.Parse(m.Tablero);

                string json;
                if (!Enviar(HttpMethod.Post, BaseUrl + "/movimientos", body, "return=representation", out json)
                    || string.IsNullOrWhiteSpace(json))
                    return -1;

                var arr = JArray.Parse(json);
                if (arr.Count == 0) return -1;
                return (int)arr[0]["id_movimiento"];
            }
            catch { return -1; }
        }

        public static bool ActualizarMovimiento(int serverId, MovimientoLocal m)
        {
            if (serverId <= 0) return false;
            try
            {
                var body = new JObject
                {
                    ["num_movimientos"] = m.NumMovimientos,
                    ["turno_final"] = m.TurnoFinal ?? "",
                    ["movimiento_final"] = m.MovimientoFinal ?? ""
                };
                if (!string.IsNullOrWhiteSpace(m.Tablero))
                    body["tablero"] = JToken.Parse(m.Tablero);

                return Patch(BaseUrl + "/movimientos?id_movimiento=eq." + serverId, body);
            }
            catch { return false; }
        }

        public static List<PartidaRepository.Movimiento> MovimientosDePartidas(IEnumerable<int> idsPartida)
        {
            var resultado = new List<PartidaRepository.Movimiento>();
            try
            {
                var ids = idsPartida != null ? idsPartida.Where(i => i > 0).Distinct().ToList() : new List<int>();
                if (ids.Count == 0) return resultado;

                string lista = string.Join(",", ids);
                string url = BaseUrl + "/movimientos?id_partida=in.(" + lista + ")&select=*";

                var list = Get<List<PartidaRepository.Movimiento>>(url);
                if (list != null) resultado = list;
            }
            catch { }
            return resultado;
        }

        // ---------------- Usuarios ----------------

        public static UsuarioNube ObtenerUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            try
            {
                string url = BaseUrl + "/usuarios?select=id_usuario,nombre_usuario,elo,partidas_jugadas,"
                             + "partidas_ganadas,partidas_empatadas,partidas_perdidas"
                             + "&nombre_usuario=eq." + Esc(nombre) + "&limit=1";
                var list = Get<List<UsuarioNube>>(url);
                return (list != null && list.Count > 0) ? list[0] : null;
            }
            catch { return null; }
        }

        public static bool ActualizarUsuario(int serverId, UsuarioLocal u)
        {
            if (serverId <= 0) return false;
            try
            {
                var body = new JObject
                {
                    ["elo"] = u.Elo,
                    ["partidas_jugadas"] = u.PartidasJugadas,
                    ["partidas_ganadas"] = u.PartidasGanadas,
                    ["partidas_empatadas"] = u.PartidasEmpatadas,
                    ["partidas_perdidas"] = u.PartidasPerdidas
                };
                return Patch(BaseUrl + "/usuarios?id_usuario=eq." + serverId, body);
            }
            catch { return false; }
        }

        // ---------------- ADMINISTRACIÓN (panel de administrador) ----------------

        private static bool Delete(string url)
        {
            try { return Enviar(HttpMethod.Delete, url, null, null, out _); }
            catch { return false; }
        }

        /// <summary>Lista TODOS los usuarios de la base (incluye contraseña).</summary>
        public static List<UsuarioCompleto> ListarUsuarios()
        {
            var resultado = new List<UsuarioCompleto>();
            try
            {
                var list = Get<List<UsuarioCompleto>>(BaseUrl + "/usuarios?select=*&order=id_usuario.asc");
                if (list != null) resultado = list;
            }
            catch { }
            return resultado;
        }

        /// <summary>Crea un usuario nuevo. Devuelve true si se insertó.</summary>
        public static bool CrearUsuario(UsuarioCompleto u)
        {
            if (u == null || string.IsNullOrWhiteSpace(u.nombre_usuario)) return false;
            try
            {
                var body = new JObject { ["nombre_usuario"] = u.nombre_usuario };
                if (!string.IsNullOrWhiteSpace(u.contrasena)) body["contrasena"] = u.contrasena;
                if (!string.IsNullOrWhiteSpace(u.email)) body["email"] = u.email;
                body["elo"] = u.elo;
                body["partidas_jugadas"] = u.partidas_jugadas;
                body["partidas_ganadas"] = u.partidas_ganadas;
                body["partidas_empatadas"] = u.partidas_empatadas;
                body["partidas_perdidas"] = u.partidas_perdidas;
                body["activo"] = u.activo;

                return Enviar(HttpMethod.Post, BaseUrl + "/usuarios", body, "return=minimal", out _);
            }
            catch { return false; }
        }

        /// <summary>Actualiza todos los campos editables de un usuario.</summary>
        public static bool ActualizarUsuarioCompleto(UsuarioCompleto u)
        {
            if (u == null || u.id_usuario <= 0) return false;
            try
            {
                var body = new JObject
                {
                    ["nombre_usuario"] = u.nombre_usuario,
                    ["elo"] = u.elo,
                    ["partidas_jugadas"] = u.partidas_jugadas,
                    ["partidas_ganadas"] = u.partidas_ganadas,
                    ["partidas_empatadas"] = u.partidas_empatadas,
                    ["partidas_perdidas"] = u.partidas_perdidas,
                    ["activo"] = u.activo
                };
                body["email"] = u.email; // admite null
                if (!string.IsNullOrWhiteSpace(u.contrasena)) body["contrasena"] = u.contrasena;

                return Patch(BaseUrl + "/usuarios?id_usuario=eq." + u.id_usuario, body);
            }
            catch { return false; }
        }

        /// <summary>Cambia la contraseña de un usuario.</summary>
        public static bool ResetearContrasena(int serverId, string nueva)
        {
            if (serverId <= 0 || nueva == null) return false;
            try
            {
                var body = new JObject { ["contrasena"] = nueva };
                return Patch(BaseUrl + "/usuarios?id_usuario=eq." + serverId, body);
            }
            catch { return false; }
        }

        public static bool EliminarUsuario(int serverId)
        {
            if (serverId <= 0) return false;
            return Delete(BaseUrl + "/usuarios?id_usuario=eq." + serverId);
        }

        /// <summary>Todas las partidas de la base (de todos los usuarios).</summary>
        public static List<PartidaRepository.Partida> ListarTodasPartidas()
        {
            var resultado = new List<PartidaRepository.Partida>();
            try
            {
                var list = Get<List<PartidaRepository.Partida>>(
                    BaseUrl + "/partidas?select=*&order=fecha_inicio.desc");
                if (list != null) resultado = list;
            }
            catch { }
            return resultado;
        }

        /// <summary>Elimina una partida y sus movimientos.</summary>
        public static bool EliminarPartida(int serverId)
        {
            if (serverId <= 0) return false;
            try
            {
                Delete(BaseUrl + "/movimientos?id_partida=eq." + serverId);
                return Delete(BaseUrl + "/partidas?id_partida=eq." + serverId);
            }
            catch { return false; }
        }

        public static bool EliminarMovimiento(int serverId)
        {
            if (serverId <= 0) return false;
            return Delete(BaseUrl + "/movimientos?id_movimiento=eq." + serverId);
        }

        /// <summary>Todos los movimientos de la base.</summary>
        public static List<PartidaRepository.Movimiento> ListarTodosMovimientos()
        {
            var resultado = new List<PartidaRepository.Movimiento>();
            try
            {
                var list = Get<List<PartidaRepository.Movimiento>>(
                    BaseUrl + "/movimientos?select=*&order=id_movimiento.desc");
                if (list != null) resultado = list;
            }
            catch { }
            return resultado;
        }
    }
}
