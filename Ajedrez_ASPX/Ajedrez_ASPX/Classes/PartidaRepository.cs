using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Fachada de acceso a los datos de partidas. En la versión web trabaja
    /// SIEMPRE contra Supabase (no hay base local): crear/finalizar partidas,
    /// guardar el estado del tablero, calcular estadísticas y actualizar el ELO.
    /// Mantiene la misma API que la clase del proyecto de escritorio.
    /// </summary>
    public static class PartidaRepository
    {
        private const double ELO_POR_DEFECTO = 800.0;  // ELO inicial estilo chess.com/lichess
        private const int ELO_MINIMO = 100;
        private const int PARTIDAS_PARA_K_MEDIO = 30;  // FIDE: 30 partidas para K=20
        private const double UMBRAL_K_BAJO = 2400.0;   // FIDE: rating >= 2400 usa K=10

        /// <summary>
        /// Factor K según reglas FIDE oficiales:
        /// - K=40 para jugadores nuevos (menos de 30 partidas) → crecimiento rápido
        /// - K=20 para jugadores establecidos (30+ partidas, rating &lt; 2400)
        /// - K=10 para maestros (rating ≥ 2400) → movimiento mínimo
        /// </summary>
        private static double ObtenerKFactor(int partidasJugadas, double elo)
        {
            if (partidasJugadas < PARTIDAS_PARA_K_MEDIO) return 40.0;
            if (elo >= UMBRAL_K_BAJO) return 10.0;
            return 20.0;
        }

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

        // ---------------- Partidas ----------------

        /// <summary>Crea una partida nueva y devuelve su id (o -1 si falla).</summary>
        public static int CrearPartida(string tipo, string blancas, string negras, int? idUsuario, bool soplar = false)
        {
            try
            {
                return Nube.InsertarPartida(blancas, negras, tipo, soplar, idUsuario,
                                            "en_curso", DateTime.Now, null, null);
            }
            catch { return -1; }
        }

        /// <summary>Marca la partida como finalizada con su ganador.</summary>
        public static bool FinalizarPartida(int idPartida, string ganador)
        {
            if (idPartida < 0) return false;
            try
            {
                // Si no hay ganador (partida abandonada) se envía solo el estado.
                return Nube.ActualizarPartida(idPartida, "finalizada", ganador, DateTime.Now, false);
            }
            catch { return false; }
        }

        /// <summary>Partidas del usuario (opcionalmente filtradas por tipo).</summary>
        public static List<Partida> ListarPartidas(string tipo, string usuario)
        {
            return Nube.ListarPartidas(tipo, usuario);
        }

        /// <summary>Estadísticas de un usuario calculadas desde sus partidas.</summary>
        public class Estadisticas
        {
            public int Ganadas { get; set; }
            public int Perdidas { get; set; }
            public int Empates { get; set; }
            public int Total { get; set; }
        }

        /// <summary>
        /// Calcula las estadísticas del usuario a partir de sus partidas finalizadas.
        /// Es la misma fuente que usan las ventanas de Partidas, Perfil y el menú,
        /// para que todas muestren siempre los mismos números.
        /// </summary>
        public static Estadisticas EstadisticasDe(string usuario)
        {
            var e = new Estadisticas();
            if (string.IsNullOrWhiteSpace(usuario)) return e;

            foreach (var p in ListarPartidas(null, usuario))
            {
                if (!string.Equals(p.estado, "finalizada", StringComparison.OrdinalIgnoreCase)) continue;

                e.Total++;
                if (string.IsNullOrWhiteSpace(p.ganador)
                    || string.Equals(p.ganador, "empate", StringComparison.OrdinalIgnoreCase))
                    e.Empates++;
                else if (string.Equals(p.ganador, usuario, StringComparison.OrdinalIgnoreCase))
                    e.Ganadas++;
                else
                    e.Perdidas++;
            }
            return e;
        }

        /// <summary>Busca una partida en curso del usuario para poder continuarla.</summary>
        public static Partida BuscarPartidaEnCurso(string tipo, string usuario)
        {
            return Nube.BuscarPartidaEnCurso(tipo, usuario);
        }

        // ---------------- Estado (continuar partida) ----------------

        public static Movimiento ObtenerMovimiento(int idPartida)
        {
            return Nube.MovimientoPorPartida(idPartida);
        }

        /// <summary>
        /// Igual que ObtenerMovimiento, pero distingue los dos casos que
        /// producen null: "no hay estado guardado" (devuelve true y mov = null)
        /// y "no se pudo consultar" (devuelve false).
        /// </summary>
        public static bool ObtenerMovimientoSeguro(int idPartida, out Movimiento mov)
        {
            mov = null;
            try
            {
                mov = Nube.MovimientoPorPartida(idPartida);
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
                var existente = Nube.MovimientoPorPartida(idPartida);
                if (existente != null && existente.id_movimiento > 0)
                    return Nube.ActualizarMovimiento(existente.id_movimiento, numMovimientos,
                                                     turnoFinal, ultimoMovimiento, tableroJson);

                return Nube.InsertarMovimiento(idPartida, numMovimientos, turnoFinal,
                                               ultimoMovimiento, tableroJson) > 0;
            }
            catch { return false; }
        }

        // ---------------- Usuarios y ELO ----------------

        /// <summary>Devuelve el id_usuario registrado o null si es un invitado.</summary>
        public static int? ObtenerIdUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            if (_idUsuarioCache.TryGetValue(nombre, out int id)) return id;

            var u = Nube.ObtenerUsuario(nombre);
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
                var ua = Nube.ObtenerUsuario(jugadorA);
                var ub = Nube.ObtenerUsuario(jugadorB);

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

        private static void ActualizarUsuario(UsuarioNube u, double propio, double rival, double s)
        {
            double k = ObtenerKFactor(u.partidas_jugadas, propio);
            double esperado = 1.0 / (1.0 + Math.Pow(10.0, (rival - propio) / 400.0));
            int nuevoElo = (int)Math.Round(propio + k * (s - esperado));
            if (nuevoElo < ELO_MINIMO) nuevoElo = ELO_MINIMO;

            u.elo = nuevoElo;
            u.partidas_jugadas += 1;
            u.partidas_ganadas += (s == 1.0 ? 1 : 0);
            u.partidas_empatadas += (s == 0.5 ? 1 : 0);
            u.partidas_perdidas += (s == 0.0 ? 1 : 0);

            Nube.ActualizarUsuarioNube(u);
        }
    }
}
