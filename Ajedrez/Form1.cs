using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ajedrez
{
    public partial class Form1 : Form
    {
        public Form1()
            : this(string.IsNullOrWhiteSpace(Sesion.NombreUsuario) ? "Jugador" : Sesion.NombreUsuario, "Oponente")
        {
        }

        public Form1(string jugadorBlancas, string jugadorNegras)
            : this(jugadorBlancas, jugadorNegras, false)
        {
        }

        public Form1(string jugadorBlancas, string jugadorNegras, bool soplar)
        {
            InitializeComponent();
            nombreBlancas = string.IsNullOrWhiteSpace(jugadorBlancas) ? "Jugador" : jugadorBlancas;
            nombreNegras = string.IsNullOrWhiteSpace(jugadorNegras) ? "Oponente" : jugadorNegras;
            modoSoplar = soplar;
            Tema.ApplyTheme(this);
        }

        int n = 8;
        PictureBox[,] P;
        int[,] tablero;
        int turno = 1; // 1 = blancas (arriba / oponente), -1 = negras (abajo / jugador local)

        string nombreBlancas = "Jugador";
        string nombreNegras = "Oponente";
        bool finPartida = false;
        bool modoSoplar = false; // true = se sopla la pieza que podía comer; false = captura obligatoria

        int blancas = 0, negras = 0;

        // Estado de selección / resaltado
        List<Point> marcados = new List<Point>();
        bool enCapturaMultiple = false;
        List<List<Paso>> secuenciasPendientes = null;
        List<Mov> movimientosSimplesActuales = null;
        int piezaR = -1, piezaC = -1, piezaV = 0;
        int selR = -1, selC = -1; // casilla de la ficha seleccionada (resaltado verde)
        bool snapshotTomado = false;
        bool canUndo = false; // solo se puede deshacer el último movimiento (una vez por jugada)

        Stack<BoardSnapshot> boardHistory = new Stack<BoardSnapshot>();

        // ----- Persistencia en Supabase -----
        private const string TIPO_JUEGO = "damas";
        private int idPartida = -1;
        private string estadoPendiente = null;
        private int numMovimientos = 0;
        private string ultimoMovimiento = "";

        /// <summary>Prepara la partida para continuar desde un estado guardado.</summary>
        public void ContinuarPartida(int id, string estadoJson)
        {
            idPartida = id;
            estadoPendiente = estadoJson;
        }

        // ----- Clases auxiliares -----
        private class Paso
        {
            public int R, C;
            public int CapR = -1, CapC = -1;
            public Paso() { }
            public Paso(int r, int c, int cr, int cc) { R = r; C = c; CapR = cr; CapC = cc; }
        }

        private class Mov
        {
            public int R, C;
            public List<Paso> Pasos = new List<Paso>();
            public bool EsCaptura { get { return Pasos.Any(p => p.CapR >= 0); } }
        }

        private class BoardSnapshot
        {
            public int[,] Tablero;
            public int Turno;
            public int Blancas;
            public int Negras;
            public bool FinPartida;
        }

        // ----- Temas / navegación -----
        private void m_claro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(false);
            Tema.ApplyTheme(this);
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);
            Tema.ApplyTheme(this);
        }

        private void ir_menu_Click(object sender, EventArgs e)
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
            if (finPartida || idPartida < 0) return true;

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
                finPartida = true;
            }
            else
            {
                // Continuar más tarde: se guarda el estado actual
                GuardarPartida();
            }
            return true;
        }

        // ----- Inicialización -----
        private void Form1_Load(object sender, EventArgs e)
        {
            tablero = new int[n, n];
            P = new PictureBox[n, n];
            int left = 2, top = 2;

            for (int i = 0; i < n; i++)
            {
                left = 2;
                for (int j = 0; j < n; j++)
                {
                    bool casillaOscura = ((i + j) % 2 == 0);

                    P[i, j] = new PictureBox();
                    P[i, j].BackColor = casillaOscura ? Color.Black : Color.White;
                    P[i, j].Location = new Point(left, top);
                    P[i, j].Size = new Size(60, 60);
                    left += 60;
                    P[i, j].Name = i + " " + j;
                    P[i, j].SizeMode = PictureBoxSizeMode.Zoom;

                    if (casillaOscura && i < 3)
                        tablero[i, j] = 1;          // blancas (arriba)
                    else if (casillaOscura && i > 4)
                        tablero[i, j] = -1;         // negras (abajo)

                    int ci = i, cj = j;
                    P[i, j].Click += (sender2, e2) => CasillaClick(ci, cj);

                    G.Controls.Add(P[i, j]);
                }
                top += 60;
            }

            Render();
            ActualizarPuntos();

            // Mostrar junto a los puntos el nombre del jugador que lleva cada color
            puntosb.Text = "PUNTOS BLANCAS: " + nombreBlancas;
            puntosn.Text = "PUNTOS NEGRAS: " + nombreNegras;

            // Si venimos de una partida guardada, reconstruir el tablero.
            if (!string.IsNullOrWhiteSpace(estadoPendiente))
            {
                AplicarEstado(estadoPendiente);
                estadoPendiente = null;
            }
        }

        // ----- Persistencia -----
        private void GuardarPartida()
        {
            if (finPartida) return;
            if (string.IsNullOrWhiteSpace(Sesion.NombreUsuario)) return;

            if (idPartida < 0)
            {
                idPartida = PartidaRepository.CrearPartida(TIPO_JUEGO, nombreBlancas, nombreNegras,
                                PartidaRepository.ObtenerIdUsuario(Sesion.NombreUsuario), modoSoplar);
                if (idPartida < 0) return;
            }

            // turno_final solo admite 'blancas' o 'negras'
            string turnoTexto = turno == 1 ? "blancas" : "negras";
            PartidaRepository.GuardarEstado(idPartida, numMovimientos, turnoTexto,
                                            ultimoMovimiento, SerializarTablero());
        }

        private string SerializarTablero()
        {
            int[] t = new int[n * n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    t[i * n + j] = tablero[i, j];

            JObject o = new JObject
            {
                ["turno"] = turno,
                ["blancas"] = blancas,
                ["negras"] = negras,
                ["soplar"] = modoSoplar,
                ["t"] = new JArray(t)
            };
            return o.ToString(Formatting.None);
        }

        private void AplicarEstado(string json)
        {
            try
            {
                JObject o = JObject.Parse(json);
                turno = o["turno"] != null ? (int)o["turno"] : 1;
                blancas = o["blancas"] != null ? (int)o["blancas"] : 0;
                negras = o["negras"] != null ? (int)o["negras"] : 0;

                // Restaurar si en esta partida se podía soplar (regla de soplar)
                if (o["soplar"] != null) modoSoplar = (bool)o["soplar"];

                JArray arr = (JArray)o["t"];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        tablero[i, j] = (int)arr[i * n + j];

                finPartida = false;
                Render();
                ActualizarPuntos();
            }
            catch { }
        }

        private void RegistrarFinDamas(string ganadorNombre)
        {
            if (idPartida >= 0)
            {
                PartidaRepository.FinalizarPartida(idPartida, ganadorNombre);
                PartidaRepository.RegistrarResultado(nombreBlancas, nombreNegras, ganadorNombre);
            }
            idPartida = -1;
            numMovimientos = 0;
            ultimoMovimiento = "";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            GuardarPartida();
            base.OnFormClosing(e);
        }

        // ----- Dibujo -----
        private void SetImagen(int i, int j)
        {
            int v = tablero[i, j];
            if (v == 1) P[i, j].Image = Properties.Resources.f_blanca;
            else if (v == 2) P[i, j].Image = Properties.Resources.corona_b;
            else if (v == -1) P[i, j].Image = Properties.Resources.f_negra;
            else if (v == -2) P[i, j].Image = Properties.Resources.corona_n;
            else P[i, j].Image = null;
        }

        private void Render()
        {
            LimpiarResaltadoSeleccion();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    SetImagen(i, j);
            marcados.Clear();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (tablero == null) return;

            using (Font f = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold))
            using (SolidBrush br = new SolidBrush(Tema.IsDarkMode ? Color.White : Color.Black))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                for (int j = 0; j < n; j++)
                {
                    RectangleF rect = new RectangleF(12 + j * 60, 494, 60, 16);
                    e.Graphics.DrawString(((char)('a' + j)).ToString(), f, br, rect, sf);
                }

                for (int i = 0; i < n; i++)
                {
                    RectangleF rect = new RectangleF(0, 12 + i * 60, 11, 60);
                    e.Graphics.DrawString((8 - i).ToString(), f, br, rect, sf);
                }
            }
        }

        private void Marcar(int r, int c)
        {
            if (!Dentro(r, c)) return;
            P[r, c].Image = Properties.Resources.Movimiento;
            marcados.Add(new Point(r, c));
        }

        private void LimpiarMarcados()
        {
            foreach (Point pt in marcados) SetImagen(pt.X, pt.Y);
            marcados.Clear();
        }

        // Resalta en verde la casilla de la ficha seleccionada
        private void ResaltarSeleccion(int r, int c)
        {
            LimpiarResaltadoSeleccion();
            if (!Dentro(r, c)) return;
            P[r, c].BackColor = Color.LimeGreen;
            selR = r; selC = c;
        }

        private void LimpiarResaltadoSeleccion()
        {
            if (selR < 0 || selC < 0) return;
            P[selR, selC].BackColor = ((selR + selC) % 2 == 0) ? Color.Black : Color.White;
            selR = selC = -1;
        }

        private void ActualizarPuntos()
        {
            p_blancas.Text = blancas + "";
            p_negras.Text = negras + "";
        }

        // Nombre del jugador según el lado (1 = blancas arriba, -1 = negras abajo)
        private string NombreDeLado(int lado)
        {
            return lado == 1 ? nombreBlancas : nombreNegras;
        }

        // Al finalizar la partida, gana quien haya comido más piezas (empate si igualan)
        private string GanadorPorPiezasComidas()
        {
            if (blancas > negras) return nombreBlancas;
            if (negras > blancas) return nombreNegras;
            return "empate";
        }

        // ----- Interacción -----
        private void CasillaClick(int r, int c)
        {
            if (finPartida) return;

            // Clic sobre una casilla destino resaltada -> mover
            if (marcados.Any(pt => pt.X == r && pt.Y == c))
            {
                EjecutarDestino(r, c);
                return;
            }

            int v = tablero[r, c];
            if (v == 0 || Math.Sign(v) != turno) return;

            List<Mov> dePieza = MovimientosDeLado(tablero, turno).Where(m => m.R == r && m.C == c).ToList();
            if (dePieza.Count == 0) return; // pieza sin movimientos legales (p. ej. captura obligatoria en otra pieza)

            LimpiarMarcados();
            piezaR = r; piezaC = c; piezaV = v;

            if (dePieza.Any(m => m.EsCaptura))
            {
                secuenciasPendientes = dePieza.Where(m => m.EsCaptura)
                                              .Select(m => new List<Paso>(m.Pasos)).ToList();
                enCapturaMultiple = true;
                movimientosSimplesActuales = null;
                DestacarPrimeros();
            }
            else
            {
                enCapturaMultiple = false;
                secuenciasPendientes = null;
                movimientosSimplesActuales = dePieza;
                foreach (Mov m in dePieza) Marcar(m.Pasos[0].R, m.Pasos[0].C);
                ResaltarSeleccion(piezaR, piezaC);
            }
        }

        private void DestacarPrimeros()
        {
            LimpiarMarcados();
            foreach (var seq in secuenciasPendientes)
            {
                if (seq.Count == 0) continue;
                Marcar(seq[0].R, seq[0].C);
            }
            ResaltarSeleccion(piezaR, piezaC);
        }

        private void EjecutarDestino(int r, int c)
        {
            if (enCapturaMultiple)
            {
                var filtradas = secuenciasPendientes.Where(s => s.Count > 0 && s[0].R == r && s[0].C == c).ToList();
                if (filtradas.Count == 0) return;

                if (!snapshotTomado) { SaveBoardState(); snapshotTomado = true; }

                Paso paso = filtradas[0][0];
                foreach (var s in filtradas) s.RemoveAt(0);
                AplicarPaso(paso);

                LimpiarMarcados();
                Render();

                int v = tablero[piezaR, piezaC];
                bool corona = Math.Abs(v) == 1 && ((v > 0 && piezaR == 7) || (v < 0 && piezaR == 0));

                if (corona)
                {
                    tablero[piezaR, piezaC] = (v > 0) ? 2 : -2; // coronación termina la jugada
                    Render();
                    TerminarTurno();
                    return;
                }

                var restantes = filtradas.Where(s => s.Count > 0).ToList();
                if (restantes.Count > 0)
                {
                    secuenciasPendientes = restantes;
                    enCapturaMultiple = true;
                    DestacarPrimeros();
                    return;
                }

                secuenciasPendientes = null;
                enCapturaMultiple = false;
                TerminarTurno();
            }
            else
            {
                Mov mov = movimientosSimplesActuales.FirstOrDefault(m => m.Pasos[0].R == r && m.Pasos[0].C == c);
                if (mov == null) return;

                // Piezas que podían comer antes de este movimiento (modo soplar)
                List<Point> podianComer = modoSoplar ? PiezasQuePuedenComer(tablero, turno) : null;

                if (!snapshotTomado) { SaveBoardState(); snapshotTomado = true; }

                tablero[piezaR, piezaC] = 0;
                tablero[r, c] = piezaV;

                LimpiarMarcados();
                Render();

                int v = tablero[r, c];
                if (Math.Abs(v) == 1 && ((v > 0 && r == 7) || (v < 0 && r == 0)))
                {
                    tablero[r, c] = (v > 0) ? 2 : -2;
                    Render();
                }

                SoplarPiezas(podianComer);

                TerminarTurno();
            }
        }

        private void AplicarPaso(Paso p)
        {
            tablero[piezaR, piezaC] = 0;
            tablero[p.R, p.C] = piezaV;

            if (p.CapR >= 0)
            {
                int cap = tablero[p.CapR, p.CapC];
                tablero[p.CapR, p.CapC] = 0;
                if (cap > 0) negras++;
                else if (cap < 0) blancas++;
                ActualizarPuntos();
            }

            piezaR = p.R; piezaC = p.C;
        }

        private void TerminarTurno()
        {
            marcados.Clear();
            secuenciasPendientes = null;
            movimientosSimplesActuales = null;
            enCapturaMultiple = false;
            piezaR = piezaC = -1;
            piezaV = 0;
            LimpiarResaltadoSeleccion();
            snapshotTomado = false;
            canUndo = true; // se acaba de mover: se puede deshacer una vez

            turno = -turno;

            List<Mov> movs = MovimientosDeLado(tablero, turno);
            if (movs.Count == 0)
            {
                finPartida = true;
                int ganador = -turno;
                string nombreGanador = NombreDeLado(ganador);
                MessageBox.Show("¡Ganó " + nombreGanador + "!", "DAMAS",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RegistrarFinDamas(nombreGanador);
            }
            else
            {
                numMovimientos++;
                ultimoMovimiento = "Jugada " + numMovimientos;
                GuardarPartida();
            }
        }

        // ----- Regla de soplar -----
        private List<Point> PiezasQuePuedenComer(int[,] b, int side)
        {
            var res = new List<Point>();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (b[i, j] != 0 && Math.Sign(b[i, j]) == side && CapturasDePieza(b, i, j).Count > 0)
                        res.Add(new Point(i, j));
            return res;
        }

        private void SoplarPiezas(List<Point> piezas)
        {
            if (piezas == null || piezas.Count == 0) return;

            List<Point> reales = piezas.Where(pt => tablero[pt.X, pt.Y] != 0).ToList();
            if (reales.Count == 0) return;

            // 1) Resaltar en rojo las casillas que se van a soplar
            foreach (Point pt in reales)
                P[pt.X, pt.Y].BackColor = Color.Red;

            Refresh();
            Application.DoEvents();

            // 2) Aviso indicando la notación de la(s) ficha(s)
            string nombres = string.Join(", ", reales.Select(pt => NombreCasilla(pt.X, pt.Y)).ToArray());
            string aviso = reales.Count == 1
                ? "¡No comiste! Se soplará la ficha que estaba en " + nombres + "."
                : "¡No comiste! Se soplarán las fichas que estaban en " + nombres + ".";
            MessageBox.Show(aviso, "SOPLADO", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            // 3) Mantener el resaltado rojo ~medio segundo
            System.Threading.Thread.Sleep(500);
            Application.DoEvents();

            // 4) Quitar las piezas y restaurar el color de las casillas
            bool soplada = false;
            foreach (Point pt in reales)
            {
                int v = tablero[pt.X, pt.Y];
                if (v == 0) continue;

                tablero[pt.X, pt.Y] = 0;
                if (v > 0) negras++; else if (v < 0) blancas++;
                soplada = true;

                bool oscura = ((pt.X + pt.Y) % 2 == 0);
                P[pt.X, pt.Y].BackColor = oscura ? Color.Black : Color.White;
            }

            if (soplada)
            {
                Render();
                ActualizarPuntos();
            }
        }

        private string NombreCasilla(int r, int c)
        {
            return ((char)('a' + c)).ToString() + (8 - r);
        }

        // ----- Generación de movimientos -----
        private bool Dentro(int r, int c)
        {
            return r >= 0 && r < n && c >= 0 && c < n;
        }

        private List<Mov> MovimientosDeLado(int[,] b, int side)
        {
            var capturas = new List<Mov>();
            var simples = new List<Mov>();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
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
                // Modo soplar: también se ofrecen movimientos simples; si no comes,
                // se soplará la pieza que podía comer.
                if (!modoSoplar)
                    return mejores;

                mejores.AddRange(simples);
                return mejores;
            }

            return simples;
        }

        private List<List<Paso>> CapturasDePieza(int[,] b, int r, int c)
        {
            var salida = new List<List<Paso>>();
            int v = b[r, c];
            if (v == 0) return salida;
            GenerarCapturasRec(b, r, c, v, new List<Paso>(), salida);
            return salida;
        }

        private void GenerarCapturasRec(int[,] b, int r, int c, int v, List<Paso> acum, List<List<Paso>> salida)
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
                    salida.Add(new List<Paso>(acum));
                else
                    GenerarCapturasRec(b, p.R, p.C, v, acum, salida);

                acum.RemoveAt(acum.Count - 1);
                b[p.R, p.C] = 0;
                b[r, c] = v;
                b[p.CapR, p.CapC] = guardado;
            }
        }

        private List<Paso> SaltosSimples(int[,] b, int r, int c, int v)
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

        private List<Paso> MovimientosSimplesDePieza(int[,] b, int r, int c)
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

        // ----- Deshacer -----
        private void SaveBoardState()
        {
            if (tablero == null) return;

            BoardSnapshot snap = new BoardSnapshot();
            snap.Tablero = (int[,])tablero.Clone();
            snap.Turno = turno;
            snap.Blancas = blancas;
            snap.Negras = negras;
            snap.FinPartida = finPartida;

            boardHistory.Push(snap);
        }

        private void deshacer_Click(object sender, EventArgs e)
        {
            if (boardHistory.Count == 0)
            {
                MessageBox.Show("No hay movimientos para deshacer.", "DAMAS",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!canUndo)
            {
                MessageBox.Show("Solo puedes deshacer el último movimiento antes de que se juegue la siguiente jugada.",
                    "DAMAS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BoardSnapshot snap = boardHistory.Pop();

            tablero = (int[,])snap.Tablero.Clone();
            turno = snap.Turno;
            blancas = snap.Blancas;
            negras = snap.Negras;
            finPartida = snap.FinPartida;

            secuenciasPendientes = null;
            movimientosSimplesActuales = null;
            enCapturaMultiple = false;
            piezaR = piezaC = -1;
            piezaV = 0;
            LimpiarResaltadoSeleccion();
            snapshotTomado = false;
            canUndo = false;

            Render();
            ActualizarPuntos();

            if (numMovimientos > 0) numMovimientos--;
            GuardarPartida();
        }
    }
}
