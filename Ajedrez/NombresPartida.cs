using System;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class NombresPartida : Form
    {
        public string NombreLocal { get; private set; }
        public string NombreOponente { get; private set; }

        public NombresPartida()
        {
            InitializeComponent();
            Tema.ApplyTheme(this);

            txtLocal.Text = string.IsNullOrWhiteSpace(Sesion.NombreUsuario) ? "Jugador" : Sesion.NombreUsuario;
            txtLocal.SelectAll();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            string local = txtLocal.Text.Trim();
            string visitante = txtOponente.Text.Trim();

            if (local.Length == 0)
            {
                MessageBox.Show("Ingresa el nombre del jugador.", "DAMAS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocal.Focus();
                return;
            }

            if (visitante.Length == 0)
            {
                MessageBox.Show("Ingresa el nombre del oponente.", "DAMAS",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOponente.Focus();
                return;
            }

            NombreLocal = local;
            NombreOponente = visitante;
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
