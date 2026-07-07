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
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        private void ajedrez_Click(object sender, EventArgs e)
        {
            // Mostrar el nuevo formulario
            Ajedrez_tablero f_ajedrez = new Ajedrez_tablero();

            f_ajedrez.Show();

            // Opcional: ocultar el formulario actual
            this.Hide();
        }

        private void ir_damas_Click(object sender, EventArgs e)
        {
            Form1 f_damas = new Form1();
            f_damas.Show();
            this.Hide();
        }

        private void ir_perfil_Click(object sender, EventArgs e)
        {
            Perfil f_perfil = new Perfil();
            f_perfil.Show();
            this.Hide();
        }

        private void ir_movimientos_Click(object sender, EventArgs e)
        {
            Partidas f_partidas = new Partidas();
                f_partidas.Show();
            this.Hide();
        }

        private void ir_login_Click(object sender, EventArgs e)
        {
            LOGIN f_login = new LOGIN();
            f_login.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
          //NO SIRVE
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //NO SIRVE

        }

        private void label5_Click(object sender, EventArgs e)
        {
            //NO SIRVE

        }

        private void m_claro_Click(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.Control;
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
          
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            label1.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            label3.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            label5.ForeColor = Color.White;
        }
    }
    }

