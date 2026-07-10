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
    public partial class LOGIN : Form
    {
        public LOGIN()
        {
            InitializeComponent();
           
        }

        private void LOGIN_Load(object sender, EventArgs e)
        {
          
        }

        private void nombre_TextChanged(object sender, EventArgs e)
        {
            //NO SIRVE

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

        private void continuar_Click(object sender, EventArgs e)
        {
            
        }

        private void ir_registro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            REGISTRADOR f_registro = new REGISTRADOR();
            f_registro.Show();
            this.Hide();
        }
    }
}
