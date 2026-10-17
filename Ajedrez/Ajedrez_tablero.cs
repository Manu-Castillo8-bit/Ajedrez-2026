using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ajedrez
{
    public partial class Ajedrez_tablero : Form
    {
     
        private const int BOARD_SIZE = 8;
        private const int CELL_SIZE = 60;
        private PictureBox[,] board;
        private PictureBox selectedPiece = null;
        private Point selectedPosition = new Point(-1, -1);
        private string currentTurn = "white"; // "white" o "black"
        private int whiteCaptures = 0;
        private int blackCaptures = 0;
        private List<Point> validMoves = new List<Point>();
        private Stack<MoveHistory> moveHistory = new Stack<MoveHistory>();
        private bool canUndo = false;

        // ----- Reglas estándar del ajedrez -----
        private Point? casillaAlPaso = null;          // objetivo de captura al paso (o null)
        private int halfmoveClock = 0;                // medios movimientos sin peón ni captura
        private int fullmoveNumber = 1;               // número de jugada (FEN)
        private List<string> historialPosiciones = new List<string>(); // claves para 3 repeticiones
        private List<string> notaciones = new List<string>();          // jugadas en notación SAN
        private bool partidaFinalizada = false;       // congela el tablero tras mate o tablas
        private string motivoFin = null;              // texto del resultado para el label
        private string motivoGanadorFin = "empate";   // ganador usado al registrar el final
        private Random rndFrases = new Random();

        // Datos de la última jugada para el label de consejos
        private bool ultimoFueCaptura = false;
        private bool ultimoFueAlPaso = false;
        private bool ultimoFueCoronacion = false;
        private bool ultimoFueEnroqueCorto = false;
        private bool ultimoFueEnroqueLargo = false;

        // ----- Resaltado del tablero -----
        private Point? casillaJaque = null;           // rey en jaque -> amarillo
        private Point? casillaMate = null;            // rey en jaque mate -> rojo
        private Point? casillaHover = null;           // casilla bajo el ratón

        // Permite ejecutar partidas de prueba sin ventanas emergentes
        private static bool silenciarDialogos = false;

        // ----- Persistencia en Supabase -----
        private const string TIPO_JUEGO = "ajedrez";
        private string nombreBlancas = "Blancas";
        private string nombreNegras = "Negras";
        private int idPartida = -1;
        private string estadoPendiente = null;
        private int numMovimientos = 0;
        private string ultimoMovimiento = "";
        private bool juegoTerminado = false;

        // ----- Inteligencia artificial (Stockfish) -----
        private bool modoIA = false;
        private string colorIA = "black";
        private int nivelIA = 5;
        private StockfishEngine motor = null;
        private bool iaPensando = false;

        public Ajedrez_tablero()
        {
        
            InitializeComponent();
            InitializeBoard();
            SetupPieces();
            historialPosiciones.Add(PosicionClave());
            Tema.ApplyTheme(this);
        }

        public Ajedrez_tablero(string jugadorBlancas, string jugadorNegras) : this()
        {
            nombreBlancas = string.IsNullOrWhiteSpace(jugadorBlancas) ? "Blancas" : jugadorBlancas;
            nombreNegras = string.IsNullOrWhiteSpace(jugadorNegras) ? "Negras" : jugadorNegras;
        }

        /// <summary>Prepara la partida para continuar desde un estado guardado.</summary>
        public void ContinuarPartida(int id, string estadoJson)
        {
            idPartida = id;
            estadoPendiente = estadoJson;
        }

        /// <summary>Activa el modo contra la IA con el color y la dificultad indicados.</summary>
        public void ConfigurarIA(bool activo, string colorDeLaIA, int nivel)
        {
            modoIA = activo;
            if (!string.IsNullOrWhiteSpace(colorDeLaIA))
                colorIA = colorDeLaIA;
            nivelIA = nivel;
        }

        private void Ajedrez_tablero_Load(object sender, EventArgs e)
        {
            // Mostrar junto a los puntos el nombre del jugador que lleva cada color
            puntosb.Text = "PUNTOS BLANCAS: " + nombreBlancas;
            puntosn.Text = "PUNTOS NEGRAS: " + nombreNegras;

            // El tablero ya se inicializa en el constructor; reconstruir si venimos de una partida guardada.
            if (!string.IsNullOrWhiteSpace(estadoPendiente))
            {
                AplicarEstado(estadoPendiente);
                estadoPendiente = null;
            }

            ActualizarResaltadoJaque();
            MostrarConsejo(null);

            // Si la IA lleva las blancas, debe abrir la partida.
            if (modoIA && !juegoTerminado && currentTurn == colorIA)
                IniciarJugadaIA();
        }

        private void InitializeBoard()
        {
            board = new PictureBox[BOARD_SIZE, BOARD_SIZE];
            int startX = 2, startY = 2;

            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    board[row, col] = new PictureBox();
                    board[row, col].Size = new Size(CELL_SIZE, CELL_SIZE);
                    board[row, col].Location = new Point(startX + col * CELL_SIZE, startY + row * CELL_SIZE);
                    board[row, col].BackColor = (row + col) % 2 == 0 ? Color.White : Color.Black;
                    board[row, col].SizeMode = PictureBoxSizeMode.Zoom;
                    board[row, col].Tag = new Point(row, col);

                    // Eventos
                    board[row, col].Click += Board_Click;
                    board[row, col].MouseEnter += Board_MouseEnter;
                    board[row, col].MouseLeave += Board_MouseLeave;

                    G.Controls.Add(board[row, col]);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (board == null) return;

            using (Font f = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold))
            using (SolidBrush br = new SolidBrush(Tema.IsDarkMode ? Color.White : Color.Black))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    RectangleF rect = new RectangleF(12 + col * CELL_SIZE, 494, CELL_SIZE, 16);
                    e.Graphics.DrawString(((char)('a' + col)).ToString(), f, br, rect, sf);
                }

                for (int row = 0; row < BOARD_SIZE; row++)
                {
                    RectangleF rect = new RectangleF(0, 12 + row * CELL_SIZE, 11, CELL_SIZE);
                    e.Graphics.DrawString((BOARD_SIZE - row).ToString(), f, br, rect, sf);
                }
            }
        }

        private void SetupPieces()
        {
            // Piezas negras (fila 0 y 1)
            // Fila 0 - Piezas mayores negras
            PlacePiece(0, 0, "Torre_n", "black", "rook");
            PlacePiece(0, 1, "caballo_n", "black", "knight");
            PlacePiece(0, 2, "alfil_n", "black", "bishop");
            PlacePiece(0, 3, "reina_n", "black", "queen");
            PlacePiece(0, 4, "rey_n", "black", "king");
            PlacePiece(0, 5, "alfil_n", "black", "bishop");
            PlacePiece(0, 6, "caballo_n", "black", "knight");
            PlacePiece(0, 7, "Torre_n", "black", "rook");

            // Fila 1 - Peones negros
            for (int col = 0; col < 8; col++)
                PlacePiece(1, col, "peon_negro_pixel", "black", "pawn");

            // Piezas blancas (fila 6 y 7)
            // Fila 6 - Peones blancos
            for (int col = 0; col < 8; col++)
                PlacePiece(6, col, "peon", "white", "pawn");

            // Fila 7 - Piezas mayores blancas
            PlacePiece(7, 0, "torre", "white", "rook");
            PlacePiece(7, 1, "caballo", "white", "knight");
            PlacePiece(7, 2, "alfil_blanco", "white", "bishop");
            PlacePiece(7, 3, "reina", "white", "queen");
            PlacePiece(7, 4, "rey", "white", "king");
            PlacePiece(7, 5, "alfil_blanco", "white", "bishop");
            PlacePiece(7, 6, "caballo", "white", "knight");
            PlacePiece(7, 7, "torre", "white", "rook");
        }

        private void PlacePiece(int row, int col, string imageName, string color, string type)
        {
            var piece = board[row, col];
            piece.Image = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
            piece.Tag = new PieceInfo(color, type, row, col);
        }

        private void Board_Click(object sender, EventArgs e)
        {
            PictureBox clickedCell = sender as PictureBox;
            if (clickedCell == null) return;

            // Tras el jaque mate o unas tablas el tablero queda congelado
            if (partidaFinalizada) return;

            // Durante el turno de la IA (o mientras esta pensando) no se permiten jugadas humanas
            if (modoIA && (iaPensando || currentTurn == colorIA)) return;

            Point pos = GetCellPosition(clickedCell);
            if (pos.X == -1 && pos.Y == -1) return;

            // Si hay una pieza seleccionada y se hace clic en una casilla válida
            if (selectedPiece != null && validMoves.Contains(pos))
            {
                string promocion = null;
                PieceInfo moving = GetPieceInfoFromCell(board[selectedPosition.X, selectedPosition.Y]);
                if (moving != null && moving.Type == "pawn"
                    && (pos.X == 0 || pos.X == BOARD_SIZE - 1))
                {
                    promocion = PedirPromocion();
                }

                // Mover la pieza
                AplicarMovimiento(selectedPosition, pos, promocion);
                return;
            }

            // Seleccionar una pieza
            if (clickedCell.Image != null)
            {
                PieceInfo info = GetPieceInfoFromCell(clickedCell);
                if (info != null && info.Color == currentTurn)
                {
                    SelectPiece(clickedCell);
                }
            }
        }
        

        private void SelectPiece(PictureBox piece)
        {
            selectedPiece = piece;
            selectedPosition = GetCellPosition(piece);

            // Calcular movimientos válidos y repintar todo el tablero de una vez
            validMoves = GetValidMoves(selectedPosition);
            RefrescarColores();
        }

        private Point GetCellPosition(PictureBox cell)
        {
            if (cell == null || cell.Tag == null)
                return new Point(-1, -1);

            if (cell.Tag is Point)
                return (Point)cell.Tag;
            else if (cell.Tag is PieceInfo)
            {
                PieceInfo info = (PieceInfo)cell.Tag;
                return new Point(info.Row, info.Col);
            }
            return new Point(-1, -1);
        }

        private PieceInfo GetPieceInfoFromCell(PictureBox cell)
        {
            if (cell == null || cell.Tag == null)
                return null;

            if (cell.Tag is PieceInfo)
                return (PieceInfo)cell.Tag;

            return null;
        }

        private void ClearSelection()
        {
            selectedPiece = null;
            selectedPosition = new Point(-1, -1);
            validMoves.Clear();
            RefrescarColores();
        }

        private Color ColorBase(int row, int col)
        {
            return (row + col) % 2 == 0 ? Color.White : Color.Black;
        }

        /// <summary>
        /// Repinta el tablero combinando selección, movimientos válidos, jaque (amarillo)
        /// y jaque mate (rojo). Centralizar los colores evita que el hover, la selección
        /// o el cambio de tema borren las marcas de peligro.
        /// </summary>
        private void RefrescarColores()
        {
            if (board == null) return;

            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    Point p = new Point(r, c);
                    Color color = ColorBase(r, c);

                    // Destinos legales de la pieza seleccionada
                    if (validMoves.Contains(p))
                        color = Color.LightGreen;

                    // Pieza seleccionada
                    if (selectedPiece != null && selectedPosition == p)
                        color = Color.Yellow;

                    // Rey en jaque: amarillo
                    if (casillaJaque.HasValue && casillaJaque.Value == p)
                        color = Color.Yellow;

                    // Hover sutil, solo sobre casillas base con pieza
                    if (casillaHover.HasValue && casillaHover.Value == p
                        && color == ColorBase(r, c) && board[r, c].Image != null)
                    {
                        color = (r + c) % 2 == 0
                            ? Color.FromArgb(220, 220, 220)
                            : Color.FromArgb(80, 80, 80);
                    }

                    // Jaque mate: el rojo tiene prioridad absoluta
                    if (casillaMate.HasValue && casillaMate.Value == p)
                        color = Color.Red;

                    board[r, c].BackColor = color;
                }
            }
        }

        private void AplicarMovimiento(Point sourcePos, Point targetPos, string promocion)
        {
            if (partidaFinalizada) return;

            PictureBox origen = board[sourcePos.X, sourcePos.Y];
            PictureBox destino = board[targetPos.X, targetPos.Y];
            PieceInfo sourceInfo = GetPieceInfoFromCell(origen);

            if (sourceInfo == null) return;

            juegoTerminado = false;

            // ¿Es un enroque? (el rey se desplaza dos columnas)
            bool isCastle = sourceInfo.Type == "king"
                            && targetPos.X == sourcePos.X
                            && Math.Abs(targetPos.Y - sourcePos.Y) == 2;

            // ¿Es una captura al paso? (peón llega a la casilla objetivo con destino vacío)
            bool isEnPassant = sourceInfo.Type == "pawn"
                               && casillaAlPaso.HasValue
                               && targetPos == casillaAlPaso.Value
                               && destino.Image == null;

            Point epVictimPos = new Point(-1, -1);
            PieceInfo epVictimInfo = null;
            Image epVictimImage = null;
            object epVictimTag = null;
            if (isEnPassant)
            {
                int victimRow = targetPos.X + (sourceInfo.Color == "white" ? 1 : -1);
                epVictimPos = new Point(victimRow, targetPos.Y);
                epVictimImage = board[victimRow, targetPos.Y].Image;
                epVictimTag = board[victimRow, targetPos.Y].Tag;
                epVictimInfo = GetPieceInfoFromCell(board[victimRow, targetPos.Y]);
                if (epVictimInfo == null || epVictimInfo.Type != "pawn" || epVictimInfo.Color == sourceInfo.Color)
                    isEnPassant = false;
            }

            // Guardar el estado actual para poder deshacer
            Image capturedImage = null;
            object capturedTag = null;
            if (destino.Image != null)
            {
                capturedImage = destino.Image;
                capturedTag = destino.Tag;
            }

            bool capturo = destino.Image != null || isEnPassant;

            MoveHistory hist = new MoveHistory(sourcePos, targetPos, capturedImage, capturedTag, sourceInfo);
            hist.MovedImage = origen.Image;
            hist.IsCastle = isCastle;
            hist.IsEnPassant = isEnPassant;
            hist.EnPassantVictimPos = epVictimPos;
            hist.EnPassantVictimImage = epVictimImage;
            hist.EnPassantVictimTag = epVictimTag;
            hist.AlPasoAnterior = casillaAlPaso;
            hist.HalfmoveAnterior = halfmoveClock;
            hist.FullmoveAnterior = fullmoveNumber;

            if (isCastle)
            {
                hist.RookFrom = new Point(sourcePos.X, targetPos.Y > sourcePos.Y ? 7 : 0);
                hist.RookTo = new Point(sourcePos.X, targetPos.Y > sourcePos.Y ? 5 : 3);
                hist.RookInfo = GetPieceInfoFromCell(board[hist.RookFrom.X, hist.RookFrom.Y]);
            }
            moveHistory.Push(hist);

            // Capturar pieza si existe (o el peón al paso)
            if (destino.Image != null)
            {
                PieceInfo targetInfo = GetPieceInfoFromCell(destino);
                if (targetInfo != null)
                {
                    if (targetInfo.Color == "white")
                        blackCaptures++;
                    else
                        whiteCaptures++;
                }

                UpdateCaptureCounters();
            }
            else if (isEnPassant && epVictimInfo != null)
            {
                if (epVictimInfo.Color == "white")
                    blackCaptures++;
                else
                    whiteCaptures++;

                UpdateCaptureCounters();
            }

            // Promoción del peón que alcanza la última fila
            string tipoFinal = sourceInfo.Type;
            bool coronacion = false;
            if (sourceInfo.Type == "pawn" && (targetPos.X == 0 || targetPos.X == BOARD_SIZE - 1))
            {
                tipoFinal = string.IsNullOrWhiteSpace(promocion) ? "queen" : promocion;
                coronacion = true;
            }

            // Notación de la jugada (para el label y las aperturas)
            string san = ConstruirSan(sourcePos, targetPos, sourceInfo, capturo,
                                      coronacion ? tipoFinal : null, isCastle);

            // Mover la pieza (marcada como movida)
            destino.Image = tipoFinal == sourceInfo.Type
                ? origen.Image
                : ImagenDe(sourceInfo.Color, tipoFinal);
            destino.Tag = new PieceInfo(sourceInfo.Color, tipoFinal, targetPos.X, targetPos.Y, true);

            // Limpiar casilla origen
            origen.Image = null;
            origen.Tag = new Point(sourcePos.X, sourcePos.Y);

            // Mover también la torre si es enroque
            if (isCastle)
            {
                PictureBox rookFrom = board[hist.RookFrom.X, hist.RookFrom.Y];
                PictureBox rookTo = board[hist.RookTo.X, hist.RookTo.Y];
                rookTo.Image = rookFrom.Image;
                rookTo.Tag = new PieceInfo(sourceInfo.Color, "rook", hist.RookTo.X, hist.RookTo.Y, true);
                rookFrom.Image = null;
                rookFrom.Tag = new Point(hist.RookFrom.X, hist.RookFrom.Y);
            }

            // Retirar el peón capturado al paso
            if (isEnPassant)
            {
                board[epVictimPos.X, epVictimPos.Y].Image = null;
                board[epVictimPos.X, epVictimPos.Y].Tag = new Point(epVictimPos.X, epVictimPos.Y);
            }

            // Actualizar la casilla de captura al paso (solo tras un doble avance de peón)
            if (sourceInfo.Type == "pawn" && targetPos.Y == sourcePos.Y
                && Math.Abs(targetPos.X - sourcePos.X) == 2)
                casillaAlPaso = new Point((sourcePos.X + targetPos.X) / 2, sourcePos.Y);
            else
                casillaAlPaso = null;

            // Reloj de 50 movimientos y numeración FEN
            if (sourceInfo.Type == "pawn" || capturo)
                halfmoveClock = 0;
            else
                halfmoveClock++;

            if (sourceInfo.Color == "black")
                fullmoveNumber++;

            // Cambiar turno
            currentTurn = currentTurn == "white" ? "black" : "white";

            // Registrar la jugada
            numMovimientos++;
            ultimoMovimiento = Notacion(sourcePos, targetPos);
            notaciones.Add(san);

            // Datos para el label de consejos
            ultimoFueCaptura = capturo;
            ultimoFueAlPaso = isEnPassant;
            ultimoFueCoronacion = coronacion;
            ultimoFueEnroqueCorto = isCastle && targetPos.Y == 6;
            ultimoFueEnroqueLargo = isCastle && targetPos.Y == 2;

            // Limpiar selección
            ClearSelection();

            // Registrar la posición para las tablas por repetición
            historialPosiciones.Add(PosicionClave());

            // Verificar jaque, jaque mate o tablas
            bool termino = EvaluarFinDePartida();
            if (termino)
            {
                FinalizarPartida();
            }
            else
            {
                MostrarConsejo(san);
                if (casillaJaque.HasValue)
                    Avisar("¡Jaque al rey de las " + ColorTexto(currentTurn) + "!",
                           "Ajedrez", MessageBoxIcon.Warning);
            }

            // Guardar el estado para poder continuar la partida (si no terminó)
            if (!juegoTerminado) GuardarPartida();

            // El movimiento reciente se puede deshacer (una sola vez hasta el siguiente movimiento)
            canUndo = !partidaFinalizada;

            // Si ahora le toca a la IA, que responda
            if (modoIA && !juegoTerminado && currentTurn == colorIA)
                IniciarJugadaIA();
        }

        private static string ColorTexto(string color)
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

        private string ConstruirSan(Point from, Point to, PieceInfo pieza, bool captura,
                                    string promocion, bool enroque)
        {
            if (enroque)
                return to.Y == 6 ? "O-O" : "O-O-O";

            if (pieza.Type == "pawn")
            {
                string s = captura
                    ? ((char)('a' + from.Y)).ToString() + "x" + NombreCasilla(to)
                    : NombreCasilla(to);
                if (!string.IsNullOrEmpty(promocion))
                    s += "=" + LetraSan(promocion);
                return s;
            }

            return LetraSan(pieza.Type) + (captura ? "x" : "") + NombreCasilla(to);
        }

        private void Avisar(string texto, string titulo, MessageBoxIcon icono)
        {
            if (silenciarDialogos) return;
            MessageBox.Show(texto, titulo, MessageBoxButtons.OK, icono);
        }

        private static Image ImagenDe(string color, string type)
        {
            return (Image)Properties.Resources.ResourceManager.GetObject(NombreImagen(color, type));
        }

        /// <summary>Pregunta al jugador por la pieza a la que corona el peón.</summary>
        private string PedirPromocion()
        {
            if (silenciarDialogos) return "queen";

            using (Form dlg = new Form())
            {
                dlg.Text = "Promoción";
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ClientSize = new Size(320, 70);

                string[] tipos = { "queen", "rook", "bishop", "knight" };
                string[] textos = { "Reina", "Torre", "Alfil", "Caballo" };
                string elegido = "queen";

                for (int i = 0; i < tipos.Length; i++)
                {
                    Button b = new Button();
                    b.Text = textos[i];
                    b.Tag = tipos[i];
                    b.Size = new Size(74, 34);
                    b.Location = new Point(10 + i * 77, 18);
                    b.Click += delegate (object s, EventArgs e)
                    {
                        elegido = (string)((Button)s).Tag;
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    };
                    dlg.Controls.Add(b);
                }

                dlg.ShowDialog(this);
                return elegido;
            }
        }

        private string Notacion(Point from, Point to)
        {
            return NombreCasilla(from) + "-" + NombreCasilla(to);
        }

        private static string NombreCasilla(Point p)
        {
            return ((char)('a' + p.Y)).ToString() + (8 - p.X);
        }

        private List<Point> GetValidMoves(Point pos)
        {
            return GetValidMoves(pos, true);
        }

        private List<Point> GetValidMoves(Point pos, bool includeCastling)
        {
            List<Point> moves = new List<Point>();
            PieceInfo info = (PieceInfo)board[pos.X, pos.Y].Tag;

            switch (info.Type)
            {
                case "pawn":
                    moves.AddRange(GetPawnMoves(pos, info.Color));
                    break;
                case "rook":
                    moves.AddRange(GetRookMoves(pos, info.Color));
                    break;
                case "knight":
                    moves.AddRange(GetKnightMoves(pos, info.Color));
                    break;
                case "bishop":
                    moves.AddRange(GetBishopMoves(pos, info.Color));
                    break;
                case "queen":
                    moves.AddRange(GetQueenMoves(pos, info.Color));
                    break;
                case "king":
                    moves.AddRange(GetKingMoves(pos, info.Color));
                    if (includeCastling)
                        AddCastlingMoves(moves, pos, info.Color);
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

        private List<Point> GetPawnMoves(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int direction = color == "white" ? -1 : 1;
            int startRow = color == "white" ? 6 : 1;

            // Movimiento hacia adelante
            int newRow = pos.X + direction;
            if (IsInBounds(newRow, pos.Y) && board[newRow, pos.Y].Image == null)
            {
                moves.Add(new Point(newRow, pos.Y));

                // Movimiento doble desde la posición inicial
                if (pos.X == startRow)
                {
                    int doubleRow = pos.X + 2 * direction;
                    if (board[doubleRow, pos.Y].Image == null)
                        moves.Add(new Point(doubleRow, pos.Y));
                }
            }

            // Captura diagonal
            int[] captureCols = { pos.Y - 1, pos.Y + 1 };
            foreach (int col in captureCols)
            {
                if (IsInBounds(newRow, col) && board[newRow, col].Image != null)
                {
                    PieceInfo targetInfo = (PieceInfo)board[newRow, col].Tag;
                    if (targetInfo.Color != color)
                        moves.Add(new Point(newRow, col));
                }
            }

            // Captura al paso: solo si el peón rival acaba de avanzar dos casillas
            if (casillaAlPaso.HasValue
                && casillaAlPaso.Value.X == newRow
                && Math.Abs(casillaAlPaso.Value.Y - pos.Y) == 1
                && IsInBounds(newRow, casillaAlPaso.Value.Y)
                && board[casillaAlPaso.Value.X, casillaAlPaso.Value.Y].Image == null)
            {
                moves.Add(casillaAlPaso.Value);
            }

            return moves;
        }

        private List<Point> GetRookMoves(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] directions = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int d = 0; d < 4; d++)
            {
                int row = pos.X + directions[d, 0];
                int col = pos.Y + directions[d, 1];

                while (IsInBounds(row, col))
                {
                    if (board[row, col].Image == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PieceInfo targetInfo = (PieceInfo)board[row, col].Tag;
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

        private List<Point> GetKnightMoves(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] offsets = { { -2, -1 }, { -2, 1 }, { -1, -2 }, { -1, 2 },
                               { 1, -2 }, { 1, 2 }, { 2, -1 }, { 2, 1 } };

            for (int i = 0; i < 8; i++)
            {
                int row = pos.X + offsets[i, 0];
                int col = pos.Y + offsets[i, 1];

                if (IsInBounds(row, col))
                {
                    if (board[row, col].Image == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PieceInfo targetInfo = (PieceInfo)board[row, col].Tag;
                        if (targetInfo.Color != color)
                            moves.Add(new Point(row, col));
                    }
                }
            }

            return moves;
        }

        private List<Point> GetBishopMoves(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] directions = { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

            for (int d = 0; d < 4; d++)
            {
                int row = pos.X + directions[d, 0];
                int col = pos.Y + directions[d, 1];

                while (IsInBounds(row, col))
                {
                    if (board[row, col].Image == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PieceInfo targetInfo = (PieceInfo)board[row, col].Tag;
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

        private List<Point> GetQueenMoves(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            moves.AddRange(GetRookMoves(pos, color));
            moves.AddRange(GetBishopMoves(pos, color));
            return moves;
        }

        private List<Point> GetKingMoves(Point pos, string color)
        {
            List<Point> moves = new List<Point>();
            int[,] offsets = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 },
                               { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            for (int i = 0; i < 8; i++)
            {
                int row = pos.X + offsets[i, 0];
                int col = pos.Y + offsets[i, 1];

                if (IsInBounds(row, col))
                {
                    if (board[row, col].Image == null)
                    {
                        moves.Add(new Point(row, col));
                    }
                    else
                    {
                        PieceInfo targetInfo = (PieceInfo)board[row, col].Tag;
                        if (targetInfo.Color != color)
                            moves.Add(new Point(row, col));
                    }
                }
            }

            return moves;
        }

        // ----- Enroques -----
        private void AddCastlingMoves(List<Point> moves, Point pos, string color)
        {
            int homeRow = color == "white" ? 7 : 0;

            // El rey debe estar en su casilla inicial y no haberse movido
            if (pos.X != homeRow || pos.Y != 4) return;

            if (CanCastleKingSide(color))
                moves.Add(new Point(homeRow, 6));

            if (CanCastleQueenSide(color))
                moves.Add(new Point(homeRow, 2));
        }

        private bool CanCastleKingSide(string color)
        {
            int row = color == "white" ? 7 : 0;

            // Rey y torre presentes, del color correcto y sin haberse movido
            if (!IsUnmovedPiece(row, 4, color, "king")) return false;
            if (!IsUnmovedPiece(row, 7, color, "rook")) return false;

            // Casillas intermedias vacías (columnas 5 y 6)
            if (board[row, 5].Image != null || board[row, 6].Image != null) return false;

            // El rey no puede estar en jaque ni pasar por casillas amenazadas
            if (IsInCheck(new Point(row, 4))) return false;
            if (IsInCheck(new Point(row, 5))) return false;
            if (IsInCheck(new Point(row, 6))) return false;

            return true;
        }

        private bool CanCastleQueenSide(string color)
        {
            int row = color == "white" ? 7 : 0;

            // Rey y torre presentes, del color correcto y sin haberse movido
            if (!IsUnmovedPiece(row, 4, color, "king")) return false;
            if (!IsUnmovedPiece(row, 0, color, "rook")) return false;

            // Casillas intermedias vacías (columnas 1, 2 y 3)
            if (board[row, 1].Image != null || board[row, 2].Image != null || board[row, 3].Image != null) return false;

            // El rey no puede estar en jaque ni pasar por casillas amenazadas
            if (IsInCheck(new Point(row, 4))) return false;
            if (IsInCheck(new Point(row, 3))) return false;
            if (IsInCheck(new Point(row, 2))) return false;

            return true;
        }

        private bool IsUnmovedPiece(int row, int col, string color, string type)
        {
            PieceInfo info = GetPieceInfoFromCell(board[row, col]);
            return info != null && info.Color == color && info.Type == type && !info.HasMoved;
        }

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < BOARD_SIZE && col >= 0 && col < BOARD_SIZE;
        }

        private void Board_MouseEnter(object sender, EventArgs e)

        {
            PictureBox cell = sender as PictureBox;
            if (cell == null) return;

            casillaHover = GetCellPosition(cell);
            RefrescarColores();
        }
        

        private void Board_MouseLeave(object sender, EventArgs e)
        {
            PictureBox cell = sender as PictureBox;
            if (cell == null) return;

            casillaHover = null;
            RefrescarColores();
        }
        

        private void UpdateCaptureCounters()
        {
            p_blancas.Text = whiteCaptures.ToString();
            p_negras.Text = blackCaptures.ToString();
        }

        // Al finalizar la partida, gana quien haya comido más piezas (empate si igualan)
        private string GanadorPorPiezasComidas()
        {
            if (whiteCaptures > blackCaptures) return nombreBlancas;
            if (blackCaptures > whiteCaptures) return nombreNegras;
            return "empate";
        }

        /// <summary>
        /// Analiza la posición del bando al que le toca mover y decide si la partida
        /// termina (jaque mate o tablas). No muestra ventanas ni reinicia el tablero.
        /// </summary>
        private bool EvaluarFinDePartida()
        {
            casillaJaque = null;
            casillaMate = null;
            motivoFin = null;
            motivoGanadorFin = "empate";

            Point rey = BuscarRey(currentTurn);
            bool enJaque = rey.X != -1 && IsInCheck(rey);

            if (enJaque && !HayMovimientosLegales(currentTurn))
            {
                casillaMate = rey;
                string ganadorColor = currentTurn == "white" ? "black" : "white";
                string ganador = ganadorColor == "white" ? nombreBlancas : nombreNegras;
                motivoGanadorFin = ganador;
                motivoFin = "¡Jaque mate! Ganan las " + ColorTexto(ganadorColor)
                            + " (" + ganador + ").\nEl rey queda marcado en rojo.";
                return true;
            }

            if (enJaque)
            {
                casillaJaque = rey;
                return false;
            }

            if (!HayMovimientosLegales(currentTurn))
            {
                motivoFin = "¡Tablas! Empate por ahogado: no hay movimientos legales.";
                return true;
            }

            if (halfmoveClock >= 100)
            {
                motivoFin = "¡Tablas! Empate por la regla de los 50 movimientos.";
                return true;
            }

            if (HayTresRepeticiones())
            {
                motivoFin = "¡Tablas! Empate por repetición de posición (3 veces).";
                return true;
            }

            if (MaterialInsuficiente())
            {
                motivoFin = "¡Tablas! Empate por material insuficiente.";
                return true;
            }

            return false;
        }

        private void FinalizarPartida()
        {
            partidaFinalizada = true;
            RegistrarFinAjedrez(motivoGanadorFin);
            RefrescarColores();
            MostrarConsejo(null);

            if (silenciarDialogos) return;

            DialogResult r = MessageBox.Show(
                motivoFin + "\n\n¿Quieres empezar una nueva partida?",
                "Fin del juego", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.Yes) ResetGame();
        }

        private void ActualizarResaltadoJaque()
        {
            casillaJaque = null;
            casillaMate = null;

            if (!partidaFinalizada)
            {
                Point rey = BuscarRey(currentTurn);
                if (rey.X != -1 && IsInCheck(rey))
                    casillaJaque = rey;
            }

            RefrescarColores();
        }

        private void MostrarConsejo(string san)
        {
            if (consejo == null) return;

            if (partidaFinalizada && !string.IsNullOrEmpty(motivoFin))
            {
                consejo.Text = motivoFin;
                return;
            }

            var partes = new List<string>();

            if (san != null)
            {
                partes.Add("Jugada: " + san);

                string contexto = Consejos.Contexto(san, ultimoFueCaptura, ultimoFueEnroqueCorto,
                                   ultimoFueEnroqueLargo, ultimoFueAlPaso, ultimoFueCoronacion);

                if (casillaJaque.HasValue)
                    contexto = Consejos.MensajeJaque;

                if (contexto != null)
                    partes.Add(contexto);
                else
                    partes.Add(rndFrases.Next(2) == 0
                        ? Consejos.FraseMotivacional(rndFrases)
                        : Consejos.ConsejoEstrategico(rndFrases));
            }
            else if (casillaJaque.HasValue)
            {
                partes.Add(Consejos.MensajeJaque);
            }
            else
            {
                partes.Add(rndFrases.Next(2) == 0
                    ? Consejos.FraseMotivacional(rndFrases)
                    : Consejos.ConsejoEstrategico(rndFrases));
            }

            string apertura = Consejos.ReconocerApertura(notaciones);
            if (apertura != null) partes.Insert(0, "Apertura: " + apertura);

            consejo.Text = string.Join("\n", partes);
        }

        private bool HayTresRepeticiones()
        {
            if (historialPosiciones.Count == 0) return false;

            string actual = historialPosiciones[historialPosiciones.Count - 1];
            int cuenta = 0;
            foreach (string clave in historialPosiciones)
                if (clave == actual) cuenta++;

            return cuenta >= 3;
        }

        private bool MaterialInsuficiente()
        {
            List<PieceInfo> piezas = new List<PieceInfo>();
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PieceInfo info = GetPieceInfoFromCell(board[r, c]);
                    if (info != null && info.Type != "king")
                        piezas.Add(info);
                }
            }

            if (piezas.Count == 0) return true;

            if (piezas.Count == 1
                && (piezas[0].Type == "bishop" || piezas[0].Type == "knight"))
                return true;

            if (piezas.Count == 2
                && piezas[0].Type == "bishop" && piezas[1].Type == "bishop"
                && piezas[0].Color != piezas[1].Color)
            {
                int c0 = (piezas[0].Row + piezas[0].Col) % 2;
                int c1 = (piezas[1].Row + piezas[1].Col) % 2;
                if (c0 == c1) return true;
            }

            return false;
        }

        private string PosicionClave()
        {
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PieceInfo info = GetPieceInfoFromCell(board[r, c]);
                    sb.Append(info == null ? '.' : LetraFen(info));
                }
            }

            sb.Append(' ').Append(currentTurn == "white" ? 'w' : 'b');
            sb.Append(' ').Append(CalcularEnroques());

            if (casillaAlPaso.HasValue && PuedeCapturarAlPaso())
                sb.Append(' ').Append(NombreCasilla(casillaAlPaso.Value));

            return sb.ToString();
        }

        private bool PuedeCapturarAlPaso()
        {
            if (!casillaAlPaso.HasValue) return false;

            int r = casillaAlPaso.Value.X;
            int c = casillaAlPaso.Value.Y;
            int pawnRow = currentTurn == "white" ? r + 1 : r - 1;

            foreach (int dc in new int[] { -1, 1 })
            {
                int cc = c + dc;
                if (!IsInBounds(pawnRow, cc)) continue;

                PieceInfo pi = GetPieceInfoFromCell(board[pawnRow, cc]);
                if (pi != null && pi.Type == "pawn" && pi.Color == currentTurn)
                    return true;
            }

            return false;
        }

        private bool IsInCheck(Point kingPos)
        {
            if (kingPos.X == -1) return false;
            string enemyColor = currentTurn == "white" ? "black" : "white";
            return IsSquareAttacked(kingPos, enemyColor);
        }

        // Verifica si la casilla es atacada por alguna pieza del color indicado
        private bool IsSquareAttacked(Point sq, string byColor)
        {
            int r = sq.X, c = sq.Y;

            // Peones: un peón de byColor ataca en diagonal según su sentido de avance
            int pawnBack = byColor == "white" ? 1 : -1; // fila desde la que vendría el peón atacante
            foreach (int dc in new int[] { -1, 1 })
            {
                int pr = r + pawnBack, pc = c + dc;
                if (IsInBounds(pr, pc))
                {
                    PieceInfo pi = GetPieceInfoFromCell(board[pr, pc]);
                    if (pi != null && pi.Color == byColor && pi.Type == "pawn")
                        return true;
                }
            }

            // Caballos
            int[,] knightOffsets = { { -2, -1 }, { -2, 1 }, { -1, -2 }, { -1, 2 },
                                     { 1, -2 }, { 1, 2 }, { 2, -1 }, { 2, 1 } };
            for (int i = 0; i < 8; i++)
            {
                int nr = r + knightOffsets[i, 0], nc = c + knightOffsets[i, 1];
                if (IsInBounds(nr, nc))
                {
                    PieceInfo pi = GetPieceInfoFromCell(board[nr, nc]);
                    if (pi != null && pi.Color == byColor && pi.Type == "knight")
                        return true;
                }
            }

            // Rey
            int[,] kingOffsets = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 },
                                   { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
            for (int i = 0; i < 8; i++)
            {
                int nr = r + kingOffsets[i, 0], nc = c + kingOffsets[i, 1];
                if (IsInBounds(nr, nc))
                {
                    PieceInfo pi = GetPieceInfoFromCell(board[nr, nc]);
                    if (pi != null && pi.Color == byColor && pi.Type == "king")
                        return true;
                }
            }

            // Torre / Reina (líneas rectas)
            int[,] orth = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            for (int d = 0; d < 4; d++)
            {
                int rr = r + orth[d, 0], cc = c + orth[d, 1];
                while (IsInBounds(rr, cc))
                {
                    PieceInfo pi = GetPieceInfoFromCell(board[rr, cc]);
                    if (pi != null)
                    {
                        if (pi.Color == byColor && (pi.Type == "rook" || pi.Type == "queen"))
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
                while (IsInBounds(rr, cc))
                {
                    PieceInfo pi = GetPieceInfoFromCell(board[rr, cc]);
                    if (pi != null)
                    {
                        if (pi.Color == byColor && (pi.Type == "bishop" || pi.Type == "queen"))
                            return true;
                        break;
                    }
                    rr += diag[d, 0];
                    cc += diag[d, 1];
                }
            }

            return false;
        }

        /// <summary>¿Está el rey del color indicado atacado?</summary>
        private bool ReyEnJaque(string color)
        {
            Point rey = BuscarRey(color);
            if (rey.X == -1) return false;
            string enemigo = color == "white" ? "black" : "white";
            return IsSquareAttacked(rey, enemigo);
        }

        private Point BuscarRey(string color)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    PieceInfo info = GetPieceInfoFromCell(board[i, j]);
                    if (info != null && info.Type == "king" && info.Color == color)
                        return new Point(i, j);
                }
            }
            return new Point(-1, -1);
        }

        /// <summary>Simula el movimiento y comprueba si deja al rey propio en jaque.</summary>
        private bool MovimientoDejaEnJaque(Point from, Point to, string color)
        {
            PictureBox origen = board[from.X, from.Y];
            PictureBox destino = board[to.X, to.Y];

            object tagOrigen = origen.Tag;
            Image imgDestino = destino.Image;
            object tagDestino = destino.Tag;

            PieceInfo pieza = tagOrigen as PieceInfo;
            string tipo = pieza != null ? pieza.Type : "queen";

            // Si es una captura al paso hay que quitar también el peón víctima
            bool quitarVictima = pieza != null && pieza.Type == "pawn"
                                 && casillaAlPaso.HasValue && to == casillaAlPaso.Value
                                 && destino.Image == null;

            Point victimaPos = new Point(-1, -1);
            Image victimaImg = null;
            object victimaTag = null;
            if (quitarVictima)
            {
                int victimRow = to.X + (color == "white" ? 1 : -1);
                victimaPos = new Point(victimRow, to.Y);
                victimaImg = board[victimRow, to.Y].Image;
                victimaTag = board[victimRow, to.Y].Tag;
                board[victimRow, to.Y].Image = null;
                board[victimRow, to.Y].Tag = new Point(victimRow, to.Y);
            }

            // Simular
            destino.Image = origen.Image;
            destino.Tag = new PieceInfo(color, tipo, to.X, to.Y, true);
            origen.Image = null;
            origen.Tag = new Point(from.X, from.Y);

            bool enJaque = ReyEnJaque(color);

            // Revertir
            origen.Image = destino.Image;
            origen.Tag = tagOrigen;
            destino.Image = imgDestino;
            destino.Tag = tagDestino;

            if (quitarVictima)
            {
                board[victimaPos.X, victimaPos.Y].Image = victimaImg;
                board[victimaPos.X, victimaPos.Y].Tag = victimaTag;
            }

            return enJaque;
        }

        /// <summary>¿Tiene el color indicado al menos un movimiento legal?</summary>
        private bool HayMovimientosLegales(string color)
        {
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PieceInfo info = GetPieceInfoFromCell(board[r, c]);
                    if (info != null && info.Color == color)
                    {
                        if (GetValidMoves(new Point(r, c), false).Count > 0)
                            return true;
                    }
                }
            }
            return false;
        }

        private void ResetGame()
        {
            // Limpiar el tablero
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j].Image = null;
                    board[i, j].Tag = new Point(i, j);
                }
            }

            // Reiniciar piezas
            SetupPieces();

            // Reiniciar contadores
            whiteCaptures = 0;
            blackCaptures = 0;
            UpdateCaptureCounters();

            // Reiniciar turno
            currentTurn = "white";

            // Reiniciar reglas estándar
            casillaAlPaso = null;
            halfmoveClock = 0;
            fullmoveNumber = 1;
            historialPosiciones.Clear();
            historialPosiciones.Add(PosicionClave());
            notaciones.Clear();
            partidaFinalizada = false;
            motivoFin = null;
            motivoGanadorFin = "empate";
            casillaJaque = null;
            casillaMate = null;
            casillaHover = null;

            // Reiniciar datos de persistencia (nueva partida)
            idPartida = -1;
            estadoPendiente = null;
            numMovimientos = 0;
            ultimoMovimiento = "";
            juegoTerminado = true;

            // Limpiar selección
            ClearSelection();

            // Limpiar historial de movimientos
            moveHistory.Clear();

            // Deshabilitar deshacer
            canUndo = false;

            // Refrescar el label de consejos
            MostrarConsejo(null);
        }

        // ----- Persistencia en Supabase -----
        private void GuardarPartida()
        {
            if (juegoTerminado || numMovimientos <= 0) return;
            if (string.IsNullOrWhiteSpace(Sesion.NombreUsuario)) return;

            if (idPartida < 0)
            {
                idPartida = PartidaRepository.CrearPartida(TIPO_JUEGO, nombreBlancas, nombreNegras,
                                PartidaRepository.ObtenerIdUsuario(Sesion.NombreUsuario));
                if (idPartida < 0) return;
            }

            // turno_final solo admite 'blancas' o 'negras'
            string turnoTexto = currentTurn == "white" ? "blancas" : "negras";
            PartidaRepository.GuardarEstado(idPartida, numMovimientos, turnoTexto,
                                            ultimoMovimiento, SerializarTablero());
        }

        private string SerializarTablero()
        {
            JArray cells = new JArray();
            for (int r = 0; r < BOARD_SIZE; r++)
            {
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PieceInfo info = GetPieceInfoFromCell(board[r, c]);
                    if (info == null)
                    {
                        cells.Add(JValue.CreateNull());
                    }
                    else
                    {
                        cells.Add(new JObject
                        {
                            ["c"] = info.Color,
                            ["t"] = info.Type,
                            ["m"] = info.HasMoved
                        });
                    }
                }
            }

            JObject o = new JObject
            {
                ["turn"] = currentTurn,
                ["wc"] = whiteCaptures,
                ["bc"] = blackCaptures,
                ["ia"] = modoIA,
                ["cia"] = colorIA,
                ["nivel"] = nivelIA,
                ["ep"] = casillaAlPaso.HasValue ? NombreCasilla(casillaAlPaso.Value) : null,
                ["hm"] = halfmoveClock,
                ["fm"] = fullmoveNumber,
                ["rep"] = new JArray(historialPosiciones),
                ["not"] = new JArray(notaciones),
                ["cells"] = cells
            };
            return o.ToString(Formatting.None);
        }

        private void AplicarEstado(string json)
        {
            try
            {
                JObject o = JObject.Parse(json);
                currentTurn = o["turn"] != null ? (string)o["turn"] : "white";
                whiteCaptures = o["wc"] != null ? (int)o["wc"] : 0;
                blackCaptures = o["bc"] != null ? (int)o["bc"] : 0;

                if (o["ia"] != null) modoIA = (bool)o["ia"];
                if (o["cia"] != null) colorIA = (string)o["cia"];
                if (o["nivel"] != null) nivelIA = (int)o["nivel"];
                if (o["hm"] != null) halfmoveClock = (int)o["hm"];
                if (o["fm"] != null) fullmoveNumber = (int)o["fm"];

                casillaAlPaso = null;
                if (o["ep"] != null && o["ep"].Type != JTokenType.Null)
                {
                    string ep = (string)o["ep"];
                    if (!string.IsNullOrEmpty(ep) && ep.Length >= 2)
                    {
                        int col = ep[0] - 'a';
                        int row = 8 - (ep[1] - '0');
                        if (IsInBounds(row, col)) casillaAlPaso = new Point(row, col);
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
                            board[r, c].Image = null;
                            board[r, c].Tag = new Point(r, c);
                        }
                        else
                        {
                            string color = (string)cell["c"];
                            string type = (string)cell["t"];
                            bool moved = cell["m"] != null && (bool)cell["m"];
                            board[r, c].Image = (Image)Properties.Resources.ResourceManager
                                                .GetObject(NombreImagen(color, type));
                            board[r, c].Tag = new PieceInfo(color, type, r, c, moved);
                        }
                    }
                }

                historialPosiciones.Clear();
                JArray rep = o["rep"] as JArray;
                if (rep != null)
                {
                    foreach (JToken t in rep)
                        historialPosiciones.Add((string)t);
                }
                if (historialPosiciones.Count == 0)
                    historialPosiciones.Add(PosicionClave());

                notaciones.Clear();
                JArray not = o["not"] as JArray;
                if (not != null)
                {
                    foreach (JToken t in not)
                        notaciones.Add((string)t);
                }

                partidaFinalizada = false;
                motivoFin = null;

                UpdateCaptureCounters();
                juegoTerminado = false;
            }
            catch { }
        }

        private static string NombreImagen(string color, string type)
        {
            if (color == "black")
            {
                switch (type)
                {
                    case "rook": return "Torre_n";
                    case "knight": return "caballo_n";
                    case "bishop": return "alfil_n";
                    case "queen": return "reina_n";
                    case "king": return "rey_n";
                    default: return "peon_negro_pixel";
                }
            }
            else
            {
                switch (type)
                {
                    case "rook": return "torre";
                    case "knight": return "caballo";
                    case "bishop": return "alfil_blanco";
                    case "queen": return "reina";
                    case "king": return "rey";
                    default: return "peon";
                }
            }
        }

        private void RegistrarFinAjedrez(string ganador)
        {
            juegoTerminado = true;
            if (idPartida >= 0)
            {
                PartidaRepository.FinalizarPartida(idPartida, ganador);
                PartidaRepository.RegistrarResultado(nombreBlancas, nombreNegras, ganador);
            }
            idPartida = -1;
            numMovimientos = 0;
            ultimoMovimiento = "";
        }

        // ----- Jugada de la IA (Stockfish) -----

        /// <summary>Arranca el motor si hace falta (descargándolo la primera vez).</summary>
        private bool AsegurarMotor()
        {
            if (motor != null)
            {
                try { motor.EstablecerNivel(nivelIA); return true; }
                catch { try { motor.Dispose(); } catch { } motor = null; }
            }

            string exe = StockfishDescarga.Buscar();
            if (exe == null)
            {
                DialogResult r = MessageBox.Show(
                    "Para jugar contra la IA se necesita el motor Stockfish (~35 MB).\n\n" +
                    "¿Descargarlo ahora desde el sitio oficial de Stockfish?",
                    "Motor de ajedrez", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (r != DialogResult.Yes) return false;

                exe = StockfishDescarga.Descargar(this);
                if (exe == null)
                {
                    MessageBox.Show("No se pudo descargar el motor. La partida seguirá en modo local.",
                                    "Ajedrez", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            try
            {
                motor = new StockfishEngine(exe);
                motor.Iniciar();
                motor.EstablecerNivel(nivelIA);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo iniciar el motor de ajedrez: " + ex.Message,
                                "Ajedrez", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                motor = null;
                return false;
            }
        }

        private int TiempoPorNivel()
        {
            if (nivelIA <= 2) return 300;
            if (nivelIA <= 8) return 600;
            if (nivelIA <= 15) return 1000;
            return 1500;
        }

        /// <summary>Lanza en segundo plano el cálculo de la jugada de la IA.</summary>
        private void IniciarJugadaIA()
        {
            if (iaPensando || juegoTerminado) return;
            if (!AsegurarMotor())
            {
                // Sin motor: se desactiva la IA y el juego continúa en modo local
                modoIA = false;
                return;
            }

            iaPensando = true;
            string fen = GenerarFen();
            int tiempo = TiempoPorNivel();

            Task.Factory.StartNew(delegate
            {
                string best = null;
                Exception err = null;
                try { best = motor.MejorMovimiento(fen, tiempo); }
                catch (Exception ex) { err = ex; }

                try
                {
                    this.BeginInvoke(new Action(delegate
                    {
                        iaPensando = false;

                        if (err != null)
                        {
                            MessageBox.Show("Error del motor de ajedrez: " + err.Message,
                                            "Ajedrez", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        ProcesarRespuestaIA(best);
                    }));
                }
                catch { }
            });
        }

        private void ProcesarRespuestaIA(string best)
        {
            if (string.IsNullOrWhiteSpace(best) || best == "(none)") return;
            if (best.Length < 4) return;

            Point from = PuntoDeCasilla(best.Substring(0, 2));
            Point to = PuntoDeCasilla(best.Substring(2, 2));

            PieceInfo info = GetPieceInfoFromCell(board[from.X, from.Y]);
            if (info == null || info.Color != colorIA) return;

            // El movimiento del motor debe ser legal en nuestra posición
            if (!GetValidMoves(from).Contains(to))
            {
                modoIA = false;
                Avisar("La IA propuso un movimiento no válido y se ha desactivado.\n" +
                       "Puedes continuar jugando los dos bandos.",
                       "Ajedrez", MessageBoxIcon.Warning);
                return;
            }

            string promocion = null;
            if (best.Length >= 5)
            {
                switch (char.ToLower(best[4]))
                {
                    case 'r': promocion = "rook"; break;
                    case 'b': promocion = "bishop"; break;
                    case 'n': promocion = "knight"; break;
                    default: promocion = "queen"; break;
                }
            }

            AplicarMovimiento(from, to, promocion);
        }

        private static Point PuntoDeCasilla(string casilla)
        {
            int col = casilla[0] - 'a';
            int row = 8 - (casilla[1] - '0');
            return new Point(row, col);
        }

        /// <summary>Genera la notación FEN de la posición actual.</summary>
        private string GenerarFen()
        {
            StringBuilder sb = new StringBuilder();

            for (int r = 0; r < BOARD_SIZE; r++)
            {
                int vacias = 0;
                for (int c = 0; c < BOARD_SIZE; c++)
                {
                    PieceInfo info = GetPieceInfoFromCell(board[r, c]);
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

            string turno = currentTurn == "white" ? "w" : "b";
            string enroques = CalcularEnroques();
            string ep = casillaAlPaso.HasValue ? NombreCasilla(casillaAlPaso.Value) : "-";
            return sb.ToString() + " " + turno + " " + enroques + " " + ep
                   + " " + halfmoveClock + " " + fullmoveNumber;
        }

        private static char LetraFen(PieceInfo info)
        {
            char c;
            switch (info.Type)
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

            if (!IsUnmovedPiece(row, 4, color, "king")) return false;

            int rookCol = flancoRey ? 7 : 0;
            return IsUnmovedPiece(row, rookCol, color, "rook");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            GuardarPartida();

            if (motor != null)
            {
                motor.Dispose();
                motor = null;
            }

            base.OnFormClosing(e);
        }

        // Clase para almacenar información de las piezas
        private class PieceInfo
        {
            public string Color { get; set; }
            public string Type { get; set; }
            public int Row { get; set; }
            public int Col { get; set; }
            public bool HasMoved { get; set; }

            public PieceInfo(string color, string type, int row, int col, bool hasMoved = false)
            {
                Color = color;
                Type = type;
                Row = row;
                Col = col;
                HasMoved = hasMoved;
            }
        }

        private void m_claro_Click(object sender, EventArgs e)
        {
            
            Tema.SetTheme(false);      // Cambia a tema claro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
            RefrescarColores();        // Mantiene las marcas de jaque/mate sobre el tablero
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
          
           
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
            RefrescarColores();        // Mantiene las marcas de jaque/mate sobre el tablero

        }

        private void menu_Click(object sender, EventArgs e)
        {
          //NO SIRVE
        }

        private void menu_Click_1(object sender, EventArgs e)
        {
            if (!ConfirmarIrAlMenu()) return;

            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }

        /// <summary>
        /// Si hay una partida en curso, pregunta si finalizarla o continuarla
        /// más tarde. Devuelve true si se puede ir al menú.
        /// </summary>
        private bool ConfirmarIrAlMenu()
        {
            if (juegoTerminado || idPartida < 0) return true;

            DialogResult r = MessageBox.Show(
                "¿Quieres finalizar la partida o continuarla más tarde?\n\n" +
                "Sí = Finalizar la partida (no se podrá continuar)\n" +
                "No = Continuar más tarde (se guarda la posición)",
                "Partida en curso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                // Finalizar la partida: el resultado se decide por las piezas comidas
                string ganador = GanadorPorPiezasComidas();
                PartidaRepository.FinalizarPartida(idPartida, ganador);
                PartidaRepository.RegistrarResultado(nombreBlancas, nombreNegras, ganador);
                idPartida = -1;
                numMovimientos = 0;
                ultimoMovimiento = "";
                juegoTerminado = true;
            }
            else
            {
                // Continuar más tarde: se guarda el estado actual
                GuardarPartida();
            }
            return true;
        }

        private void deshacer_Click(object sender, EventArgs e)
        {
            if (partidaFinalizada)
            {
                Avisar("La partida ya ha terminado y el tablero está congelado.", "Ajedrez",
                       MessageBoxIcon.Information);
                return;
            }

            if (moveHistory.Count == 0)
            {
                Avisar("No hay movimientos para deshacer.", "Ajedrez",
                       MessageBoxIcon.Information);
                return;
            }

            if (!canUndo)
            {
                Avisar("Solo puedes deshacer el último movimiento, antes de que juegue el rival.", "Ajedrez",
                       MessageBoxIcon.Information);
                return;
            }

            if (iaPensando)
            {
                Avisar("Espera a que la IA termine de pensar.", "Ajedrez",
                       MessageBoxIcon.Information);
                return;
            }

            ClearSelection();

            // En modo IA se deshacen las dos últimas jugadas (la respuesta de la IA y la tuya)
            int plies = modoIA ? 2 : 1;
            while (plies > 0 && moveHistory.Count > 0)
            {
                DeshacerUnMovimiento();
                plies--;
            }

            ultimoMovimiento = "Deshacer";
            ultimoFueCaptura = false;
            ultimoFueAlPaso = false;
            ultimoFueCoronacion = false;
            ultimoFueEnroqueCorto = false;
            ultimoFueEnroqueLargo = false;

            ActualizarResaltadoJaque();
            MostrarConsejo(null);
            GuardarPartida();

            // Ya no se puede deshacer hasta que se vuelva a mover una pieza
            canUndo = false;
        }

        private void DeshacerUnMovimiento()
        {
            MoveHistory move = moveHistory.Pop();

            // Mover la pieza de vuelta a su casilla de origen (con su imagen y estado originales)
            board[move.From.X, move.From.Y].Image = move.MovedImage;
            board[move.From.X, move.From.Y].Tag = move.MovedPiece;

            // Restaurar lo que había en la casilla de destino (pieza capturada o vacía)
            board[move.To.X, move.To.Y].Image = move.CapturedImage;
            board[move.To.X, move.To.Y].Tag = move.CapturedTag ?? (object)new Point(move.To.X, move.To.Y);

            // Si fue un enroque, regresar también la torre
            if (move.IsCastle)
            {
                board[move.RookFrom.X, move.RookFrom.Y].Image = board[move.RookTo.X, move.RookTo.Y].Image;
                board[move.RookFrom.X, move.RookFrom.Y].Tag = move.RookInfo;
                board[move.RookTo.X, move.RookTo.Y].Image = null;
                board[move.RookTo.X, move.RookTo.Y].Tag = new Point(move.RookTo.X, move.RookTo.Y);
            }

            // Si fue una captura al paso, devolver el peón capturado
            if (move.IsEnPassant)
            {
                board[move.EnPassantVictimPos.X, move.EnPassantVictimPos.Y].Image = move.EnPassantVictimImage;
                board[move.EnPassantVictimPos.X, move.EnPassantVictimPos.Y].Tag = move.EnPassantVictimTag;

                PieceInfo capInfo = move.EnPassantVictimTag as PieceInfo;
                if (capInfo != null)
                {
                    if (capInfo.Color == "white")
                        blackCaptures--;
                    else
                        whiteCaptures--;
                    UpdateCaptureCounters();
                }
            }

            // Ajustar contadores de capturas si se deshace una captura normal
            if (move.CapturedImage != null)
            {
                PieceInfo capInfo = move.CapturedTag as PieceInfo;
                if (capInfo != null)
                {
                    if (capInfo.Color == "white")
                        blackCaptures--;
                    else
                        whiteCaptures--;
                    UpdateCaptureCounters();
                }
            }

            // Restaurar el estado de las reglas estándar
            casillaAlPaso = move.AlPasoAnterior;
            halfmoveClock = move.HalfmoveAnterior;
            fullmoveNumber = move.FullmoveAnterior;

            if (historialPosiciones.Count > 0)
                historialPosiciones.RemoveAt(historialPosiciones.Count - 1);
            if (notaciones.Count > 0)
                notaciones.RemoveAt(notaciones.Count - 1);

            // Cambiar el turno de vuelta
            currentTurn = move.MovedPiece.Color;

            if (numMovimientos > 0) numMovimientos--;
        }

        // Clase para almacenar el historial de movimientos
        private class MoveHistory
        {
            public Point From { get; set; }
            public Point To { get; set; }
            public Image CapturedImage { get; set; }
            public object CapturedTag { get; set; }
            public PieceInfo MovedPiece { get; set; }
            public Image MovedImage { get; set; }
            public bool IsCastle { get; set; }
            public Point RookFrom { get; set; }
            public Point RookTo { get; set; }
            public PieceInfo RookInfo { get; set; }
            public bool IsEnPassant { get; set; }
            public Point EnPassantVictimPos { get; set; }
            public Image EnPassantVictimImage { get; set; }
            public object EnPassantVictimTag { get; set; }
            public Point? AlPasoAnterior { get; set; }
            public int HalfmoveAnterior { get; set; }
            public int FullmoveAnterior { get; set; }

            public MoveHistory(Point from, Point to, Image capturedImage, object capturedTag, PieceInfo movedPiece)
            {
                From = from;
                To = to;
                CapturedImage = capturedImage;
                CapturedTag = capturedTag;
                MovedPiece = movedPiece;
            }
        }
    }
}