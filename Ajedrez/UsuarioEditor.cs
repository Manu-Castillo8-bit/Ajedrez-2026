using System;
using System.Windows.Forms;

namespace Ajedrez
{
    /// <summary>
    /// Diálogo para crear o editar un usuario desde el panel administrativo.
    /// Devuelve en <see cref="Usuario"/> los datos introducidos.
    /// </summary>
    public partial class UsuarioEditor : Form
    {
        /// <summary>Usuario con los datos del formulario (solo válido si el resultado es OK).</summary>
        public UsuarioCompleto Usuario { get; private set; }

        private readonly bool _esNuevo;

        public UsuarioEditor(UsuarioCompleto existente)
        {
            InitializeComponent();

            _esNuevo = (existente == null);
            Usuario = existente ?? new UsuarioCompleto { activo = true, elo = 800 };
            this.Text = _esNuevo ? "Nuevo usuario" : "Editar usuario";

            Cargar();
            Tema.ApplyTheme(this);
        }

        private void Cargar()
        {
            txtNombre.Text = Usuario.nombre_usuario ?? "";
            txtEmail.Text = Usuario.email ?? "";
            txtContrasena.Text = Usuario.contrasena ?? "";
            txtElo.Text = Usuario.elo.ToString("0");
            txtJugadas.Text = Usuario.partidas_jugadas.ToString();
            txtGanadas.Text = Usuario.partidas_ganadas.ToString();
            txtEmpatadas.Text = Usuario.partidas_empatadas.ToString();
            txtPerdidas.Text = Usuario.partidas_perdidas.ToString();
            chkActivo.Checked = Usuario.activo;
        }

        private void chkVer_CheckedChanged(object sender, EventArgs e)
        {
            txtContrasena.UseSystemPasswordChar = !chkVer.Checked;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (_esNuevo && string.IsNullOrEmpty(txtContrasena.Text))
            {
                MessageBox.Show("La contraseña es obligatoria para un usuario nuevo.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.Focus();
                return;
            }

            Usuario.nombre_usuario = txtNombre.Text.Trim();
            Usuario.email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            Usuario.contrasena = txtContrasena.Text;
            Usuario.elo = Parsear(txtElo.Text, Usuario.elo);
            Usuario.partidas_jugadas = (int)Parsear(txtJugadas.Text, Usuario.partidas_jugadas);
            Usuario.partidas_ganadas = (int)Parsear(txtGanadas.Text, Usuario.partidas_ganadas);
            Usuario.partidas_empatadas = (int)Parsear(txtEmpatadas.Text, Usuario.partidas_empatadas);
            Usuario.partidas_perdidas = (int)Parsear(txtPerdidas.Text, Usuario.partidas_perdidas);
            Usuario.activo = chkActivo.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private static double Parsear(string texto, double porDefecto)
        {
            double v;
            if (double.TryParse(texto, out v)) return v;
            return porDefecto;
        }
    }
}
