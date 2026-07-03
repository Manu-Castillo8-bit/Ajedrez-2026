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
        }

        private void ir_menu_Click(object sender, EventArgs e)
        {
            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }

        private void m_claro_Click(object sender, EventArgs e)
        {

        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {

        }
    }
}
