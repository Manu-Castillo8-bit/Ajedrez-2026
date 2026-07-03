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
    }
    }

