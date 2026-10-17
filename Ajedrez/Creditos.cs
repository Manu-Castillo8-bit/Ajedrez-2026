using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class Creditos : Form
    {
        public Creditos()
        {
            InitializeComponent();

            // Aplicar el tema actual (el tema reajusta las fuentes de las etiquetas,
            // por eso las volvemos a fijar después).
            Tema.ApplyTheme(this);

            lblTitulo.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold);
            lblCuerpo.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
