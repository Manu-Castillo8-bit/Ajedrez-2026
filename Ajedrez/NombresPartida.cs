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

            // El modo de captura (soplar) solo aplica a las damas
            if (TipoJuego == "ajedrez")
            {
                lblModo.Visible = false;
                rbNormal.Visible = false;
                rbSoplar.Visible = false;
                btnIniciar.Location = new Point(150, 170);
                btnCancelar.Location = new Point(255, 170);
                ClientSize = new Size(384, 216);
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
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
