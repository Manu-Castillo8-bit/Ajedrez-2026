using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ajedrez
{
    /// <summary>
    /// Fachada de acceso a los datos de partidas. Trabaja SIEMPRE contra la base
    /// local SQLite (para que el juego funcione sin conexión) y, tras cada
    /// escritura, lanza una sincronización en segundo plano con Supabase.
    ///
    /// Los identificadores que se exponen a las ventanas son los de la fila
    /// local (siempre positivos); el id real de Supabase se guarda aparte y solo
    /// lo usa el <see cref="Sincronizador"/>.
    /// </summary>
    public static class PartidaRepository
    {
        private const double K_FACTOR = 32.0;          // Factor K estándar
        private const double ELO_POR_DEFECTO = 1200.0; // ELO de un invitado sin cuenta
        private const int ELO_MINIMO = 100;

        private static readonly Dictionary<string, int> _idUsuarioCache = new Dictionary<string, int>();

        // ---------------- Modelos expuestos a la interfaz ----------------

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

        // ---------------- Utilidades de mapeo ----------------

        private static Partida Mapear(PartidaLocal p)
        {
            if (p == null) return null;
            return new Partida
            {
                id_partida = p.IdLocal,
                jugador_blancas = p.JugadorBlancas,
                jugador_negras = p.JugadorNegras,
                ganador = p.Ganador,
                estado = p.Estado,
                modo = p.Modo,
                soplar = p.Soplar,
                tipo_juego = p.TipoJuego,
                id_usuario = p.IdUsuario,
                fecha_inicio = p.FechaInicio,
                fecha_fin = p.FechaFin
            };
        }

        private static Movimiento Mapear(MovimientoLocal m)
        {
            if (m == null) return null;
            JToken tablero = null;
            if (!string.IsNullOrWhiteSpace(m.Tablero))
            {
                try { tablero = JToken.Parse(m.Tablero); }
                catch { tablero = null; }
            }
            return new Movimiento
            {
                id_movimiento = m.IdLocal,
                id_partida = m.IdPartidaLocal,
                num_movimientos = m.NumMovimientos,
                turno_final = m.TurnoFinal,
                movimiento_final = m.MovimientoFinal,
                tablero = tablero
            };
        }

        private static void Sincronizar()
        {
            Sincronizador.EnSegundoPlano(Sesion.NombreUsuario);
        }

        // ---------------- Partidas ----------------

        /// <summary>Crea una partida nueva y devuelve su id local (o -1 si falla).</summary>
        public static int CrearPartida(string tipo, string blancas, string negras, int? idUsuario, bool soplar = false)
        {
            try
            {
                var p = new PartidaLocal
                {
                    JugadorBlancas = blancas,
                    JugadorNegras = negras,
                    TipoJuego = tipo,
                    Estado = "en_curso",
                    Soplar = soplar,
                    IdUsuario = idUsuario,
                    FechaInicio = DateTime.Now,
                    SyncState = EstadoSync.Pendiente
                };
                int id = BaseDatosLocal.InsertarPartida(p);
                if (id > 0) Sincronizar();
                return id;
            }
            catch { return -1; }
        }

        /// <summary>Marca la partida como finalizada con su ganador.</summary>
        public static bool FinalizarPartida(int idPartida, string ganador)
        {
            if (idPartida < 0) return false;
            try
            {
                var p = BaseDatosLocal.PartidaPorIdLocal(idPartida);
                if (p == null) return false;

                p.Estado = "finalizada";
                p.FechaFin = DateTime.Now;
                // Si no hay ganador (partida abandonada) se deja el campo sin tocar.
                if (!string.IsNullOrWhiteSpace(ganador)) p.Ganador = ganador;
                p.SyncState = EstadoSync.Pendiente;

                BaseDatosLocal.ActualizarPartida(p);
                Sincronizar();
                return true;
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
                foreach (var p in BaseDatosLocal.ListarPartidas(tipo, usuario))
                    resultado.Add(Mapear(p));
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
                return Mapear(BaseDatosLocal.BuscarPartidaEnCurso(tipo, usuario));
            }
            catch { return null; }
        }

        // ---------------- Estado (continuar partida) ----------------

        public static Movimiento ObtenerMovimiento(int idPartida)
        {
            try
            {
                return Mapear(BaseDatosLocal.MovimientoPorPartida(idPartida));
            }
            catch { return null; }
        }

        /// <summary>
        /// Igual que ObtenerMovimiento, pero distingue los dos casos que
        /// producen null: "no hay estado guardado" (devuelve true y mov = null)
        /// y "no se pudo consultar" (devuelve false). Así, ante un fallo local no
        /// se cierra por error una partida que sí se podía continuar.
        /// </summary>
        public static bool ObtenerMovimientoSeguro(int idPartida, out Movimiento mov)
        {
            mov = null;
            if (!BaseDatosLocal.Disponible)
                return false; // no se pudo consultar

            try
            {
                mov = Mapear(BaseDatosLocal.MovimientoPorPartida(idPartida));
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
            if (!BaseDatosLocal.Disponible) return false;
            try
            {
                BaseDatosLocal.GuardarMovimiento(idPartida, numMovimientos, turnoFinal,
                                                 ultimoMovimiento, tableroJson);
                Sincronizar();
                return true;
            }
            catch { return false; }
        }

        // ---------------- Usuarios y ELO ----------------

        /// <summary>Devuelve el id_usuario registrado o null si es un invitado.</summary>
        public static int? ObtenerIdUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            if (_idUsuarioCache.TryGetValue(nombre, out int id)) return id;

            var local = BaseDatosLocal.ObtenerUsuario(nombre);
            if (local != null && local.ServerId.HasValue)
            {
                _idUsuarioCache[nombre] = local.ServerId.Value;
                return local.ServerId.Value;
            }

            var u = Nube.ObtenerUsuario(nombre);
            if (u == null) return null;

            if (local == null) BaseDatosLocal.InsertarUsuarioDesdeServidor(u);
            _idUsuarioCache[nombre] = u.id_usuario;
            return u.id_usuario;
        }

        private static UsuarioLocal AsegurarUsuario(string nombre)
        {
            var local = BaseDatosLocal.ObtenerUsuario(nombre);
            if (local != null) return local;

            var su = Nube.ObtenerUsuario(nombre);
            if (su == null) return null;

            BaseDatosLocal.InsertarUsuarioDesdeServidor(su);
            return BaseDatosLocal.ObtenerUsuario(nombre);
        }

        /// <summary>
        /// Registra el resultado de la partida y actualiza el ELO y las
        /// estadísticas de los jugadores que tengan cuenta registrada. El cálculo
        /// se hace contra la copia local para que funcione sin conexión; el
        /// resultado se sube al servidor en la siguiente sincronización.
        /// <paramref name="ganador"/> puede ser el nombre de A, de B o "empate".
        /// </summary>
        public static void RegistrarResultado(string jugadorA, string jugadorB, string ganador)
        {
            if (string.IsNullOrWhiteSpace(jugadorA) || string.IsNullOrWhiteSpace(jugadorB)) return;
            if (string.Equals(jugadorA, jugadorB, StringComparison.OrdinalIgnoreCase)) return;

            try
            {
                var ua = AsegurarUsuario(jugadorA);
                var ub = AsegurarUsuario(jugadorB);

                double eloA = ua != null ? ua.Elo : ELO_POR_DEFECTO;
                double eloB = ub != null ? ub.Elo : ELO_POR_DEFECTO;

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

                Sincronizar();
            }
            catch
            {
                // El ELO es secundario: si falla no debe cerrar el juego.
            }
        }

        private static void ActualizarUsuario(UsuarioLocal u, double propio, double rival, double s)
        {
            double esperado = 1.0 / (1.0 + Math.Pow(10.0, (rival - propio) / 400.0));
            int nuevoElo = (int)Math.Round(propio + K_FACTOR * (s - esperado));
            if (nuevoElo < ELO_MINIMO) nuevoElo = ELO_MINIMO;

            u.Elo = nuevoElo;
            u.PartidasJugadas += 1;
            u.PartidasGanadas += (s == 1.0 ? 1 : 0);
            u.PartidasEmpatadas += (s == 0.5 ? 1 : 0);
            u.PartidasPerdidas += (s == 0.0 ? 1 : 0);
            u.SyncState = EstadoSync.Pendiente;

            BaseDatosLocal.GuardarUsuario(u);
        }
    }
}
