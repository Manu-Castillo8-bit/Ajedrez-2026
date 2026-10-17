using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class Partidas : Form
    {
        // Fila que se muestra en el DataGridView
        private class FilaPartida
        {
            public string Tipo { get; set; }
            public string Jugador { get; set; }
            public string Rival { get; set; }
            public string Resultado { get; set; }
            public string Estado { get; set; }
            public string Fecha { get; set; }
        }

        private enum Filtro { Todas, Ganadas, Perdidas, Empates, Fecha }
        private Filtro filtroActual = Filtro.Todas;
        private List<PartidaRepository.Partida> partidas = new List<PartidaRepository.Partida>();

        public Partidas()
        {
            InitializeComponent();
            Tema.ApplyTheme(this);
        }

        private void Partidas_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;

            CargarPartidas();
        }

        private void ir_menu_Click(object sender, EventArgs e)
        {
            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }

        // Sincroniza los datos entre Supabase y SQLite y vuelve a cargar la lista.
        private async void actualizar_Click(object sender, EventArgs e)
        {
            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                CargarPartidas();
                return;
            }

            actualizar.Enabled = false;
            string textoOriginal = actualizar.Text;
            actualizar.Text = "Actualizando...";

            try
            {
                await Task.Run(() => Sincronizador.Sincronizar(usuario));
            }
            catch
            {
                // Si falla la red, igualmente se muestra lo que haya en local.
            }

            CargarPartidas();

            actualizar.Text = textoOriginal;
            actualizar.Enabled = true;
        }

        // ---------------- Filtros (botones) ----------------
        private void button1_Click(object sender, EventArgs e) // TODAS
        {
            filtroActual = Filtro.Todas;
            AplicarFiltro();
        }

        private void button2_Click(object sender, EventArgs e) // VICTORIAS
        {
            filtroActual = Filtro.Ganadas;
            AplicarFiltro();
        }

        private void button3_Click(object sender, EventArgs e) // DERROTAS
        {
            filtroActual = Filtro.Perdidas;
            AplicarFiltro();
        }

        private void button4_Click(object sender, EventArgs e) // EMPATES
        {
            filtroActual = Filtro.Empates;
            AplicarFiltro();
        }

        private void button5_Click(object sender, EventArgs e) // FECHA
        {
            filtroActual = Filtro.Fecha;
            AplicarFiltro();
        }

        private void tipo_CheckedChanged(object sender, EventArgs e)
        {
            // Recargar según el tipo (damas / ajedrez) seleccionado
            CargarPartidas();
        }

        // ---------------- Carga y estadísticas ----------------
        private string TipoSeleccionado()
        {
            if (damas.Checked) return "damas";
            if (ajedrez.Checked) return "ajedrez";
            return null;
        }

        private void CargarPartidas()
        {
            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                dataGridView1.DataSource = new List<FilaPartida>();
                MostrarEstadisticas(new List<PartidaRepository.Partida>());
                return;
            }

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
                Tipo = p.tipo_juego,
                Jugador = usuario,
                Rival = string.Equals(p.jugador_blancas, usuario, StringComparison.OrdinalIgnoreCase)
                        ? p.jugador_negras : p.jugador_blancas,
                Resultado = ResultadoTexto(p, usuario),
                Estado = p.estado,
                Fecha = p.fecha_inicio.HasValue
                        ? p.fecha_inicio.Value.ToString("dd/MM/yyyy HH:mm") : ""
            }).ToList();

            dataGridView1.DataSource = filas;

            MostrarEstadisticas(partidas);
        }

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

            groupBox1.Text = "ESTADISTICAS  (WINRATE " + winrate + "%)";
        }

        private void m_claro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(false);      // Cambia a tema claro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
        }
    }
}
