using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Página de partida de ajedrez (equivalente al formulario Ajedrez_tablero
    /// del escritorio). Toda la lógica de reglas vive en AjedrezGame; aquí se
    /// gestionan los clics del tablero, la IA, el consejo y la persistencia.
    /// </summary>
    public partial class Ajedrez : System.Web.UI.Page
    {
        private AjedrezGame juego;
        private LinkButton[,] celdas;

        private AjedrezGame Juego { get { return juego; } }

        /// <summary>Si la IA lleva las blancas el tablero se muestra volteado (negras abajo).</summary>
        private bool Volteado
        {
            get { return Juego != null && Juego.ModoIA && Juego.ColorIA == "white"; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            juego = Session["EstadoAjedrez"] as AjedrezGame;
            if (juego == null)
            {
                PartidaConfig cfg = PartidaSesion.Actual;
                if (cfg == null)
                {
                    Response.Redirect("Menu.aspx");
                    return;
                }

                juego = new AjedrezGame();
                juego.ModoIA = cfg.ModoIA;
                juego.NivelIA = cfg.NivelIA;
                if (cfg.ModoIA)
                    juego.ColorIA = CalcularColorIA(cfg);

                if (!string.IsNullOrWhiteSpace(cfg.EstadoInicial))
                    juego.AplicarEstado(cfg.EstadoInicial);

                // La partida se crea en la nube en cuanto se abre la página
                if (cfg.IdPartida == null)
                {
                    int id = PartidaRepository.CrearPartida("ajedrez", cfg.Blancas, cfg.Negras,
                                    PartidaRepository.ObtenerIdUsuario(Sesion.NombreUsuario), false);
                    cfg.IdPartida = id;
                }

                Session["EstadoAjedrez"] = juego;
            }

            // Los nombres se leen de la configuración en cada petición (como en el escritorio)
            PartidaConfig config = PartidaSesion.Actual;
            if (config != null)
            {
                juego.NombreBlancas = string.IsNullOrWhiteSpace(config.Blancas) ? "Blancas" : config.Blancas;
                juego.NombreNegras = string.IsNullOrWhiteSpace(config.Negras) ? "Negras" : config.Negras;
            }

            CrearTablero();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblAviso.Text = "";
            if (Juego == null) return;

            if (!IsPostBack)
            {
                MostrarConsejo(null);

                // Si la IA lleva las blancas, debe abrir la partida
                if (Juego.ModoIA && !Juego.PartidaFinalizada && Juego.TurnoActual == Juego.ColorIA)
                    JugarIA();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Juego == null) return;
            PintarTablero();
            ActualizarLateral();
        }

        // ----- Configuración del modo IA -----

        /// <summary>La IA juega con el color contrario al del jugador local (igual que en el escritorio).</summary>
        private static string CalcularColorIA(PartidaConfig cfg)
        {
            // El local es el usuario de la sesión; si eligió negras, la IA juega con blancas.
            string usuario = Sesion.NombreUsuario;
            if (!string.IsNullOrWhiteSpace(usuario)
                && string.Equals(usuario, cfg.Negras, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(usuario, cfg.Blancas, StringComparison.OrdinalIgnoreCase))
                return "white";
            return "black";
        }

        private static int TiempoPorNivel(int nivelIA)
        {
            if (nivelIA <= 2) return 300;
            if (nivelIA <= 8) return 600;
            if (nivelIA <= 15) return 1000;
            return 1500;
        }

        // ----- Tablero -----

        private void CrearTablero()
        {
            celdas = new LinkButton[AjedrezGame.BOARD_SIZE, AjedrezGame.BOARD_SIZE];
            tablero.Rows.Clear();

            for (int dr = 0; dr < AjedrezGame.BOARD_SIZE; dr++)
            {
                HtmlTableRow fila = new HtmlTableRow();

                int logFila = Volteado ? AjedrezGame.BOARD_SIZE - 1 - dr : dr;
                HtmlTableCell celdaNum = new HtmlTableCell("th");
                celdaNum.Attributes["class"] = "coordenada";
                celdaNum.InnerHtml = (AjedrezGame.BOARD_SIZE - logFila).ToString();
                fila.Cells.Add(celdaNum);

                for (int dc = 0; dc < AjedrezGame.BOARD_SIZE; dc++)
                {
                    int logCol = Volteado ? AjedrezGame.BOARD_SIZE - 1 - dc : dc;

                    HtmlTableCell celda = new HtmlTableCell("td");
                    celda.Attributes["class"] = (logFila + logCol) % 2 == 0
                        ? "casilla casilla-clara" : "casilla casilla-oscura";

                    LinkButton btn = new LinkButton();
                    btn.ID = "cf" + dr + "_" + dc;
                    btn.CommandArgument = logFila + "," + logCol;
                    btn.Command += Casilla_Click;
                    btn.Text = "&nbsp;";
                    btn.CssClass = "enlace-casilla";

                    celda.Controls.Add(btn);
                    celdas[logFila, logCol] = btn;
                    fila.Cells.Add(celda);
                }

                tablero.Rows.Add(fila);
            }

            // Fila inferior con las letras de las columnas
            HtmlTableRow filaAbajo = new HtmlTableRow();
            HtmlTableCell esquina = new HtmlTableCell("th");
            esquina.Attributes["class"] = "coordenada";
            filaAbajo.Cells.Add(esquina);

            for (int dc = 0; dc < AjedrezGame.BOARD_SIZE; dc++)
            {
                int logCol = Volteado ? AjedrezGame.BOARD_SIZE - 1 - dc : dc;
                HtmlTableCell celdaLetra = new HtmlTableCell("th");
                celdaLetra.Attributes["class"] = "coordenada";
                celdaLetra.InnerHtml = ((char)('a' + logCol)).ToString();
                filaAbajo.Cells.Add(celdaLetra);
            }
            tablero.Rows.Add(filaAbajo);
        }

        /// <summary>
        /// Repinta el tablero combinando selección, movimientos válidos, última
        /// jugada, jaque (amarillo) y jaque mate (rojo), con la misma prioridad
        /// de colores que RefrescarColores del escritorio.
        /// </summary>
        private void PintarTablero()
        {
            var g = Juego;
            Point? sel = ObtenerSeleccion();
            List<Point> validos = sel.HasValue ? g.MovimientosValidos(sel.Value) : new List<Point>();

            Point? ultOrigen = null, ultDestino = null;
            if (g.HistorialMovimientos.Count > 0)
            {
                MovHistorial h = g.HistorialMovimientos.Peek();
                ultOrigen = h.Origen;
                ultDestino = h.Destino;
            }

            for (int r = 0; r < AjedrezGame.BOARD_SIZE; r++)
            {
                for (int c = 0; c < AjedrezGame.BOARD_SIZE; c++)
                {
                    LinkButton btn = celdas[r, c];
                    if (btn == null) continue;

                    PiezaInfo p = g.Casillas[r, c];
                    btn.Text = p == null ? "&nbsp;" : AjedrezGame.Glifo(p.Color, p.Tipo);

                    string cls = "enlace-casilla";
                    if (p != null) cls += p.Color == "white" ? " ficha-b" : " ficha-n";

                    Point casilla = new Point(r, c);

                    // Destinos legales de la pieza seleccionada
                    if (validos.Contains(casilla))
                        cls += " resaltado-mov";

                    // Pieza seleccionada
                    if (sel.HasValue && sel.Value == casilla)
                        cls += " resaltado-sel";

                    // Última jugada (ambas casillas)
                    if ((ultOrigen.HasValue && ultOrigen.Value == casilla)
                        || (ultDestino.HasValue && ultDestino.Value == casilla))
                        cls += " resaltado-ultimo";

                    // Rey en jaque: amarillo
                    if (g.JaqueF == r && g.JaqueC == c)
                        cls += " resaltado-jaque";

                    // Jaque mate: el rojo tiene prioridad absoluta
                    if (g.MateF == r && g.MateC == c)
                        cls += " resaltado-mate";

                    btn.CssClass = cls;
                }
            }
        }

        private void ActualizarLateral()
        {
            var g = Juego;

            puntosb.Text = "PUNTOS BLANCAS: " + g.NombreBlancas;
            puntosn.Text = "PUNTOS NEGRAS: " + g.NombreNegras;
            p_blancas.Text = g.BlancasCapturaron.ToString();
            p_negras.Text = g.NegrasCapturaron.ToString();

            // Tiras de piezas capturadas, en glifos, junto al contador del bando que comió
            StringBuilder tb = new StringBuilder();
            foreach (string tipo in g.CapturadasPorBlancas)
                tb.Append(AjedrezGame.Glifo("black", tipo));
            puntos_b.Text = tb.ToString();

            StringBuilder tn = new StringBuilder();
            foreach (string tipo in g.CapturadasPorNegras)
                tn.Append(AjedrezGame.Glifo("white", tipo));
            puntos_n.Text = tn.ToString();
        }

        // ----- Selección -----

        private Point? ObtenerSeleccion()
        {
            if (!(ViewState["SelF"] is int) || !(ViewState["SelC"] is int)) return null;
            int f = (int)ViewState["SelF"];
            int c = (int)ViewState["SelC"];
            if (f < 0 || c < 0) return null;
            return new Point(f, c);
        }

        private void LimpiarSeleccion()
        {
            ViewState["SelF"] = -1;
            ViewState["SelC"] = -1;
        }

        // ----- Clic en una casilla -----

        protected void Casilla_Click(object sender, CommandEventArgs e)
        {
            var g = Juego;
            if (g == null) return;

            // Tras el jaque mate o unas tablas el tablero queda congelado
            if (g.PartidaFinalizada) return;

            // Esperando a que el jugador elija la pieza de coronación
            if (pnlPromocion.Visible) return;

            // Durante el turno de la IA no se permiten jugadas humanas
            if (g.ModoIA && g.TurnoActual == g.ColorIA) return;

            string[] partes = ((string)e.CommandArgument).Split(',');
            int r = int.Parse(partes[0]);
            int c = int.Parse(partes[1]);

            // Si hay una pieza seleccionada y se hace clic en una casilla válida
            Point? sel = ObtenerSeleccion();
            if (sel.HasValue && g.MovimientosValidos(sel.Value).Contains(new Point(r, c)))
            {
                PiezaInfo moving = g.Casillas[sel.Value.X, sel.Value.Y];
                if (moving != null && moving.Tipo == "pawn"
                    && (r == 0 || r == AjedrezGame.BOARD_SIZE - 1))
                {
                    // Preguntar al jugador por la pieza a la que corona el peón
                    ViewState["PromOrF"] = sel.Value.X;
                    ViewState["PromOrC"] = sel.Value.Y;
                    ViewState["PromDstF"] = r;
                    ViewState["PromDstC"] = c;
                    pnlPromocion.Visible = true;
                    return;
                }

                AplicarYMover(sel.Value, new Point(r, c), null);
                return;
            }

            // Seleccionar una pieza del color que le toca
            PiezaInfo info = g.Casillas[r, c];
            if (info != null && info.Color == g.TurnoActual)
            {
                ViewState["SelF"] = r;
                ViewState["SelC"] = c;
            }
        }

        // ----- Promoción -----

        protected void btnPromReina_Click(object sender, EventArgs e) { Promover("queen"); }
        protected void btnPromTorre_Click(object sender, EventArgs e) { Promover("rook"); }
        protected void btnPromAlfil_Click(object sender, EventArgs e) { Promover("bishop"); }
        protected void btnPromCaballo_Click(object sender, EventArgs e) { Promover("knight"); }

        private void Promover(string tipo)
        {
            var g = Juego;
            pnlPromocion.Visible = false;
            if (g == null || g.PartidaFinalizada) return;
            if (!(ViewState["PromOrF"] is int) || !(ViewState["PromDstF"] is int)) return;

            int of = (int)ViewState["PromOrF"];
            int oc = (int)ViewState["PromOrC"];
            int df = (int)ViewState["PromDstF"];
            int dc = (int)ViewState["PromDstC"];

            AplicarYMover(new Point(of, oc), new Point(df, dc), tipo);
        }

        // ----- Aplicar jugada y respuesta de la IA -----

        private void AplicarYMover(Point origen, Point destino, string promocion)
        {
            var g = Juego;
            LimpiarSeleccion();
            g.AplicarMovimiento(origen, destino, promocion);

            if (g.PartidaFinalizada)
            {
                RegistrarFinAjedrez(g.MotivoGanadorFin);
                MostrarConsejo(null);
            }
            else
            {
                MostrarConsejo(g.UltimaNotacion);
                if (g.JaqueF >= 0)
                    lblAviso.Text = "¡Jaque al rey de las " + AjedrezGame.ColorTexto(g.TurnoActual) + "!";
                GuardarPartida();
            }

            // Si ahora le toca a la IA, que responda
            if (g.ModoIA && !g.PartidaFinalizada && g.TurnoActual == g.ColorIA)
                JugarIA();
        }

        /// <summary>Busca o descarga el motor y devuelve su ruta (o null si no hay motor).</summary>
        private string AsegurarMotor()
        {
            string exe = StockfishDescarga.Buscar();
            if (exe != null) return exe;

            lblAviso.Text = "Descargando Stockfish (~35 MB)…";
            exe = StockfishDescarga.Descargar();
            if (exe == null)
                lblAviso.Text = "No se pudo descargar el motor. La partida seguirá en modo local.";
            return exe;
        }

        /// <summary>Calcula y aplica la jugada de la IA de forma síncrona (como en el escritorio).</summary>
        private void JugarIA()
        {
            var g = Juego;
            string exe = AsegurarMotor();
            if (exe == null)
            {
                // Sin motor: se desactiva la IA y el juego continúa en modo local
                g.ModoIA = false;
                return;
            }

            string best = null;
            try
            {
                using (var motor = new StockfishEngine(exe))
                {
                    motor.Iniciar();
                    motor.EstablecerNivel(g.NivelIA);
                    best = motor.MejorMovimiento(g.GenerarFen(), TiempoPorNivel(g.NivelIA));
                }
            }
            catch (Exception ex)
            {
                g.ModoIA = false;
                lblAviso.Text = "No se pudo iniciar el motor de ajedrez: " + ex.Message;
                return;
            }

            ProcesarRespuestaIA(best);
        }

        private void ProcesarRespuestaIA(string best)
        {
            var g = Juego;
            if (string.IsNullOrWhiteSpace(best) || best == "(none)") return;
            if (best.Length < 4) return;

            Point from = AjedrezGame.PuntoDeCasilla(best.Substring(0, 2));
            Point to = AjedrezGame.PuntoDeCasilla(best.Substring(2, 2));

            PiezaInfo info = g.Casillas[from.X, from.Y];
            if (info == null || info.Color != g.ColorIA) return;

            // El movimiento del motor debe ser legal en nuestra posición
            if (!g.MovimientosValidos(from).Contains(to))
            {
                g.ModoIA = false;
                lblAviso.Text = "La IA propuso un movimiento no válido y se ha desactivado.<br />" +
                                "Puedes continuar jugando los dos bandos.";
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

            AplicarYMover(from, to, promocion);
        }

        // ----- Consejos -----

        private void MostrarConsejo(string san)
        {
            var g = Juego;
            if (g == null) return;

            if (g.PartidaFinalizada && !string.IsNullOrEmpty(g.MotivoFin))
            {
                consejo.Text = g.MotivoFin.Replace("\n", "<br />");
                return;
            }

            var partes = new List<string>();
            var rnd = new Random();
            bool hayJaque = g.JaqueF >= 0;

            if (san != null)
            {
                partes.Add("Jugada: " + san);

                string contexto = Consejos.Contexto(san, g.UltimoFueCaptura, g.UltimoFueEnroqueCorto,
                                   g.UltimoFueEnroqueLargo, g.UltimoFueAlPaso, g.UltimoFueCoronacion);

                if (hayJaque)
                    contexto = Consejos.MensajeJaque;

                if (contexto != null)
                    partes.Add(contexto);
                else
                    partes.Add(rnd.Next(2) == 0
                        ? Consejos.FraseMotivacional(rnd)
                        : Consejos.ConsejoEstrategico(rnd));
            }
            else if (hayJaque)
            {
                partes.Add(Consejos.MensajeJaque);
            }
            else
            {
                partes.Add(rnd.Next(2) == 0
                    ? Consejos.FraseMotivacional(rnd)
                    : Consejos.ConsejoEstrategico(rnd));
            }

            string apertura = Consejos.ReconocerApertura(g.Notaciones);
            if (apertura != null) partes.Insert(0, "Apertura: " + apertura);

            consejo.Text = string.Join("<br />", partes);
        }

        // ----- Deshacer -----

        protected void deshacer_Click(object sender, EventArgs e)
        {
            var g = Juego;
            if (g == null) return;

            if (g.PartidaFinalizada)
            {
                lblAviso.Text = "La partida ya ha terminado y el tablero está congelado.";
                return;
            }

            if (g.HistorialMovimientos.Count == 0)
            {
                lblAviso.Text = "No hay movimientos para deshacer.";
                return;
            }

            if (!g.PuedeDeshacer)
            {
                lblAviso.Text = "Solo puedes deshacer el último movimiento, antes de que juegue el rival.";
                return;
            }

            LimpiarSeleccion();
            pnlPromocion.Visible = false;

            // En modo IA se deshacen las dos últimas jugadas (la respuesta de la IA y la tuya)
            int plies = g.ModoIA ? 2 : 1;
            while (plies > 0 && g.HistorialMovimientos.Count > 0)
            {
                g.DeshacerUnMovimiento();
                plies--;
            }

            g.UltimoMovimiento = "Deshacer";
            g.UltimoFueCaptura = false;
            g.UltimoFueAlPaso = false;
            g.UltimoFueCoronacion = false;
            g.UltimoFueEnroqueCorto = false;
            g.UltimoFueEnroqueLargo = false;

            g.ActualizarResaltadoJaque();
            MostrarConsejo(null);
            GuardarPartida();

            // Ya no se puede deshacer hasta que se vuelva a mover una pieza
            g.PuedeDeshacer = false;
        }

        // ----- Menú -----

        protected void ir_menu_Click(object sender, EventArgs e)
        {
            // La partida queda "en_curso" en la nube para poder continuarla desde el menú.
            GuardarPartida();
            Session.Remove("EstadoAjedrez");
            Response.Redirect("Menu.aspx");
        }

        // ----- Persistencia en Supabase -----

        private void GuardarPartida()
        {
            var g = Juego;
            PartidaConfig cfg = PartidaSesion.Actual;
            if (g == null || cfg == null) return;
            if (g.NumMovimientos <= 0) return;

            int id = cfg.IdPartida.HasValue ? cfg.IdPartida.Value : -1;
            if (id < 0) return;

            // turno_final solo admite 'blancas' o 'negras'
            string turnoTexto = g.TurnoActual == "white" ? "blancas" : "negras";
            PartidaRepository.GuardarEstado(id, g.NumMovimientos, turnoTexto,
                                            g.UltimoMovimiento, g.Serializar());
        }

        private void RegistrarFinAjedrez(string ganador)
        {
            var g = Juego;
            PartidaConfig cfg = PartidaSesion.Actual;

            if (cfg != null && cfg.IdPartida.HasValue && cfg.IdPartida.Value >= 0)
            {
                PartidaRepository.FinalizarPartida(cfg.IdPartida.Value, ganador);
                PartidaRepository.RegistrarResultado(cfg.Blancas, cfg.Negras, ganador);
                cfg.IdPartida = null;
            }

            if (g != null)
            {
                g.NumMovimientos = 0;
                g.UltimoMovimiento = "";
            }
        }
    }
}
