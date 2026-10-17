using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class NombresPartida : Form
    {
        public string NombreLocal { get; private set; }
        public string NombreOponente { get; private set; }
        public bool ModoSoplar { get; private set; }
        public bool LocalEsBlancas { get; private set; } = true;
        public string TipoJuego { get; private set; } = "damas";
        public bool ModoIA { get; private set; }
        public int NivelIA { get; private set; } = 5;

        public NombresPartida() : this("damas")
        {
        }

        public NombresPartida(string tipoJuego)
        {
            InitializeComponent();
            Tema.ApplyTheme(this);

            TipoJuego = string.IsNullOrWhiteSpace(tipoJuego) ? "damas" : tipoJuego;
            Text = "NUEVA PARTIDA - " + TipoJuego.ToUpper();

            txtLocal.Text = string.IsNullOrWhiteSpace(Sesion.NombreUsuario) ? "Jugador" : Sesion.NombreUsuario;
            txtLocal.SelectAll();

            cmbNivel.SelectedIndex = 1;

            // El modo de captura (soplar) solo aplica a las damas
            if (TipoJuego == "ajedrez")
            {
                lblModo.Visible = false;
                rbNormal.Visible = false;
                rbSoplar.Visible = false;

                gbModo.Visible = true;
                gbModo.Location = new Point(24, 162);

                btnIniciar.Location = new Point(125, 248);
                btnCancelar.Location = new Point(235, 248);
                ClientSize = new Size(384, 292);
            }
            else
            {
                gbModo.Visible = false;
                rbLocal.Checked = true;
            }

            ActualizarNivel();
        }

        private void rbLocal_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarNivel();
        }

        private void rbIA_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarNivel();
        }

        private void ActualizarNivel()
        {
            bool contraIA = rbIA.Checked;
            lblNivel.Enabled = contraIA;
            cmbNivel.Enabled = contraIA;

            if (contraIA)
            {
                txtOponente.Text = "Stockfish";
                txtOponente.ReadOnly = true;
            }
            else
            {
                if (txtOponente.Text == "Stockfish")
                    txtOponente.Text = "";
                txtOponente.ReadOnly = false;
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            string local = txtLocal.Text.Trim();
            string visitante = txtOponente.Text.Trim();

            if (local.Length == 0)
            {
                MessageBox.Show("Ingresa el nombre del jugador.", TipoJuego.ToUpper(),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocal.Focus();
                return;
            }

            if (visitante.Length == 0)
            {
                MessageBox.Show("Ingresa el nombre del oponente.", TipoJuego.ToUpper(),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOponente.Focus();
                return;
            }

            NombreLocal = local;
            NombreOponente = visitante;
            ModoSoplar = rbSoplar.Checked;
            LocalEsBlancas = rbBlancas.Checked;

            if (TipoJuego == "ajedrez" && rbIA.Checked)
            {
                ModoIA = true;
                NivelIA = NivelDesdeIndice(cmbNivel.SelectedIndex);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private static int NivelDesdeIndice(int indice)
        {
            switch (indice)
            {
                case 0: return 1;   // Fácil
                case 2: return 12;  // Difícil
                case 3: return 20;  // Experto
                default: return 5;  // Media
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
