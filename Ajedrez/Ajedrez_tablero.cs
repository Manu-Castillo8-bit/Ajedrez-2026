using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class Ajedrez_tablero : Form
    {
        private const int BOARD_SIZE = 8;
        private const int CELL_SIZE = 60;
        private PictureBox[,] board;
        private PictureBox selectedPiece = null;
        private Point selectedPosition;
        private string currentTurn = "white"; // "white" o "black"
        private int whiteCaptures = 0;
        private int blackCaptures = 0;
        private List<Point> validMoves = new List<Point>();

        public Ajedrez_tablero()
        {
            InitializeComponent();
            InitializeBoard();
            SetupPieces();
        }

        private void Ajedrez_tablero_Load(object sender, EventArgs e)
        {
            // El tablero ya se inicializa en el constructor
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
            PlacePiece(7, 2, "alfil", "white", "bishop");
            PlacePiece(7, 3, "reina", "white", "queen");
            PlacePiece(7, 4, "rey", "white", "king");
            PlacePiece(7, 5, "alfil", "white", "bishop");
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

            Point pos = GetCellPosition(clickedCell);
            if (pos.X == -1 && pos.Y == -1) return;

            // Si hay una pieza seleccionada y se hace clic en una casilla válida
            if (selectedPiece != null && validMoves.Contains(pos))
            {
                // Mover la pieza
                MovePiece(pos);
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
            // Limpiar selección anterior
            ClearSelection();

            selectedPiece = piece;
            selectedPosition = GetCellPosition(piece);

            // Calcular movimientos válidos
            validMoves = GetValidMoves(selectedPosition);

            // Resaltar movimientos
            foreach (Point move in validMoves)
            {
                board[move.X, move.Y].BackColor = Color.LightGreen;
            }

            // Resaltar pieza seleccionada
            piece.BackColor = Color.Yellow;
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
            if (selectedPiece != null)
            {
                // Restaurar color original
                Point pos = GetCellPosition(selectedPiece);
                if (pos.X != -1 && pos.Y != -1)
                {
                    selectedPiece.BackColor = (pos.X + pos.Y) % 2 == 0 ? Color.White : Color.Black;
                }
                selectedPiece = null;
            }

            // Limpiar movimientos válidos
            foreach (Point move in validMoves)
            {
                Point pos = GetCellPosition(board[move.X, move.Y]);
                if (pos.X != -1 && pos.Y != -1)
                {
                    board[move.X, move.Y].BackColor = (pos.X + pos.Y) % 2 == 0 ? Color.White : Color.Black;
                }
            }
            validMoves.Clear();
        }

        private void MovePiece(Point targetPos)
        {
            Point sourcePos = GetCellPosition(selectedPiece);
            PieceInfo sourceInfo = GetPieceInfoFromCell(selectedPiece);

            if (sourceInfo == null) return;

            // Capturar pieza si existe
            if (board[targetPos.X, targetPos.Y].Image != null)
            {
                PieceInfo targetInfo = GetPieceInfoFromCell(board[targetPos.X, targetPos.Y]);
                if (targetInfo != null)
                {
                    if (targetInfo.Color == "white")
                        blackCaptures++;
                    else
                        whiteCaptures++;
                }

                UpdateCaptureCounters();
            }

            // Mover la pieza
            board[targetPos.X, targetPos.Y].Image = selectedPiece.Image;
            board[targetPos.X, targetPos.Y].Tag = new PieceInfo(sourceInfo.Color, sourceInfo.Type, targetPos.X, targetPos.Y);

            // Limpiar casilla origen
            selectedPiece.Image = null;
            selectedPiece.Tag = new Point(sourcePos.X, sourcePos.Y);

            // Cambiar turno
            currentTurn = currentTurn == "white" ? "black" : "white";

            // Limpiar selección
            ClearSelection();

            // Verificar jaque o jaque mate
            CheckGameState();
        
        }

        private List<Point> GetValidMoves(Point pos)
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
                    break;
            }

            return moves;
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

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < BOARD_SIZE && col >= 0 && col < BOARD_SIZE;
        }

        private void Board_MouseEnter(object sender, EventArgs e)

        {
            PictureBox cell = sender as PictureBox;
            if (cell == null) return;

            if (cell.Image != null && cell != selectedPiece)
            {
                // Efecto hover sutil
                if (cell.BackColor == Color.White)
                    cell.BackColor = Color.FromArgb(220, 220, 220);
                else
                    cell.BackColor = Color.FromArgb(80, 80, 80);
            }
        }
        

        private void Board_MouseLeave(object sender, EventArgs e)
        {
            PictureBox cell = sender as PictureBox;
            if (cell == null) return;

            Point pos = GetCellPosition(cell);
            if (pos.X == -1 && pos.Y == -1) return;

            // Restaurar color original si no está seleccionada
            if (cell != selectedPiece && !validMoves.Contains(pos))
            {
                cell.BackColor = (pos.X + pos.Y) % 2 == 0 ? Color.White : Color.Black;
            }
        }
        

        private void UpdateCaptureCounters()
        {
            p_blancas.Text = whiteCaptures.ToString();
            p_negras.Text = blackCaptures.ToString();
        }

        private void CheckGameState()
        {
            // Buscar el rey del turno actual
            string currentKing = currentTurn == "white" ? "rey_blanco" : "rey_negro";
            Point kingPos = FindKing(currentKing);

            if (IsInCheck(kingPos))
            {
                MessageBox.Show($"¡Jaque al rey {currentTurn}!", "Ajedrez",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Verificar jaque mate
                if (IsCheckMate(kingPos))
                {
                    string winner = currentTurn == "white" ? "Negras" : "Blancas";
                    MessageBox.Show($"¡Jaque mate! ¡Ganan las {winner}!",
                                  "Fin del juego", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetGame();
                }
            }
        }

        private Point FindKing(string kingImageName)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j].Image != null)
                    {
                        PieceInfo info = GetPieceInfoFromCell(board[i, j]);
                        if (info != null && info.Type == "king" && info.Color == currentTurn)
                            return new Point(i, j);
                    }
                }
            }
            return new Point(-1, -1);
        }

        private bool IsInCheck(Point kingPos)
        {
            // Verificar si alguna pieza enemiga puede capturar al rey
            string enemyColor = currentTurn == "white" ? "black" : "white";

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j].Image != null)
                    {
                        PieceInfo info = (PieceInfo)board[i, j].Tag;
                        if (info.Color == enemyColor)
                        {
                            List<Point> moves = GetValidMoves(new Point(i, j));
                            if (moves.Contains(kingPos))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsCheckMate(Point kingPos)
        {
            // Verificar si el rey puede moverse
            List<Point> kingMoves = GetValidMoves(kingPos);
            foreach (Point move in kingMoves)
            {
                // Simular el movimiento
                PieceInfo kingInfo = (PieceInfo)board[kingPos.X, kingPos.Y].Tag;
                Image tempImage = board[move.X, move.Y].Image;
                object tempTag = board[move.X, move.Y].Tag;

                board[move.X, move.Y].Image = board[kingPos.X, kingPos.Y].Image;
                board[move.X, move.Y].Tag = new PieceInfo(kingInfo.Color, kingInfo.Type, move.X, move.Y);
                board[kingPos.X, kingPos.Y].Image = null;

                bool stillInCheck = IsInCheck(move);

                // Deshacer movimiento
                board[kingPos.X, kingPos.Y].Image = board[move.X, move.Y].Image;
                board[kingPos.X, kingPos.Y].Tag = new PieceInfo(kingInfo.Color, kingInfo.Type, kingPos.X, kingPos.Y);
                board[move.X, move.Y].Image = tempImage;
                board[move.X, move.Y].Tag = tempTag;

                if (!stillInCheck)
                    return false;
            }

            return true;
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

            // Limpiar selección
            ClearSelection();
        }

        // Clase para almacenar información de las piezas
        private class PieceInfo
        {
            public string Color { get; set; }
            public string Type { get; set; }
            public int Row { get; set; }
            public int Col { get; set; }

            public PieceInfo(string color, string type, int row, int col)
            {
                Color = color;
                Type = type;
                Row = row;
                Col = col;
            }
        }

        private void m_claro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(false);      // Cambia a tema claro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
        }

        private void menu_Click(object sender, EventArgs e)
        {
          //NO SIRVE
        }

        private void menu_Click_1(object sender, EventArgs e)
        {
            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }
    }
}