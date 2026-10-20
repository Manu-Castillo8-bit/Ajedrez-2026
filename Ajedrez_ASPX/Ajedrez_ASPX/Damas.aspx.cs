using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Página de damas (equivalente web de Form1 del escritorio). Las reglas
    /// viven en DamasGame; aquí están la interacción, el pintado del tablero
    /// y la persistencia de la partida en Supabase.
    /// </summary>
    public partial class Damas : System.Web.UI.Page
    {
        private const string CLAVE_ESTADO = "EstadoDamas";

        private DamasEstado Estado
        {
            get { return Session[CLAVE_ESTADO] as DamasEstado; }
            set { Session[CLAVE_ESTADO] = value; }
        }

        // ----- Ciclo de vida -----

        protected void Page_Load(object sender, EventArgs e)
        {
            // Sin sesión iniciada no se puede jugar ni guardar partidas
            if (!Acceso.ExigirSesion(this)) return;

            if (!IsPostBack)
            {
                if (!Inicializar()) return;
            }
            Pintar();
        }

        /// <summary>
        /// Crea el estado de la partida (nueva o continuada). Equivale al
        /// constructor + Form1_Load + ContinuarPartida del escritorio.
        /// </summary>
        private bool Inicializar()
        {
            PartidaConfig cfg = PartidaSesion.Actual;
            if (cfg == null)
            {
                Response.Redirect("Menu.aspx");
                return false;
            }

            var est = new DamasEstado();
            est.NombreBlancas = string.IsNullOrWhiteSpace(cfg.Blancas) ? "Jugador" : cfg.Blancas;
            est.NombreNegras = string.IsNullOrWhiteSpace(cfg.Negras) ? "Oponente" : cfg.Negras;
            est.ModoSoplar = cfg.Soplar;
            DamasGame.TableroInicial(est.Tablero);

            // Si todavía no se había creado la partida en la nube, crearla ahora.
            if (cfg.IdPartida == null)
            {
                int id = PartidaRepository.CrearPartida("damas", cfg.Blancas, cfg.Negras,
                    PartidaRepository.ObtenerIdUsuario(Sesion.NombreUsuario), cfg.Soplar);
                cfg.IdPartida = id;
            }
            est.IdPartida = cfg.IdPartida.Value;

            // Si venimos de una partida guardada, reconstruir el tablero
            // (igual que AplicarEstado del escritorio).
            if (!string.IsNullOrWhiteSpace(cfg.EstadoInicial))
                DamasGame.AplicarEstado(est, cfg.EstadoInicial);

            Estado = est;
            return true;
        }

        // ----- Pintado -----

        private void Pintar()
        {
            DamasEstado est = Estado;
            if (est == null) return;

            // Puntos junto al nombre del jugador que lleva cada color
            // (puntosb / puntosn del escritorio).
            lblPuntosBlancas.Text = "PUNTOS BLANCAS: " + est.NombreBlancas;
            lblPuntosNegras.Text = "PUNTOS NEGRAS: " + est.NombreNegras;
            pBlancas.Text = est.Blancas + "";
            pNegras.Text = est.Negras + "";
            litComidasBlancas.Text = RepetirFicha("pieza-blanca", est.Blancas);
            litComidasNegras.Text = RepetirFicha("pieza-negra", est.Negras);

            lblAviso.Text = est.Aviso;
            lblAviso.CssClass = "mensaje damas-aviso" + (est.FinPartida ? " fin-partida" : "");

            PintarTablero(est);
        }

        private void PintarTablero(DamasEstado est)
        {
            phTablero.Controls.Clear();

            var t = new Table();
            t.CellPadding = 0;
            t.CellSpacing = 0;
            t.CssClass = "tablero-damas";

            for (int r = 0; r < DamasGame.N; r++)
            {
                var tr = new TableRow();

                // Columna de números (8..1), igual que el OnPaint del escritorio
                var tdNum = new TableCell();
                tdNum.CssClass = "celda coord";
                tdNum.Text = (DamasGame.N - r).ToString();
                tr.Cells.Add(tdNum);

                for (int c = 0; c < DamasGame.N; c++)
                {
                    var td = new TableCell();
                    bool oscura = ((r + c) % 2 == 0);
                    td.CssClass = "celda " + (oscura ? "oscura" : "clara");

                    int v = est.Tablero[r, c];

                    var btn = new LinkButton();
                    btn.ID = "cas" + r + "_" + c;
                    btn.CommandArgument = r + "," + c;
                    btn.Click += Casilla_Click;
                    btn.ToolTip = DamasGame.NombreCasilla(r, c);
                    btn.CssClass = "casilla";

                    if (v != 0)
                    {
                        btn.CssClass += (v > 0) ? " pieza-blanca" : " pieza-negra";
                        btn.Text = (Math.Abs(v) == 2) ? "\u265B" : "\u25CF"; // dama (corona) o ficha
                    }

                    // Resaltados: selección (verde), destinos, sopladas (rojo) y último movimiento
                    if (r == est.SelR && c == est.SelC) td.CssClass += " seleccionada";
                    if (est.Marcados.Any(pt => pt.R == r && pt.C == c)) btn.CssClass += " destino";
                    if (est.Sopladas.Any(pt => pt.R == r && pt.C == c)) td.CssClass += " soplada";
                    if ((r == est.UltimoOrigenR && c == est.UltimoOrigenC)
                        || (r == est.UltimoDestinoR && c == est.UltimoDestinoC)) td.CssClass += " ultimo-mov";

                    td.Controls.Add(btn);
                    tr.Cells.Add(td);
                }

                t.Rows.Add(tr);
            }

            // Fila de letras (a..h) debajo del tablero
            var trLetras = new TableRow();
            var tdEsquina = new TableCell();
            tdEsquina.CssClass = "celda coord";
            trLetras.Cells.Add(tdEsquina);
            for (int c = 0; c < DamasGame.N; c++)
            {
                var td = new TableCell();
                td.CssClass = "celda coord texto-centrado";
                td.Text = ((char)('a' + c)).ToString();
                trLetras.Cells.Add(td);
            }
            t.Rows.Add(trLetras);

            phTablero.Controls.Add(t);
        }

        private static string RepetirFicha(string clase, int cantidad)
        {
            if (cantidad <= 0) return string.Empty;
            var sb = new StringBuilder();
            int tope = Math.Min(cantidad, 12);
            for (int i = 0; i < tope; i++)
                sb.Append("<span class=\"ficha-mini ").Append(clase).Append("\">&#9679;</span>");
            return sb.ToString();
        }

        // ----- Interacción con las casillas -----

        private void Casilla_Click(object sender, EventArgs e)
        {
            DamasEstado est = Estado;
            if (est == null || est.FinPartida) return;

            pnlMenu.Visible = false; // cerrar la confirmación al interactuar con el tablero

            var btn = (LinkButton)sender;
            string[] partes = btn.CommandArgument.Split(',');
            int r = int.Parse(partes[0]);
            int c = int.Parse(partes[1]);

            // Un clic nuevo descarta el aviso y el resaltado rojo del soplar
            // (equivale a cerrar el MessageBox del escritorio).
            est.Aviso = "";
            est.Sopladas.Clear();

            // Clic sobre una casilla destino resaltada -> mover
            if (est.Marcados.Any(pt => pt.R == r && pt.C == c))
            {
                EjecutarDestino(est, r, c);
                Pintar();
                return;
            }

            int v = est.Tablero[r, c];
            if (v == 0 || Math.Sign(v) != est.Turno)
            {
                Pintar();
                return;
            }

            List<Mov> dePieza = DamasGame.MovimientosDeLado(est.Tablero, est.Turno, est.ModoSoplar)
                .Where(m => m.R == r && m.C == c).ToList();
            if (dePieza.Count == 0)
            {
                // pieza sin movimientos legales (p. ej. captura obligatoria en otra pieza)
                Pintar();
                return;
            }

            est.Marcados.Clear();
            est.PiezaR = r; est.PiezaC = c; est.PiezaV = v;

            if (dePieza.Any(m => m.EsCaptura))
            {
                est.SecuenciasPendientes = dePieza.Where(m => m.EsCaptura)
                                                  .Select(m => new List<Paso>(m.Pasos)).ToList();
                est.EnCapturaMultiple = true;
                est.MovimientosSimplesActuales = null;
                DestacarPrimeros(est);
            }
            else
            {
                est.EnCapturaMultiple = false;
                est.SecuenciasPendientes = null;
                est.MovimientosSimplesActuales = dePieza;
                foreach (Mov m in dePieza) est.Marcados.Add(new Casilla(m.Pasos[0].R, m.Pasos[0].C));
                ResaltarSeleccion(est, r, c);
            }

            Pintar();
        }

        private static void DestacarPrimeros(DamasEstado est)
        {
            est.Marcados.Clear();
            foreach (var seq in est.SecuenciasPendientes)
            {
                if (seq.Count == 0) continue;
                est.Marcados.Add(new Casilla(seq[0].R, seq[0].C));
            }
            ResaltarSeleccion(est, est.PiezaR, est.PiezaC);
        }

        private static void ResaltarSeleccion(DamasEstado est, int r, int c)
        {
            if (!DamasGame.Dentro(r, c)) return;
            est.SelR = r; est.SelC = c;
        }

        private void EjecutarDestino(DamasEstado est, int r, int c)
        {
            if (est.EnCapturaMultiple)
            {
                var filtradas = est.SecuenciasPendientes
                    .Where(s => s.Count > 0 && s[0].R == r && s[0].C == c).ToList();
                if (filtradas.Count == 0) return;

                if (!est.SnapshotTomado)
                {
                    GuardarSnapshot(est);
                    est.SnapshotTomado = true;
                    est.UltimoOrigenR = est.PiezaR;
                    est.UltimoOrigenC = est.PiezaC;
                }

                Paso paso = filtradas[0][0];
                foreach (var s in filtradas) s.RemoveAt(0);
                AplicarPaso(est, paso);

                est.Marcados.Clear();
                est.SelR = est.SelC = -1; // equivalente a Render() del escritorio

                int v = est.Tablero[est.PiezaR, est.PiezaC];
                bool corona = Math.Abs(v) == 1 && ((v > 0 && est.PiezaR == 7) || (v < 0 && est.PiezaR == 0));

                if (corona)
                {
                    est.Tablero[est.PiezaR, est.PiezaC] = (v > 0) ? 2 : -2; // la coronación termina la jugada
                    TerminarTurno(est);
                    return;
                }

                var restantes = filtradas.Where(s => s.Count > 0).ToList();
                if (restantes.Count > 0)
                {
                    est.SecuenciasPendientes = restantes;
                    est.EnCapturaMultiple = true;
                    DestacarPrimeros(est);
                    return;
                }

                est.SecuenciasPendientes = null;
                est.EnCapturaMultiple = false;
                TerminarTurno(est);
            }
            else
            {
                Mov mov = est.MovimientosSimplesActuales == null
                    ? null
                    : est.MovimientosSimplesActuales.FirstOrDefault(m => m.Pasos[0].R == r && m.Pasos[0].C == c);
                if (mov == null) return;

                // Piezas que podían comer antes de este movimiento (modo soplar)
                List<Casilla> podianComer = est.ModoSoplar
                    ? DamasGame.PiezasQuePuedenComer(est.Tablero, est.Turno) : null;

                if (!est.SnapshotTomado)
                {
                    GuardarSnapshot(est);
                    est.SnapshotTomado = true;
                }
                est.UltimoOrigenR = est.PiezaR;
                est.UltimoOrigenC = est.PiezaC;

                est.Tablero[est.PiezaR, est.PiezaC] = 0;
                est.Tablero[r, c] = est.PiezaV;

                est.Marcados.Clear();
                est.SelR = est.SelC = -1;

                int v = est.Tablero[r, c];
                if (Math.Abs(v) == 1 && ((v > 0 && r == 7) || (v < 0 && r == 0)))
                    est.Tablero[r, c] = (v > 0) ? 2 : -2;

                est.UltimoDestinoR = r;
                est.UltimoDestinoC = c;

                SoplarPiezas(est, podianComer);

                TerminarTurno(est);
            }
        }

        private static void AplicarPaso(DamasEstado est, Paso p)
        {
            est.Tablero[est.PiezaR, est.PiezaC] = 0;
            est.Tablero[p.R, p.C] = est.PiezaV;

            if (p.CapR >= 0)
            {
                int cap = est.Tablero[p.CapR, p.CapC];
                est.Tablero[p.CapR, p.CapC] = 0;
                if (cap > 0) est.Negras++;
                else if (cap < 0) est.Blancas++;
            }

            est.PiezaR = p.R; est.PiezaC = p.C;
            est.UltimoDestinoR = p.R; est.UltimoDestinoC = p.C;
        }

        private void TerminarTurno(DamasEstado est)
        {
            est.Marcados.Clear();
            est.SecuenciasPendientes = null;
            est.MovimientosSimplesActuales = null;
            est.EnCapturaMultiple = false;
            est.PiezaR = est.PiezaC = -1;
            est.PiezaV = 0;
            est.SelR = est.SelC = -1;
            est.SnapshotTomado = false;
            est.PuedeDeshacer = true; // se acaba de mover: se puede deshacer una vez

            est.Turno = -est.Turno;

            List<Mov> movs = DamasGame.MovimientosDeLado(est.Tablero, est.Turno, est.ModoSoplar);
            if (movs.Count == 0)
            {
                est.FinPartida = true;
                int ganador = -est.Turno;
                string nombreGanador = ganador == 1 ? est.NombreBlancas : est.NombreNegras;
                est.Aviso = "¡Ganó " + nombreGanador + "!";
                RegistrarFinDamas(est, nombreGanador);
            }
            else
            {
                est.NumMovimientos++;
                est.UltimoMovimiento = "Jugada " + est.NumMovimientos;
                GuardarPartida(est);
            }
        }

        // ----- Regla de soplar -----

        private static void SoplarPiezas(DamasEstado est, List<Casilla> piezas)
        {
            if (piezas == null || piezas.Count == 0) return;

            List<Casilla> reales = piezas.Where(pt => est.Tablero[pt.R, pt.C] != 0).ToList();
            if (reales.Count == 0) return;

            // Aviso indicando la notación de la(s) ficha(s) (MessageBox "SOPLADO")
            string nombres = string.Join(", ", reales.Select(pt => DamasGame.NombreCasilla(pt.R, pt.C)).ToArray());
            est.Aviso = reales.Count == 1
                ? "¡No comiste! Se soplará la ficha que estaba en " + nombres + "."
                : "¡No comiste! Se soplarán las fichas que estaban en " + nombres + ".";

            // Quitar las piezas y marcar en rojo las casillas sopladas
            // (el escritorio las mantiene en rojo mientras se muestra el aviso)
            bool soplada = false;
            foreach (Casilla pt in reales)
            {
                int v = est.Tablero[pt.R, pt.C];
                if (v == 0) continue;

                est.Tablero[pt.R, pt.C] = 0;
                if (v > 0) est.Negras++; else if (v < 0) est.Blancas++;
                soplada = true;
                est.Sopladas.Add(new Casilla(pt.R, pt.C));
            }

            if (!soplada) est.Sopladas.Clear();
        }

        // ----- Persistencia -----

        private static void GuardarSnapshot(DamasEstado est)
        {
            if (est.Tablero == null) return;

            var snap = new FichaSnapshot();
            snap.Tablero = (int[,])est.Tablero.Clone();
            snap.Turno = est.Turno;
            snap.Blancas = est.Blancas;
            snap.Negras = est.Negras;
            snap.FinPartida = est.FinPartida;

            est.Historial.Push(snap);
        }

        private void GuardarPartida(DamasEstado est)
        {
            if (est.FinPartida) return;

            if (est.IdPartida < 0)
            {
                if (string.IsNullOrWhiteSpace(Sesion.NombreUsuario)) return;

                est.IdPartida = PartidaRepository.CrearPartida("damas", est.NombreBlancas, est.NombreNegras,
                    PartidaRepository.ObtenerIdUsuario(Sesion.NombreUsuario), est.ModoSoplar);
                if (est.IdPartida < 0) return;

                PartidaConfig cfgId = PartidaSesion.Actual;
                if (cfgId != null) cfgId.IdPartida = est.IdPartida;
            }

            // turno_final solo admite 'blancas' o 'negras'
            string turnoTexto = DamasGame.TurnoTexto(est.Turno);
            string json = DamasGame.SerializarTablero(est);
            PartidaRepository.GuardarEstado(est.IdPartida, est.NumMovimientos, turnoTexto,
                                            est.UltimoMovimiento, json);

            // Mantener el estado inicial al día para que un refresco de la
            // página no retroceda la partida.
            PartidaConfig cfg = PartidaSesion.Actual;
            if (cfg != null) cfg.EstadoInicial = json;
        }

        private void RegistrarFinDamas(DamasEstado est, string ganadorNombre)
        {
            if (est.IdPartida >= 0)
            {
                PartidaRepository.FinalizarPartida(est.IdPartida, ganadorNombre);
                PartidaRepository.RegistrarResultado(est.NombreBlancas, est.NombreNegras, ganadorNombre);
            }
            est.IdPartida = -1;
            est.NumMovimientos = 0;
            est.UltimoMovimiento = "";

            // La partida ya no se puede continuar desde el menú.
            PartidaSesion.Limpiar();
        }

        // ----- Deshacer -----

        protected void btnDeshacer_Click(object sender, EventArgs e)
        {
            DamasEstado est = Estado;
            if (est == null) return;

            pnlMenu.Visible = false;
            est.Aviso = "";
            est.Sopladas.Clear();

            if (est.Historial.Count == 0)
            {
                est.Aviso = "No hay movimientos para deshacer.";
                Pintar();
                return;
            }

            if (!est.PuedeDeshacer)
            {
                est.Aviso = "Solo puedes deshacer el último movimiento antes de que se juegue la siguiente jugada.";
                Pintar();
                return;
            }

            FichaSnapshot snap = est.Historial.Pop();

            est.Tablero = (int[,])snap.Tablero.Clone();
            est.Turno = snap.Turno;
            est.Blancas = snap.Blancas;
            est.Negras = snap.Negras;
            est.FinPartida = snap.FinPartida;

            est.SecuenciasPendientes = null;
            est.MovimientosSimplesActuales = null;
            est.EnCapturaMultiple = false;
            est.PiezaR = est.PiezaC = -1;
            est.PiezaV = 0;
            est.SelR = est.SelC = -1;
            est.SnapshotTomado = false;
            est.PuedeDeshacer = false;
            est.Marcados.Clear();
            est.UltimoOrigenR = est.UltimoOrigenC = -1;
            est.UltimoDestinoR = est.UltimoDestinoC = -1;

            if (est.NumMovimientos > 0) est.NumMovimientos--;
            GuardarPartida(est);
            Pintar();
        }

        // ----- Navegación -----

        protected void btnIrMenu_Click(object sender, EventArgs e)
        {
            DamasEstado est = Estado;
            if (est == null)
            {
                Response.Redirect("Menu.aspx");
                return;
            }

            // Partida terminada (o sin partida en la nube): ir directamente.
            if (est.FinPartida || est.IdPartida < 0)
            {
                Session.Remove(CLAVE_ESTADO);
                Response.Redirect("Menu.aspx");
                return;
            }

            // Preguntar si finalizarla o continuarla más tarde
            // (MessageBox "Partida en curso" del escritorio).
            litMenuTexto.Text =
                "¿Quieres finalizar la partida o continuarla más tarde?\n\n" +
                "Sí = Finalizar la partida (no se podrá continuar)\n" +
                "No = Continuar más tarde (se guarda la posición)";
            pnlMenu.Visible = true;
        }

        protected void btnMenuFinalizar_Click(object sender, EventArgs e)
        {
            DamasEstado est = Estado;
            if (est == null)
            {
                Response.Redirect("Menu.aspx");
                return;
            }

            // El resultado se decide por las piezas comidas (igual que el escritorio)
            string ganador = DamasGame.GanadorPorPiezasComidas(est);
            PartidaRepository.FinalizarPartida(est.IdPartida, ganador);
            PartidaRepository.RegistrarResultado(est.NombreBlancas, est.NombreNegras, ganador);
            est.IdPartida = -1;
            est.NumMovimientos = 0;
            est.UltimoMovimiento = "";
            est.FinPartida = true;

            Session.Remove(CLAVE_ESTADO);
            Response.Redirect("Menu.aspx");
        }

        protected void btnMenuContinuar_Click(object sender, EventArgs e)
        {
            DamasEstado est = Estado;
            if (est != null) GuardarPartida(est); // Continuar más tarde: se guarda el estado actual

            Session.Remove(CLAVE_ESTADO);
            Response.Redirect("Menu.aspx");
        }
    }
}
