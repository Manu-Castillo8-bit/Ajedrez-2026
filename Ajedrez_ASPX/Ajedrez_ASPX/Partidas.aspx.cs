using System;
using System.Collections.Generic;
using System.Linq;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Historial de partidas del usuario (equivalente a la ventana PARTIDAS
    /// del escritorio): filtros, estadísticas con winrate y movimientos.
    /// </summary>
    public partial class Partidas : System.Web.UI.Page
    {
        // Fila que se muestra en el GridView (igual que FilaPartida del escritorio).
        private class FilaPartida
        {
            public int Id { get; set; }
            public string Tipo { get; set; }
            public string Jugador { get; set; }
            public string Rival { get; set; }
            public string Resultado { get; set; }
            public string Estado { get; set; }
            public string Fecha { get; set; }
        }

        private enum Filtro { Todas, Ganadas, Perdidas, Empates, Fecha }

        private Filtro filtroActual
        {
            get
            {
                object v = ViewState["Filtro"];
                return v is Filtro ? (Filtro)v : Filtro.Todas;
            }
            set { ViewState["Filtro"] = value; }
        }

        // Partida cuyos movimientos se están mostrando (0 = panel cerrado).
        private int IdPartidaSeleccionada
        {
            get { return ViewState["IdPartidaSeleccionada"] is int ? (int)ViewState["IdPartidaSeleccionada"] : 0; }
            set { ViewState["IdPartidaSeleccionada"] = value; }
        }

        private bool MovimientosAbiertos
        {
            get { return ViewState["MovimientosAbiertos"] is bool && (bool)ViewState["MovimientosAbiertos"]; }
            set { ViewState["MovimientosAbiertos"] = value; }
        }

        private List<PartidaRepository.Partida> partidas = new List<PartidaRepository.Partida>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "PARTIDAS";

            CargarPartidas();

            // El panel de movimientos se conserva entre peticiones (ViewState).
            if (MovimientosAbiertos && IdPartidaSeleccionada > 0)
            {
                CargarMovimientos(IdPartidaSeleccionada);
                pnlMovimientos.Visible = true;
            }
            else
            {
                pnlMovimientos.Visible = false;
            }
        }

        // ---------------- Navegación ----------------

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        // ---------------- Carga y filtros ----------------

        private string TipoSeleccionado()
        {
            if (rbDamas.Checked) return "damas";
            if (rbAjedrez.Checked) return "ajedrez";
            return null;
        }

        private void CargarPartidas()
        {
            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                partidas = new List<PartidaRepository.Partida>();
                gvPartidas.DataSource = new List<FilaPartida>();
                gvPartidas.DataBind();
                MostrarEstadisticas(partidas);
                return;
            }

            // En el escritorio este botón sincroniza Supabase y la base local;
            // en la web solo se recarga desde Supabase (no hay base local).
            partidas = PartidaRepository.ListarPartidas(TipoSeleccionado(), usuario);
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            string usuario = Sesion.NombreUsuario;
            IEnumerable<PartidaRepository.Partida> q = partidas;

            switch (filtroActual)
            {
                case Filtro.Ganadas:
                    q = q.Where(p => EsVictoria(p, usuario));
                    break;
                case Filtro.Perdidas:
                    q = q.Where(p => EsDerrota(p, usuario));
                    break;
                case Filtro.Empates:
                    q = q.Where(p => EsEmpate(p, usuario));
                    break;
                case Filtro.Fecha:
                    q = q.OrderByDescending(p => p.fecha_inicio);
                    break;
            }

            List<FilaPartida> filas = q.Select(p => new FilaPartida
            {
                Id = p.id_partida,
                Tipo = p.tipo_juego,
                Jugador = usuario,
                Rival = string.Equals(p.jugador_blancas, usuario, StringComparison.OrdinalIgnoreCase)
                        ? p.jugador_negras : p.jugador_blancas,
                Resultado = ResultadoTexto(p, usuario),
                Estado = p.estado,
                Fecha = p.fecha_inicio.HasValue
                        ? p.fecha_inicio.Value.ToString("dd/MM/yyyy HH:mm") : ""
            }).ToList();

            // La selección anterior puede quedar fuera de rango al cambiar el filtro.
            if (gvPartidas.SelectedIndex >= filas.Count)
                gvPartidas.SelectedIndex = -1;

            gvPartidas.DataSource = filas;
            gvPartidas.DataBind();

            MostrarEstadisticas(partidas);
        }

        // Sincroniza/recarga la lista (equivale al botón ACTUALIZAR del escritorio).
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string textoOriginal = btnActualizar.Text;
            btnActualizar.Enabled = false;
            btnActualizar.Text = "Actualizando...";

            try
            {
                CargarPartidas();
            }
            catch
            {
                // Si falla la red, igualmente se muestra lo que haya cargado.
            }

            btnActualizar.Text = textoOriginal;
            btnActualizar.Enabled = true;
        }

        // ---------------- Filtros (botones) ----------------

        protected void btnTodas_Click(object sender, EventArgs e)
        {
            filtroActual = Filtro.Todas;
            AplicarFiltro();
        }

        protected void btnGanadas_Click(object sender, EventArgs e)
        {
            filtroActual = Filtro.Ganadas;
            AplicarFiltro();
        }

        protected void btnPerdidas_Click(object sender, EventArgs e)
        {
            filtroActual = Filtro.Perdidas;
            AplicarFiltro();
        }

        protected void btnTablas_Click(object sender, EventArgs e)
        {
            filtroActual = Filtro.Empates;
            AplicarFiltro();
        }

        protected void btnFecha_Click(object sender, EventArgs e)
        {
            filtroActual = Filtro.Fecha;
            AplicarFiltro();
        }

        // Al cambiar el tipo (damas / ajedrez) se vuelve a cargar la lista:
        // el postback llega a Page_Load, que ya llama a CargarPartidas()
        // (equivalente a tipo_CheckedChanged del escritorio).

        // ---------------- Resultados ----------------

        private static bool EsFinalizada(PartidaRepository.Partida p)
        {
            return string.Equals(p.estado, "finalizada", StringComparison.OrdinalIgnoreCase);
        }

        // Partida terminada y ganada por el usuario
        private static bool EsVictoria(PartidaRepository.Partida p, string usuario)
        {
            return EsFinalizada(p)
                   && !string.IsNullOrWhiteSpace(p.ganador)
                   && string.Equals(p.ganador, usuario, StringComparison.OrdinalIgnoreCase);
        }

        // Partida terminada y con un ganador distinto del usuario (no tablas)
        private static bool EsDerrota(PartidaRepository.Partida p, string usuario)
        {
            return EsFinalizada(p)
                   && !string.IsNullOrWhiteSpace(p.ganador)
                   && !string.Equals(p.ganador, "empate", StringComparison.OrdinalIgnoreCase)
                   && !string.Equals(p.ganador, usuario, StringComparison.OrdinalIgnoreCase);
        }

        // Todo lo demás: tablas, abandonos sin ganador y partidas sin terminar / en pausa
        private static bool EsEmpate(PartidaRepository.Partida p, string usuario)
        {
            return !EsVictoria(p, usuario) && !EsDerrota(p, usuario);
        }

        private static string ResultadoTexto(PartidaRepository.Partida p, string usuario)
        {
            if (EsVictoria(p, usuario)) return "Victoria";
            if (EsDerrota(p, usuario)) return "Derrota";
            return "Empate";
        }

        private void MostrarEstadisticas(List<PartidaRepository.Partida> lista)
        {
            string usuario = Sesion.NombreUsuario;

            // Solo se cuentan las partidas finalizadas (se descartan las que siguen en curso)
            int nVictorias = lista.Count(p => EsVictoria(p, usuario));
            int nDerrotas = lista.Count(p => EsDerrota(p, usuario));
            int nEmpates = lista.Count(p => EsFinalizada(p) && EsEmpate(p, usuario));
            int nTotal = lista.Count(p => EsFinalizada(p));

            // Winrate = victorias sobre las partidas decididas (ganadas + perdidas)
            int nDecididas = nVictorias + nDerrotas;
            int winrate = nDecididas > 0 ? (int)Math.Round(100.0 * nVictorias / nDecididas) : 0;

            // Fila 1: ganadas
            ganadas_.Text = "GANADAS:";
            victorias_.Text = nVictorias.ToString();

            perdidas_.Text = "PERDIDAS:";
            derrotas.Text = nDerrotas.ToString();

            empates_.Text = "EMPATES:";
            empates.Text = nEmpates.ToString();

            todas_.Text = "TOTAL:";
            total.Text = nTotal.ToString();

            lblEstadisticas.Text = "ESTADISTICAS  (WINRATE " + winrate + "%)";
        }

        // ---------------- Movimientos ----------------

        // Al seleccionar una partida se abren sus movimientos (como la ventana
        // MOVIMIENTOS del escritorio).
        protected void gvPartidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvPartidas.SelectedIndex < 0 || gvPartidas.SelectedDataKey == null)
                return;

            object valor = gvPartidas.SelectedDataKey.Value;
            if (valor == null) return;

            IdPartidaSeleccionada = Convert.ToInt32(valor);
            MovimientosAbiertos = true;
            CargarMovimientos(IdPartidaSeleccionada);
            pnlMovimientos.Visible = true;
        }

        protected void btnCerrarMov_Click(object sender, EventArgs e)
        {
            MovimientosAbiertos = false;
            IdPartidaSeleccionada = 0;
            gvPartidas.SelectedIndex = -1;
            pnlMovimientos.Visible = false;
            AplicarFiltro(); // vuelve a dibujar la lista sin la selección
        }

        private void CargarMovimientos(int idPartida)
        {
            List<PartidaRepository.Movimiento> movs = Nube.MovimientosDePartidas(new List<int> { idPartida });
            movs = movs.OrderBy(m => m.num_movimientos).ToList();
            gvMovimientos.DataSource = movs;
            gvMovimientos.DataBind();
        }
    }
}
