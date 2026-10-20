using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ajedrez_ASPX
{
    /// <summary>Un paso de un movimiento (simple o salto de captura).</summary>
    [Serializable]
    public class Paso
    {
        public int R, C;
        public int CapR = -1, CapC = -1;
        public Paso() { }
        public Paso(int r, int c, int cr, int cc) { R = r; C = c; CapR = cr; CapC = cc; }
    }

    /// <summary>Movimiento completo de una ficha (varios pasos si es captura múltiple).</summary>
    [Serializable]
    public class Mov
    {
        public int R, C;
        public List<Paso> Pasos = new List<Paso>();
        public bool EsCaptura { get { return Pasos.Any(p => p.CapR >= 0); } }
    }

    /// <summary>Casilla del tablero (fila R, columna C).</summary>
    [Serializable]
    public class Casilla
    {
        public int R, C;
        public Casilla() { }
        public Casilla(int r, int c) { R = r; C = c; }
    }

    /// <summary>Instantánea del tablero para poder deshacer (BoardSnapshot del escritorio).</summary>
    [Serializable]
    public class FichaSnapshot
    {
        public int[,] Tablero;
        public int Turno;
        public int Blancas;
        public int Negras;
        public bool FinPartida;
    }

    /// <summary>
    /// Estado completo de la partida de damas. Vive en Session["EstadoDamas"].
    /// </summary>
    [Serializable]
    public class DamasEstado
    {
        // 1 = ficha blanca / 2 = dama blanca; -1 = ficha negra / -2 = dama negra; 0 = vacía
        public int[,] Tablero = new int[8, 8];

        public int Turno = 1; // 1 = blancas (arriba), -1 = negras (abajo)
        public string NombreBlancas = "Jugador";
        public string NombreNegras = "Oponente";
        public bool ModoSoplar = false; // true = se sopla la pieza que podía comer
        public bool FinPartida = false;
        public int Blancas = 0; // puntos de las blancas (fichas negras comidas)
        public int Negras = 0;  // puntos de las negras (fichas blancas comidas)

        public int IdPartida = -1;
        public int NumMovimientos = 0;
        public string UltimoMovimiento = "";

        // Selección / resaltado / capturas encadenadas
        public List<Casilla> Marcados = new List<Casilla>();
        public bool EnCapturaMultiple = false;
        public List<List<Paso>> SecuenciasPendientes = null;
        public List<Mov> MovimientosSimplesActuales = null;
        public int PiezaR = -1, PiezaC = -1, PiezaV = 0;
        public int SelR = -1, SelC = -1; // casilla de la ficha seleccionada (resaltado verde)
        public bool SnapshotTomado = false;
        public bool PuedeDeshacer = false; // solo se puede deshacer el último movimiento
        public Stack<FichaSnapshot> Historial = new Stack<FichaSnapshot>();

        // Último movimiento (resaltado informativo de la versión web)
        public int UltimoOrigenR = -1, UltimoOrigenC = -1;
        public int UltimoDestinoR = -1, UltimoDestinoC = -1;

        // Aviso tipo MessageBox y casillas sopladas (resaltado rojo)
        public string Aviso = "";
        public List<Casilla> Sopladas = new List<Casilla>();
    }

    /// <summary>
    /// Motor de damas. Reglas portadas exactamente desde Form1.cs del escritorio:
    /// disposición inicial, movimientos simples, capturas obligatorias (salvo en
    /// modo soplar), cadenas de captura más largas, coronación y damas voladoras.
    /// </summary>
    public static class DamasGame
    {
        public const int N = 8;

        /// <summary>Disposición inicial: blancas (1) arriba en filas 0-2, negras (-1) abajo en filas 5-7, solo en casillas oscuras.</summary>
        public static void TableroInicial(int[,] b)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    b[i, j] = 0;
                    bool casillaOscura = ((i + j) % 2 == 0);
                    if (casillaOscura && i < 3)
                        b[i, j] = 1;   // blancas (arriba)
                    else if (casillaOscura && i > 4)
                        b[i, j] = -1;  // negras (abajo)
                }
            }
        }

        public static bool Dentro(int r, int c)
        {
            return r >= 0 && r < N && c >= 0 && c < N;
        }

        /// <summary>
        /// Movimientos legales de un lado. Si hay capturas, solo se ofrecen las
        /// de longitud máxima (captura obligatoria); en modo soplar también se
        /// ofrecen movimientos simples (si no comes, se sopla la pieza).
        /// </summary>
        public static List<Mov> MovimientosDeLado(int[,] b, int side, bool modoSoplar)
        {
            var capturas = new List<Mov>();
            var simples = new List<Mov>();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (b[i, j] == 0 || Math.Sign(b[i, j]) != side) continue;

                    List<List<Paso>> caps = CapturasDePieza(b, i, j);
                    if (caps.Count > 0)
                    {
                        foreach (var seq in caps)
                            capturas.Add(new Mov { R = i, C = j, Pasos = seq });
                    }
                    else
                    {
                        foreach (var p in MovimientosSimplesDePieza(b, i, j))
                            simples.Add(new Mov { R = i, C = j, Pasos = new List<Paso> { p } });
                    }
                }
            }

            if (capturas.Count > 0)
            {
                int max = capturas.Max(m => m.Pasos.Count(p => p.CapR >= 0));
                var mejores = capturas.Where(m => m.Pasos.Count(p => p.CapR >= 0) == max).ToList();

                // Modo normal: la captura es obligatoria y solo se ofrecen capturas.
                // Modo soplar: también se ofrecen movimientos simples.
                if (!modoSoplar)
                    return mejores;

                mejores.AddRange(simples);
                return mejores;
            }

            return simples;
        }

        /// <summary>Todas las secuencias de captura (posibles cadenas) de una pieza.</summary>
        public static List<List<Paso>> CapturasDePieza(int[,] b, int r, int c)
        {
            var salida = new List<List<Paso>>();
            int v = b[r, c];
            if (v == 0) return salida;
            GenerarCapturasRec(b, r, c, v, new List<Paso>(), salida);
            return salida;
        }

        private static void GenerarCapturasRec(int[,] b, int r, int c, int v, List<Paso> acum, List<List<Paso>> salida)
        {
            List<Paso> saltos = SaltosSimples(b, r, c, v);

            if (saltos.Count == 0)
            {
                if (acum.Count > 0) salida.Add(new List<Paso>(acum));
                return;
            }

            foreach (Paso p in saltos)
            {
                int guardado = b[p.CapR, p.CapC];
                b[p.CapR, p.CapC] = 0;
                b[r, c] = 0;
                b[p.R, p.C] = v;
                acum.Add(p);

                bool corona = Math.Abs(v) == 1 && ((v > 0 && p.R == 7) || (v < 0 && p.R == 0));
                if (corona)
                    salida.Add(new List<Paso>(acum)); // la coronación termina la captura
                else
                    GenerarCapturasRec(b, p.R, p.C, v, acum, salida);

                acum.RemoveAt(acum.Count - 1);
                b[p.R, p.C] = 0;
                b[r, c] = v;
                b[p.CapR, p.CapC] = guardado;
            }
        }

        private static List<Paso> SaltosSimples(int[,] b, int r, int c, int v)
        {
            var res = new List<Paso>();
            int s = Math.Sign(v);
            int[] deltas = new int[] { -1, 1 };

            if (Math.Abs(v) == 1)
            {
                // Peón: solo come hacia adelante; solo la dama coronada come hacia atrás
                int dr = s;
                foreach (int dc in deltas)
                {
                    int mr = r + dr, mc = c + dc, tr = r + 2 * dr, tc = c + 2 * dc;
                    if (!Dentro(tr, tc)) continue;
                    if (b[mr, mc] != 0 && Math.Sign(b[mr, mc]) == -s && b[tr, tc] == 0)
                        res.Add(new Paso(tr, tc, mr, mc));
                }
            }
            else
            {
                // Dama (rey volador): busca la primera pieza en la diagonal
                foreach (int dr in deltas)
                {
                    foreach (int dc in deltas)
                    {
                        int rr = r + dr, cc = c + dc;
                        while (Dentro(rr, cc) && b[rr, cc] == 0) { rr += dr; cc += dc; }
                        if (!Dentro(rr, cc)) continue;
                        if (Math.Sign(b[rr, cc]) != -s) continue; // debe ser enemiga

                        int lr = rr + dr, lc = cc + dc;
                        while (Dentro(lr, lc) && b[lr, lc] == 0)
                        {
                            res.Add(new Paso(lr, lc, rr, cc));
                            lr += dr; lc += dc;
                        }
                    }
                }
            }

            return res;
        }

        private static List<Paso> MovimientosSimplesDePieza(int[,] b, int r, int c)
        {
            var res = new List<Paso>();
            int v = b[r, c];
            if (v == 0) return res;
            int s = Math.Sign(v);
            int[] deltas = new int[] { -1, 1 };

            if (Math.Abs(v) == 1)
            {
                foreach (int dc in deltas)
                {
                    int nr = r + s, nc = c + dc;
                    if (Dentro(nr, nc) && b[nr, nc] == 0)
                        res.Add(new Paso(nr, nc, -1, -1));
                }
            }
            else
            {
                foreach (int dr in deltas)
                {
                    foreach (int dc in deltas)
                    {
                        int rr = r + dr, cc = c + dc;
                        while (Dentro(rr, cc) && b[rr, cc] == 0)
                        {
                            res.Add(new Paso(rr, cc, -1, -1));
                            rr += dr; cc += dc;
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>Piezas de un lado que pueden comer en el tablero actual (modo soplar).</summary>
        public static List<Casilla> PiezasQuePuedenComer(int[,] b, int side)
        {
            var res = new List<Casilla>();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (b[i, j] != 0 && Math.Sign(b[i, j]) == side && CapturasDePieza(b, i, j).Count > 0)
                        res.Add(new Casilla(i, j));
            return res;
        }

        /// <summary>Notación de la casilla: por ejemplo "a3".</summary>
        public static string NombreCasilla(int r, int c)
        {
            return ((char)('a' + c)).ToString() + (N - r);
        }

        /// <summary>Ganador según las piezas comidas (empate si igualan).</summary>
        public static string GanadorPorPiezasComidas(DamasEstado est)
        {
            if (est.Blancas > est.Negras) return est.NombreBlancas;
            if (est.Negras > est.Blancas) return est.NombreNegras;
            return "empate";
        }

        /// <summary>turno_final solo admite 'blancas' o 'negras'.</summary>
        public static string TurnoTexto(int turno)
        {
            return turno == 1 ? "blancas" : "negras";
        }

        /// <summary>
        /// Serializa el estado del tablero con el MISMO esquema del escritorio
        /// (Form1.SerializarTablero), para que las partidas guardadas sean
        /// compatibles entre las dos versiones.
        /// </summary>
        public static string SerializarTablero(DamasEstado est)
        {
            int[] t = new int[N * N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    t[i * N + j] = est.Tablero[i, j];

            JObject o = new JObject
            {
                ["turno"] = est.Turno,
                ["blancas"] = est.Blancas,
                ["negras"] = est.Negras,
                ["soplar"] = est.ModoSoplar,
                ["t"] = new JArray(t)
            };
            return o.ToString(Formatting.None);
        }

        /// <summary>Restaura el estado guardado (igual que Form1.AplicarEstado).</summary>
        public static void AplicarEstado(DamasEstado est, string json)
        {
            try
            {
                JObject o = JObject.Parse(json);
                est.Turno = o["turno"] != null ? (int)o["turno"] : 1;
                est.Blancas = o["blancas"] != null ? (int)o["blancas"] : 0;
                est.Negras = o["negras"] != null ? (int)o["negras"] : 0;

                // Restaurar si en esta partida se podía soplar (regla de soplar)
                if (o["soplar"] != null) est.ModoSoplar = (bool)o["soplar"];

                JArray arr = (JArray)o["t"];
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                        est.Tablero[i, j] = (int)arr[i * N + j];

                est.FinPartida = false;
            }
            catch { }
        }
    }
}
