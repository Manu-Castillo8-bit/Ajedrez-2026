using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class Perfil : Form
    {
        public Perfil()
        {
            InitializeComponent();
            Tema.ApplyTheme(this);
        }

        private void ir_menu_Click(object sender, EventArgs e)
        {
            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }

        private void m_claro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(false);      // Cambia a tema claro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual  
        }

        private void Perfil_Load(object sender, EventArgs e)
        {
            picture_victorias.Visible = true;
            picture_elo.Visible = true;
            picture_derrotas.Visible = true;
            picture_partidas.Visible = true;

        }
    }
}
