using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ajedrez_ASPX
{
    /// <summary>Información de una pieza del tablero.</summary>
    public class PiezaInfo
    {
        public string Color { get; set; }
        public string Tipo { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public bool SeHaMovido { get; set; }

        public PiezaInfo(string color, string tipo, int fila, int columna, bool seHaMovido = false)
        {
            Color = color;
            Tipo = tipo;
            Fila = fila;
            Columna = columna;
            SeHaMovido = seHaMovido;
        }

        public PiezaInfo Clon()
        {
            return new PiezaInfo(Color, Tipo, Fila, Columna, SeHaMovido);
        }
    }

    /// <summary>Historial de una jugada para poder deshacerla.</summary>
    public class MovHistorial
    {
        public Point Origen { get; set; }
        public Point Destino { get; set; }
        public PiezaInfo PiezaCapturada { get; set; }
        public PiezaInfo PiezaMovida { get; set; }
        public bool EsEnroque { get; set; }
        public Point TorreOrigen { get; set; }
        public Point TorreDestino { get; set; }
        public PiezaInfo TorreInfo { get; set; }
        public bool EsAlPaso { get; set; }
        public Point VictimaPos { get; set; }
        public PiezaInfo VictimaInfo { get; set; }
        public Point? AlPasoAnterior { get; set; }
        public int HalfmoveAnterior { get; set; }
        public int FullmoveAnterior { get; set; }
    }

    /// <summary>
    /// Motor de reglas y estado de la partida de ajedrez. Puerto completo de
    /// la lógica del formulario Ajedrez_tablero del proyecto de escritorio,
    /// sin dependencias de Windows Forms: todo el juego vive aquí y la página
    /// solo pinta y gestiona los eventos.
    /// </summary>
    public class AjedrezGame
    {
        public const int BOARD_SIZE = 8;

        public PiezaInfo[,] Casillas;
        public string TurnoActual = "white"; // "white" o "black"
        public int BlancasCapturaron = 0;    // piezas negras que comió el bando blanco
        public int NegrasCapturaron = 0;     // piezas blancas que comió el bando negro
        public List<string> CapturadasPorBlancas = new List<string>(); // tipos de las piezas negras comidaas
        public List<string> CapturadasPorNegras = new List<string>();  // tipos de las piezas blancas comidaas

        public bool HayAlPaso = false;       // objetivo de captura al paso
        public int AlPasoF = -1;
        public int AlPasoC = -1;
        public int MedioReloj = 0;           // medios movimientos sin peón ni captura
        public int NumeroJugada = 1;         // número de jugada (FEN)
        public List<string> HistorialPosiciones = new List<string>(); // claves para 3 repeticiones
        public List<string> Notaciones = new List<string>();          // jugadas en notación SAN

        public bool PartidaFinalizada = false;
        public string MotivoFin = null;
        public string MotivoGanadorFin = "empate";

        // Datos de la última jugada para el label de consejos
        public bool UltimoFueCaptura = false;
        public bool UltimoFueAlPaso = false;
        public bool UltimoFueCoronacion = false;
        public bool UltimoFueEnroqueCorto = false;
        public bool UltimoFueEnroqueLargo = false;
        public string UltimaNotacion = null;

        // Resaltado del tablero
        public int JaqueF = -1;
        public int JaqueC = -1;
        public int MateF = -1;
        public int MateC = -1;

        public Stack<MovHistorial> HistorialMovimientos = new Stack<MovHistorial>();
        public bool PuedeDeshacer = false;

        public int NumMovimientos = 0;
        public string UltimoMovimiento = "";

        // Inteligencia artificial (Stockfish)
        public bool ModoIA = false;
        public string ColorIA = "black";
        public int NivelIA = 5;

        // Nombres de los jugadores (los mantiene la página, como en el escritorio)
        public string NombreBlancas = "Blancas";
        public string NombreNegras = "Negras";

        public AjedrezGame()
        {
            Casillas = new PiezaInfo[BOARD_SIZE, BOARD_SIZE];
            ColocarPiezasIniciales();
            HistorialPosiciones.Add(PosicionClave());
        }

        // ----- Configuración y utilidades comunes -----

        public static string ColorTexto(string color)
        {
            return color == "white" ? "Blancas" : "Negras";
        }

        private static string LetraSan(string tipo)
        {
            switch (tipo)
            {
                case "king": return "R";
                case "queen": return "D";
                case "rook": return "T";
                case "bishop": return "A";
                case "knight": return "C";
                default: return "";
            }
        }

        public static string NombreCasilla(Point p)
        {
            return ((char)('a' + p.Y)).ToString() + (8 - p.X);
        }

        public static Point PuntoDeCasilla(string casilla)
        {
            int col = casilla[0] - 'a';
            int row = 8 - (casilla[1] - '0');
            return new Point(row, col);
        }

        private string Notacion(Point from, Point to)
        {
            return NombreCasilla(from) + "-" + NombreCasilla(to);
        }

        /// <summary>Glifo Unicode de la pieza, igual para escritorio y web.</summary>
        public static string Glifo(string color, string tipo)
        {
            if (color == "black")
            {
                switch (tipo)
                {
                    case "rook": return "♜";
                    case "knight": return "♞";
                    case "bishop": return "♝";
                    case "queen": return "♛";
                    case "king": return "♚";
                    default: return "♟";
                }
            }
            else
            {
                switch (tipo)
                {
                    case "rook": return "♖";
                    case "knight": return "♘";
                    case "bishop": return "♗";
                    case "queen": return "♕";
                    case "king": return "♔";
                    default: return "♙";
                }
            }
        }

        public bool EnLimites(int row, int col)
        {
            return row >= 0 && row < BOARD_SIZE && col >= 0 && col < BOARD_SIZE;
        }

        public Point BuscarRey(string color)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    PiezaInfo info = Casillas[i, j];
                    if (info != null && info.Tipo == "king" && info.Color == color)
                        return new Point(i, j);
                }
            }
            return new Point(-1, -1);
        }

        /// <summary>¿Está el rey del color indicado atacado?</summary>
        public bool ReyEnJaque(string color)
        {
            Point rey = BuscarRey(color);
            if (rey.X == -1) return false;
            string enemigo = color == "white" ? "black" : "white";
            return CasillaAtacada(rey, enemigo);
        }

        private bool IsInCheck(Point kingPos)
        {
            if (kingPos.X == -1) return false;
            string enemyColor = TurnoActual == "white" ? "black" : "white";
            return CasillaAtacada(kingPos, enemyColor);
        }

        // Verifica si la casilla es atacada por alguna pieza del color indicado
        private bool CasillaAtacada(Point sq, string byColor)
        {
            int r = sq.X, c = sq.Y;

            // Peones: un peón de byColor ataca en diagonal según su sentido de avance
            int pawnBack = byColor == "white" ? 1 : -1;
            foreach (int dc in new int[] { -1, 1 })
            {
                int pr = r + pawnBack, pc = c + dc;
                if (EnLimites(pr, pc))
                {
                    PiezaInfo pi = Casillas[pr, pc];
                    if (pi != null && pi.Color == byColor && pi.Tipo == "pawn")
                        return true;
                }
            }

            // Caballos
            int[,] knightOffsets = { { -2, -1 }, { -2, 1 }, { -1, -2 }, { -1, 2 },
                                     { 1, -2 }, { 1, 2 }, { 2, -1 }, { 2, 1 } };
            for (int i = 0; i < 8; i++)
            {
                int nr = r + knightOffsets[i, 0], nc = c + knightOffsets[i, 1];
                if (EnLimites(nr, nc))
                {
                    PiezaInfo pi = Casillas[nr, nc];
                    if (pi != null && pi.Color == byColor && pi.Tipo == "knight")
                        return true;
                }
            }

            // Rey
            int[,] kingOffsets = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 },
                                   { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
            for (int i = 0; i < 8; i++)
            {
                int nr = r + kingOffsets[i, 0], nc = c + kingOffsets[i, 1];
                if (EnLimites(nr, nc))
                {
                    PiezaInfo pi = Casillas[nr, nc];
                    if (pi != null && pi.Color == byColor && pi.Tipo == "king")
                        return true;
                }
            }

            // Torre / Reina (líneas rectas)
            int[,] orth = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            for (int d = 0; d < 4; d++)
            {
                int rr = r + orth[d, 0], cc = c + orth[d, 1];
                while (EnLimites(rr, cc))
                {
                    PiezaInfo pi = Casillas[rr, cc];
                    if (pi != null)
                    {
                        if (pi.Color == byColor && (pi.Tipo == "rook" || pi.Tipo == "queen"))
                            return true;
                        break;
                    }
                    rr += orth[d, 0];
                    cc += orth[d, 1];
                }
            }

            // Alfil / Reina (diagonales)
            int[,] diag = { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
            for (int d = 0; d < 4; d++)
            {
                int rr = r + diag[d, 0], cc = c + diag[d, 1];
                while (EnLimites(rr, cc))
                {
                    PiezaInfo pi = Casillas[rr, cc];
                    if (pi != null)
                    {
                        if (pi.Color == byColor && (pi.Tipo == "bishop" || pi.Tipo == "queen"))
                            return true;
                        break;
                    }
                    rr += diag[d, 0];
                    cc += diag[d, 1];
                }
            }

            return false;
        }

        /// <summary>Simula el movimiento y comprueba si deja al rey propio en jaque.</summary>
        private bool MovimientoDejaEnJaque(Point from, Point to, string color)
        {
            PiezaInfo tagOrigen = Casillas[from.X, from.Y];
            PiezaInfo tagDestino = Casillas[to.X, to.Y];

            PiezaInfo pieza = tagOrigen;
            string tipo = pieza != null ? pieza.Tipo : "queen";

            // Si es una captura al paso hay que quitar también el peón víctima
            bool quitarVictima = pieza != null && pieza.Tipo == "pawn"
                                 && HayAlPaso && to.X == AlPasoF && to.Y == AlPasoC
                                 && Casillas[to.X, to.Y] == null;

            Point victimaPos = new Point(-1, -1);
            PiezaInfo victimaTag = null;
            if (quitarVictima)
            {
                int victimRow = to.X + (color == "white" ? 1 : -1);
                victimaPos = new Point(victimRow, to.Y);
                victimaTag = Casillas[victimRow, to.Y];
                Casillas[victimRow, to.Y] = null;
            }

            // Simular
            Casillas[to.X, to.Y] = new PiezaInfo(color, tipo, to.X, to.Y, true);
            Casillas[from.X, from.Y] = null;

            bool enJaque = ReyEnJaque(color);

            // Revertir
            Casillas[from.X, from.Y] = tagOrigen;
            Casillas[to.X, to.Y] = tagDestino;

            if (quitarVictima)
                Casillas[victimaPos.X, victimaPos.Y] = victimaTag;

            return enJaque;
        }

        // ----- Generación de movimientos -----

        public List<Point> MovimientosValidos(Point pos)
        {
            return MovimientosValidos(pos, true);
        }

        public List<Point> MovimientosValidos(Point pos, bool includeCastling)
        {
            List<Point> moves = new List<Point>();
            PiezaInfo info = Casillas[pos.X, pos.Y];
            if (info == null) return moves;

            switch (info.Tipo)
            {
                case "pawn":
                    moves.AddRange(MovimientosPeon(pos, info.Color));
                    break;
                case "rook":
                    moves.AddRange(MovimientosTorre(pos, info.Color));
                    break;
                case "knight":
                    moves.AddRange(MovimientosCaballo(pos, info.Color));
                    break;
                case "bishop":
                    moves.AddRange(MovimientosAlfil(pos, info.Color));
                    break;
                case "queen":
                    moves.AddRange(MovimientosReina(pos, info.Color));
                    break;
                case "king":
                    moves.AddRange(MovimientosRey(pos, info.Color));
                    if (includeCastling)
                        AgregarEnroques(moves, pos, info.Color);
                    break;
            }

            // Descartar movimientos que dejarían al propio rey en jaque
            List<Point> legales = new List<Point>();
            foreach (Point m in moves)
            {
                if (!MovimientoDejaEnJaque(pos, m, info.Color))
                    legales.Add(m);
            }

            return legales;
        }

        private List<Point> MovimientosPeon(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int direction = color == "white" ? -1 : 1;
            int startRow = color == "white" ? 6 : 1;

            // Movimiento hacia adelante
            int newRow = pos.X + direction;
            if (EnLimites(newRow, pos.Y) && Casillas[newRow, pos.Y] == null)
            {
                moves.Add(new Point(newRow, pos.Y));

                // Movimiento doble desde la posición inicial
                if (pos.X == startRow)
                {
                    int doubleRow = pos.X + 2 * direction;
                    if (Casillas[doubleRow, pos.Y] == null)
                        moves.Add(new Point(doubleRow, pos.Y));
                }
            }

            // Captura diagonal
            int[] captureCols = { pos.Y - 1, pos.Y + 1 };
            foreach (int col in captureCols)
            {
                if (EnLimites(newRow, col) && Casillas[newRow, col] != null)
                {
                    PiezaInfo targetInfo = Casillas[newRow, col];
                    if (targetInfo.Color != color)
                        moves.Add(new Point(newRow, col));
                }
            }

            // Captura al paso: solo si el peón rival acaba de avanzar dos casillas
            if (HayAlPaso
                && AlPasoF == newRow
                && Math.Abs(AlPasoC - pos.Y) == 1
                && EnLimites(newRow, AlPasoC)
                && Casillas[AlPasoF, AlPasoC] == null)
            {
                moves.Add(new Point(AlPasoF, AlPasoC));
            }

            return moves;
        }

        private List<Point> MovimientosTorre(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] directions = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int d = 0; d < 4; d++)
            {
                int row = pos.X + directions[d, 0];
                int col = pos.Y + directions[d, 1];

                while (EnLimites(row, col))
                {
                    if (Casillas[row, col] == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PiezaInfo targetInfo = Casillas[row, col];
                        if (targetInfo.Color != color)
                            moves.Add(new Point(row, col));
                        break;
                    }
                    row += directions[d, 0];
                    col += directions[d, 1];
                }
            }

            return moves;
        }

        private List<Point> MovimientosCaballo(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] offsets = { { -2, -1 }, { -2, 1 }, { -1, -2 }, { -1, 2 },
                               { 1, -2 }, { 1, 2 }, { 2, -1 }, { 2, 1 } };

            for (int i = 0; i < 8; i++)
            {
                int row = pos.X + offsets[i, 0];
                int col = pos.Y + offsets[i, 1];

                if (EnLimites(row, col))
                {
                    if (Casillas[row, col] == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PiezaInfo targetInfo = Casillas[row, col];
                        if (targetInfo.Color != color)
                            moves.Add(new Point(row, col));
                    }
                }
            }

            return moves;
        }

        private List<Point> MovimientosAlfil(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] directions = { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

            for (int d = 0; d < 4; d++)
            {
                int row = pos.X + directions[d, 0];
                int col = pos.Y + directions[d, 1];

                while (EnLimites(row, col))
                {
                    if (Casillas[row, col] == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PiezaInfo targetInfo = Casillas[row, col];
                        if (targetInfo.Color != color)
                            moves.Add(new Point(row, col));
                        break;
                    }
                    row += directions[d, 0];
                    col += directions[d, 1];
                }
            }

            return moves;
        }

        private List<Point> MovimientosReina(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            moves.AddRange(MovimientosTorre(pos, color));
            moves.AddRange(MovimientosAlfil(pos, color));
            return moves;
        }

        private List<Point> MovimientosRey(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] offsets = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 },
                               { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            for (int i = 0; i < 8; i++)
            {
                int row = pos.X + offsets[i, 0];
                int col = pos.Y + offsets[i, 1];

                if (EnLimites(row, col))
                {
                    if (Casillas[row, col] == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PiezaInfo targetInfo = Casillas[row, col];
                        if (targetInfo.Color != color)
                            moves.Add(new Point(row, col));
                    }
                }
            }

            return moves;
        }

        // ----- Enroques -----

        private void AgregarEnroques(List<Point> moves, Point pos, string color)
        {
            int homeRow = color == "white" ? 7 : 0;

            // El rey debe estar en su casilla inicial y no haberse movido
            if (pos.X != homeRow || pos.Y != 4) return;

            if (PuedeEnrocarRey(color))
                moves.Add(new Point(homeRow, 6));

            if (PuedeEnrocarDama(color))
                moves.Add(new Point(homeRow, 2));
        }

        private bool PuedeEnrocarRey(string color)
        {
            int row = color == "white" ? 7 : 0;

            // Rey y torre presentes, del color correcto y sin haberse movido
            if (!PiezaSinMover(row, 4, color, "king")) return false;
            if (!PiezaSinMover(row, 7, color, "rook")) return false;

            // Casillas intermedias vacías (columnas 5 y 6)
            if (Casillas[row, 5] != null || Casillas[row, 6] != null) return false;

            // El rey no puede estar en jaque ni pasar por casillas amenazadas
            if (IsInCheck(new Point(row, 4))) return false;
            if (IsInCheck(new Point(row, 5))) return false;
            if (IsInCheck(new Point(row, 6))) return false;

            return true;
        }

        private bool PuedeEnrocarDama(string color)
        {
            int row = color == "white" ? 7 : 0;

            // Rey y torre presentes, del color correcto y sin haberse movido
            if (!PiezaSinMover(row, 4, color, "king")) return false;
            if (!PiezaSinMover(row, 0, color, "rook")) return false;

            // Casillas intermedias vacías (columnas 1, 2 y 3)
            if (Casillas[row, 1] != null || Casillas[row, 2] != null || Casillas[row, 3] != null) return false;

            // El rey no puede estar en jaque ni pasar por casillas amenazadas
            if (IsInCheck(new Point(row, 4))) return false;
            if (IsInCheck(new Point(row, 3))) return false;
            if (IsInCheck(new Point(row, 2))) return false;

            return true;
        }

        private bool PiezaSinMover(int row, int col, string color, string type)
        {
            PiezaInfo info = Casillas[row, col];
            return info != null && info.Color == color && info.Tipo == type && !info.SeHaMovido;
        }

        /// <summary>¿Tiene el color indicado al menos un movimiento legal?</summary>
        public bool HayMovimientosLegales(string color)
        {
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PiezaInfo info = Casillas[r, c];
                    if (info != null && info.Color == color)
                    {
                        if (MovimientosValidos(new Point(r, c), false).Count > 0)
                            return true;
                    }
                }
            }
            return false;
        }

        // ----- Notación y fin de partida -----

        private string ConstruirSan(Point from, Point to, PiezaInfo pieza, bool captura,
                                    string promocion, bool enroque)
        {
            if (enroque)
                return to.Y == 6 ? "O-O" : "O-O-O";

            if (pieza.Tipo == "pawn")
            {
                string s = captura
                    ? ((char)('a' + from.Y)).ToString() + "x" + NombreCasilla(to)
                    : NombreCasilla(to);
                if (!string.IsNullOrEmpty(promocion))
                    s += "=" + LetraSan(promocion);
                return s;
            }

            return LetraSan(pieza.Tipo) + (captura ? "x" : "") + NombreCasilla(to);
        }

        /// <summary>
        /// Analiza la posición del bando al que le toca mover y decide si la partida
        /// termina (jaque mate o tablas). No reinicia el tablero.
        /// </summary>
        public bool EvaluarFinDePartida()
        {
            JaqueF = -1; JaqueC = -1;
            MateF = -1; MateC = -1;
            MotivoFin = null;
            MotivoGanadorFin = "empate";

            Point rey = BuscarRey(TurnoActual);
            bool enJaque = rey.X != -1 && IsInCheck(rey);

            if (enJaque && !HayMovimientosLegales(TurnoActual))
            {
                MateF = rey.X; MateC = rey.Y;
                string ganadorColor = TurnoActual == "white" ? "black" : "white";
                string ganador = ganadorColor == "white" ? NombreBlancas : NombreNegras;
                MotivoGanadorFin = ganador;
                MotivoFin = "¡Jaque mate! Ganan las " + ColorTexto(ganadorColor)
                            + " (" + ganador + ").\nEl rey queda marcado en rojo.";
                PartidaFinalizada = true;
                return true;
            }

            if (enJaque)
            {
                JaqueF = rey.X; JaqueC = rey.Y;
                return false;
            }

            if (!HayMovimientosLegales(TurnoActual))
            {
                MotivoFin = "¡Tablas! Empate por ahogado: no hay movimientos legales.";
                PartidaFinalizada = true;
                return true;
            }

            if (MedioReloj >= 100)
            {
                MotivoFin = "¡Tablas! Empate por la regla de los 50 movimientos.";
                PartidaFinalizada = true;
                return true;
            }

            if (HayTresRepeticiones())
            {
                MotivoFin = "¡Tablas! Empate por repetición de posición (3 veces).";
                PartidaFinalizada = true;
                return true;
            }

            if (MaterialInsuficiente())
            {
                MotivoFin = "¡Tablas! Empate por material insuficiente.";
                PartidaFinalizada = true;
                return true;
            }

            return false;
        }

        /// <summary>Al finalizar la partida, gana quien haya comido más piezas (empate si igualan).</summary>
        public string GanadorPorPiezasComidas(string nombreBlancas, string nombreNegras)
        {
            if (BlancasCapturaron > NegrasCapturaron) return nombreBlancas;
            if (NegrasCapturaron > BlancasCapturaron) return nombreNegras;
            return "empate";
        }

        public void ActualizarResaltadoJaque()
        {
            JaqueF = -1; JaqueC = -1;
            MateF = -1; MateC = -1;

            if (!PartidaFinalizada)
            {
                Point rey = BuscarRey(TurnoActual);
                if (rey.X != -1 && IsInCheck(rey))
                {
                    JaqueF = rey.X; JaqueC = rey.Y;
                }
            }
        }

        private bool HayTresRepeticiones()
        {
            if (HistorialPosiciones.Count == 0) return false;

            string actual = HistorialPosiciones[HistorialPosiciones.Count - 1];
            int cuenta = 0;
            foreach (string clave in HistorialPosiciones)
                if (clave == actual) cuenta++;

            return cuenta >= 3;
        }

        private bool MaterialInsuficiente()
        {
            List<PiezaInfo> piezas = new List<PiezaInfo>();
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PiezaInfo info = Casillas[r, c];
                    if (info != null && info.Tipo != "king")
                        piezas.Add(info);
                }
            }

            if (piezas.Count == 0) return true;

            if (piezas.Count == 1
                && (piezas[0].Tipo == "bishop" || piezas[0].Tipo == "knight"))
                return true;

            if (piezas.Count == 2
                && piezas[0].Tipo == "bishop" && piezas[1].Tipo == "bishop"
                && piezas[0].Color != piezas[1].Color)
            {
                int c0 = (piezas[0].Fila + piezas[0].Columna) % 2;
                int c1 = (piezas[1].Fila + piezas[1].Columna) % 2;
                if (c0 == c1) return true;
            }

            return false;
        }

        // ----- Aplicar y deshacer jugadas -----

        public void AplicarMovimiento(Point sourcePos, Point targetPos, string promocion)
        {
            if (PartidaFinalizada) return;

            PiezaInfo sourceInfo = Casillas[sourcePos.X, sourcePos.Y];
            if (sourceInfo == null) return;

            // ¿Es un enroque? (el rey se desplaza dos columnas)
            bool isCastle = sourceInfo.Tipo == "king"
                            && targetPos.X == sourcePos.X
                            && Math.Abs(targetPos.Y - sourcePos.Y) == 2;

            // ¿Es una captura al paso? (peón llega a la casilla objetivo con destino vacío)
            bool isEnPassant = sourceInfo.Tipo == "pawn"
                               && HayAlPaso
                               && targetPos.X == AlPasoF && targetPos.Y == AlPasoC
                               && Casillas[targetPos.X, targetPos.Y] == null;

            Point epVictimPos = new Point(-1, -1);
            PiezaInfo epVictimInfo = null;
            if (isEnPassant)
            {
                int victimRow = targetPos.X + (sourceInfo.Color == "white" ? 1 : -1);
                epVictimPos = new Point(victimRow, targetPos.Y);
                epVictimInfo = Casillas[victimRow, targetPos.Y];
                if (epVictimInfo == null || epVictimInfo.Tipo != "pawn" || epVictimInfo.Color == sourceInfo.Color)
                    isEnPassant = false;
            }

            // Guardar el estado actual para poder deshacer
            PiezaInfo capturedTag = Casillas[targetPos.X, targetPos.Y];
            bool capturo = capturedTag != null || isEnPassant;

            MovHistorial hist = new MovHistorial();
            hist.Origen = sourcePos;
            hist.Destino = targetPos;
            hist.PiezaCapturada = capturedTag;
            hist.PiezaMovida = sourceInfo;
            hist.EsEnroque = isCastle;
            hist.EsAlPaso = isEnPassant;
            hist.VictimaPos = epVictimPos;
            hist.VictimaInfo = epVictimInfo;
            hist.AlPasoAnterior = HayAlPaso ? new Point(AlPasoF, AlPasoC) : (Point?)null;
            hist.HalfmoveAnterior = MedioReloj;
            hist.FullmoveAnterior = NumeroJugada;

            if (isCastle)
            {
                hist.TorreOrigen = new Point(sourcePos.X, targetPos.Y > sourcePos.Y ? 7 : 0);
                hist.TorreDestino = new Point(sourcePos.X, targetPos.Y > sourcePos.Y ? 5 : 3);
                hist.TorreInfo = Casillas[hist.TorreOrigen.X, hist.TorreOrigen.Y];
            }
            HistorialMovimientos.Push(hist);

            // Capturar pieza si existe (o el peón al paso)
            if (capturedTag != null)
            {
                if (capturedTag.Color == "white")
                {
                    NegrasCapturaron++;
                    CapturadasPorNegras.Add(capturedTag.Tipo);
                }
                else
                {
                    BlancasCapturaron++;
                    CapturadasPorBlancas.Add(capturedTag.Tipo);
                }
            }
            else if (isEnPassant && epVictimInfo != null)
            {
                if (epVictimInfo.Color == "white")
                {
                    NegrasCapturaron++;
                    CapturadasPorNegras.Add(epVictimInfo.Tipo);
                }
                else
                {
                    BlancasCapturaron++;
                    CapturadasPorBlancas.Add(epVictimInfo.Tipo);
                }
            }

            // Promoción del peón que alcanza la última fila
            string tipoFinal = sourceInfo.Tipo;
            bool coronacion = false;
            if (sourceInfo.Tipo == "pawn" && (targetPos.X == 0 || targetPos.X == BOARD_SIZE - 1))
            {
                tipoFinal = string.IsNullOrWhiteSpace(promocion) ? "queen" : promocion;
                coronacion = true;
            }

            // Notación de la jugada (para el label y las aperturas)
            string san = ConstruirSan(sourcePos, targetPos, sourceInfo, capturo,
                                      coronacion ? tipoFinal : null, isCastle);

            // Mover la pieza (marcada como movida)
            Casillas[targetPos.X, targetPos.Y] =
                new PiezaInfo(sourceInfo.Color, tipoFinal, targetPos.X, targetPos.Y, true);

            // Limpiar casilla origen
            Casillas[sourcePos.X, sourcePos.Y] = null;

            // Mover también la torre si es enroque
            if (isCastle)
            {
                Casillas[hist.TorreDestino.X, hist.TorreDestino.Y] =
                    new PiezaInfo(sourceInfo.Color, "rook", hist.TorreDestino.X, hist.TorreDestino.Y, true);
                Casillas[hist.TorreOrigen.X, hist.TorreOrigen.Y] = null;
            }

            // Retirar el peón capturado al paso
            if (isEnPassant)
                Casillas[epVictimPos.X, epVictimPos.Y] = null;

            // Actualizar la casilla de captura al paso (solo tras un doble avance de peón)
            if (sourceInfo.Tipo == "pawn" && targetPos.Y == sourcePos.Y
                && Math.Abs(targetPos.X - sourcePos.X) == 2)
            {
                HayAlPaso = true;
                AlPasoF = (sourcePos.X + targetPos.X) / 2;
                AlPasoC = sourcePos.Y;
            }
            else
            {
                HayAlPaso = false;
                AlPasoF = -1;
                AlPasoC = -1;
            }

            // Reloj de 50 movimientos y numeración FEN
            if (sourceInfo.Tipo == "pawn" || capturo)
                MedioReloj = 0;
            else
                MedioReloj++;

            if (sourceInfo.Color == "black")
                NumeroJugada++;

            // Cambiar turno
            TurnoActual = TurnoActual == "white" ? "black" : "white";

            // Registrar la jugada
            NumMovimientos++;
            UltimoMovimiento = Notacion(sourcePos, targetPos);
            Notaciones.Add(san);
            UltimaNotacion = san;

            // Datos para el label de consejos
            UltimoFueCaptura = capturo;
            UltimoFueAlPaso = isEnPassant;
            UltimoFueCoronacion = coronacion;
            UltimoFueEnroqueCorto = isCastle && targetPos.Y == 6;
            UltimoFueEnroqueLargo = isCastle && targetPos.Y == 2;

            // Registrar la posición para las tablas por repetición
            HistorialPosiciones.Add(PosicionClave());

            // Verificar jaque, jaque mate o tablas
            EvaluarFinDePartida();

            // El movimiento reciente se puede deshacer (una sola vez hasta el siguiente movimiento)
            PuedeDeshacer = !PartidaFinalizada;
        }

        public void DeshacerUnMovimiento()
        {
            MovHistorial move = HistorialMovimientos.Pop();

            // Mover la pieza de vuelta a su casilla de origen
            Casillas[move.Origen.X, move.Origen.Y] = move.PiezaMovida;

            // Restaurar lo que había en la casilla de destino (pieza capturada o vacía)
            Casillas[move.Destino.X, move.Destino.Y] = move.PiezaCapturada;

            // Si fue un enroque, regresar también la torre
            if (move.EsEnroque)
            {
                Casillas[move.TorreOrigen.X, move.TorreOrigen.Y] = move.TorreInfo;
                Casillas[move.TorreDestino.X, move.TorreDestino.Y] = null;
            }

            // Si fue una captura al paso, devolver el peón capturado
            if (move.EsAlPaso)
            {
                Casillas[move.VictimaPos.X, move.VictimaPos.Y] = move.VictimaInfo;

                if (move.VictimaInfo != null)
                {
                    if (move.VictimaInfo.Color == "white")
                        NegrasCapturaron--;
                    else
                        BlancasCapturaron--;

                    QuitarCaptura(move.VictimaInfo.Color == "white"
                        ? CapturadasPorNegras : CapturadasPorBlancas);
                }
            }

            // Ajustar contadores de capturas si se deshace una captura normal
            if (move.PiezaCapturada != null)
            {
                if (move.PiezaCapturada.Color == "white")
                    NegrasCapturaron--;
                else
                    BlancasCapturaron--;

                QuitarCaptura(move.PiezaCapturada.Color == "white"
                    ? CapturadasPorNegras : CapturadasPorBlancas);
            }

            // Restaurar el estado de las reglas estándar
            HayAlPaso = move.AlPasoAnterior.HasValue;
            AlPasoF = move.AlPasoAnterior.HasValue ? move.AlPasoAnterior.Value.X : -1;
            AlPasoC = move.AlPasoAnterior.HasValue ? move.AlPasoAnterior.Value.Y : -1;
            MedioReloj = move.HalfmoveAnterior;
            NumeroJugada = move.FullmoveAnterior;

            if (HistorialPosiciones.Count > 0)
                HistorialPosiciones.RemoveAt(HistorialPosiciones.Count - 1);
            if (Notaciones.Count > 0)
                Notaciones.RemoveAt(Notaciones.Count - 1);

            // Cambiar el turno de vuelta
            TurnoActual = move.PiezaMovida.Color;

            if (NumMovimientos > 0) NumMovimientos--;
        }

        private static void QuitarCaptura(List<string> lista)
        {
            if (lista.Count > 0)
                lista.RemoveAt(lista.Count - 1);
        }

        // ----- Posición clave, FEN y enroques -----

        public string PosicionClave()
        {
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PiezaInfo info = Casillas[r, c];
                    sb.Append(info == null ? '.' : LetraFen(info));
                }
            }

            sb.Append(' ').Append(TurnoActual == "white" ? 'w' : 'b');
            sb.Append(' ').Append(CalcularEnroques());

            if (HayAlPaso && PuedeCapturarAlPaso())
                sb.Append(' ').Append(NombreCasilla(new Point(AlPasoF, AlPasoC)));

            return sb.ToString();
        }

        private bool PuedeCapturarAlPaso()
        {
            if (!HayAlPaso) return false;

            int r = AlPasoF;
            int c = AlPasoC;
            int pawnRow = TurnoActual == "white" ? r + 1 : r - 1;

            foreach (int dc in new int[] { -1, 1 })
            {
                int cc = c + dc;
                if (!EnLimites(pawnRow, cc)) continue;

                PiezaInfo pi = Casillas[pawnRow, cc];
                if (pi != null && pi.Tipo == "pawn" && pi.Color == TurnoActual)
                    return true;
            }

            return false;
        }

        /// <summary>Genera la notación FEN de la posición actual.</summary>
        public string GenerarFen()
        {
            StringBuilder sb = new StringBuilder();

            for (int r = 0; r < BOARD_SIZE; r++)
            {
                int vacias = 0;
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PiezaInfo info = Casillas[r, c];
                    if (info == null)
                    {
                        vacias++;
                        continue;
                    }
                    if (vacias > 0)
                    {
                        sb.Append(vacias);
                        vacias = 0;
                    }
                    sb.Append(LetraFen(info));
                }
                if (vacias > 0) sb.Append(vacias);
                if (r < BOARD_SIZE - 1) sb.Append('/');
            }

            string turno = TurnoActual == "white" ? "w" : "b";
            string enroques = CalcularEnroques();
            string ep = HayAlPaso ? NombreCasilla(new Point(AlPasoF, AlPasoC)) : "-";
            return sb.ToString() + " " + turno + " " + enroques + " " + ep
                   + " " + MedioReloj + " " + NumeroJugada;
        }

        private static char LetraFen(PiezaInfo info)
        {
            char c;
            switch (info.Tipo)
            {
                case "king": c = 'k'; break;
                case "queen": c = 'q'; break;
                case "rook": c = 'r'; break;
                case "bishop": c = 'b'; break;
                case "knight": c = 'n'; break;
                default: c = 'p'; break;
            }
            return info.Color == "white" ? char.ToUpper(c) : c;
        }

        private string CalcularEnroques()
        {
            StringBuilder sb = new StringBuilder();
            if (DerechoEnroque(true, true)) sb.Append('K');
            if (DerechoEnroque(true, false)) sb.Append('Q');
            if (DerechoEnroque(false, true)) sb.Append('k');
            if (DerechoEnroque(false, false)) sb.Append('q');
            return sb.Length == 0 ? "-" : sb.ToString();
        }

        private bool DerechoEnroque(bool blancas, bool flancoRey)
        {
            string color = blancas ? "white" : "black";
            int row = blancas ? 7 : 0;

            if (!PiezaSinMover(row, 4, color, "king")) return false;

            int rookCol = flancoRey ? 7 : 0;
            return PiezaSinMover(row, rookCol, color, "rook");
        }

        // ----- Piezas iniciales -----

        public void ColocarPiezasIniciales()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
                for (int j = 0; j < BOARD_SIZE; j++)
                    Casillas[i, j] = null;

            // Piezas negras (fila 0 y 1)
            ColocarPieza(0, 0, "black", "rook");
            ColocarPieza(0, 1, "black", "knight");
            ColocarPieza(0, 2, "black", "bishop");
            ColocarPieza(0, 3, "black", "queen");
            ColocarPieza(0, 4, "black", "king");
            ColocarPieza(0, 5, "black", "bishop");
            ColocarPieza(0, 6, "black", "knight");
            ColocarPieza(0, 7, "black", "rook");
            for (int col = 0; col < 8; col++)
                ColocarPieza(1, col, "black", "pawn");

            // Piezas blancas (fila 6 y 7)
            for (int col = 0; col < 8; col++)
                ColocarPieza(6, col, "white", "pawn");
            ColocarPieza(7, 0, "white", "rook");
            ColocarPieza(7, 1, "white", "knight");
            ColocarPieza(7, 2, "white", "bishop");
            ColocarPieza(7, 3, "white", "queen");
            ColocarPieza(7, 4, "white", "king");
            ColocarPieza(7, 5, "white", "bishop");
            ColocarPieza(7, 6, "white", "knight");
            ColocarPieza(7, 7, "white", "rook");
        }

        private void ColocarPieza(int row, int col, string color, string type)
        {
            Casillas[row, col] = new PiezaInfo(color, type, row, col);
        }

        // ----- Persistencia (tableroJson) -----

        public string Serializar()
        {
            JArray cells = new JArray();
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PiezaInfo info = Casillas[r, c];
                    if (info == null)
                    {
                        cells.Add(JValue.CreateNull());
                    }
                    else
                    {
                        cells.Add(new JObject
                        {
                            ["c"] = info.Color,
                            ["t"] = info.Tipo,
                            ["m"] = info.SeHaMovido
                        });
                    }
                }
            }

            JObject o = new JObject
            {
                ["turn"] = TurnoActual,
                ["wc"] = BlancasCapturaron,
                ["bc"] = NegrasCapturaron,
                ["ia"] = ModoIA,
                ["cia"] = ColorIA,
                ["nivel"] = NivelIA,
                ["ep"] = HayAlPaso ? NombreCasilla(new Point(AlPasoF, AlPasoC)) : null,
                ["hm"] = MedioReloj,
                ["fm"] = NumeroJugada,
                ["rep"] = new JArray(HistorialPosiciones),
                ["not"] = new JArray(Notaciones),
                ["cb"] = new JArray(CapturadasPorBlancas),
                ["cn"] = new JArray(CapturadasPorNegras),
                ["cells"] = cells
            };
            return o.ToString(Formatting.None);
        }

        public void AplicarEstado(string json)
        {
            try
            {
                JObject o = JObject.Parse(json);
                TurnoActual = o["turn"] != null ? (string)o["turn"] : "white";
                BlancasCapturaron = o["wc"] != null ? (int)o["wc"] : 0;
                NegrasCapturaron = o["bc"] != null ? (int)o["bc"] : 0;

                if (o["ia"] != null) ModoIA = (bool)o["ia"];
                if (o["cia"] != null) ColorIA = (string)o["cia"];
                if (o["nivel"] != null) NivelIA = (int)o["nivel"];
                if (o["hm"] != null) MedioReloj = (int)o["hm"];
                if (o["fm"] != null) NumeroJugada = (int)o["fm"];

                HayAlPaso = false;
                AlPasoF = -1;
                AlPasoC = -1;
                if (o["ep"] != null && o["ep"].Type != JTokenType.Null)
                {
                    string ep = (string)o["ep"];
                    if (!string.IsNullOrEmpty(ep) && ep.Length >= 2)
                    {
                        int col = ep[0] - 'a';
                        int row = 8 - (ep[1] - '0');
                        if (EnLimites(row, col))
                        {
                            HayAlPaso = true;
                            AlPasoF = row;
                            AlPasoC = col;
                        }
                    }
                }

                JArray cells = (JArray)o["cells"];
                for (int r = 0; r < BOARD_SIZE; r++)
                {
                    for (int c = 0; c < BOARD_SIZE; c++)
                    {
                        JToken cell = cells[r * BOARD_SIZE + c];
                        if (cell == null || cell.Type == JTokenType.Null)
                        {
                            Casillas[r, c] = null;
                        }
                        else
                        {
                            string color = (string)cell["c"];
                            string type = (string)cell["t"];
                            bool moved = cell["m"] != null && (bool)cell["m"];
                            Casillas[r, c] = new PiezaInfo(color, type, r, c, moved);
                        }
                    }
                }

                HistorialPosiciones.Clear();
                JArray rep = o["rep"] as JArray;
                if (rep != null)
                {
                    foreach (JToken t in rep)
                        HistorialPosiciones.Add((string)t);
                }
                if (HistorialPosiciones.Count == 0)
                    HistorialPosiciones.Add(PosicionClave());

                Notaciones.Clear();
                JArray not = o["not"] as JArray;
                if (not != null)
                {
                    foreach (JToken t in not)
                        Notaciones.Add((string)t);
                }

                CapturadasPorBlancas.Clear();
                JArray cb = o["cb"] as JArray;
                if (cb != null)
                {
                    foreach (JToken t in cb)
                        CapturadasPorBlancas.Add((string)t);
                }

                CapturadasPorNegras.Clear();
                JArray cn = o["cn"] as JArray;
                if (cn != null)
                {
                    foreach (JToken t in cn)
                        CapturadasPorNegras.Add((string)t);
                }

                PartidaFinalizada = false;
                MotivoFin = null;
                JaqueF = -1; JaqueC = -1;
                MateF = -1; MateC = -1;
                HistorialMovimientos.Clear();
                PuedeDeshacer = false;
            }
            catch { }
        }
    }
}
