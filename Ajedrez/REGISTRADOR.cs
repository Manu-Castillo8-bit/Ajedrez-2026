using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Ajedrez
{
    public partial class REGISTRADOR : Form
    {
        public REGISTRADOR()
        {
            InitializeComponent();
          
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

        private void ir_login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LOGIN f_login = new LOGIN();
            f_login.Show();
            this.Hide();
        }
    }
}
