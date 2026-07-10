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

            Tema.ApplyTheme(this);
        }



        private void ajedrez_Click(object sender, EventArgs e)
        {
            // Mostrar el nuevo formulario
            Ajedrez_tablero f_ajedrez = new Ajedrez_tablero();
            Tema.ApplyTheme(f_ajedrez);
            f_ajedrez.Show();

            // Opcional: ocultar el formulario actual
            this.Hide();
        }

        private void ir_damas_Click(object sender, EventArgs e)
        {
            Form1 f_damas = new Form1();
            Tema.ApplyTheme(f_damas);
            f_damas.Show();
            this.Hide();
        }

        private void ir_perfil_Click(object sender, EventArgs e)
        {
            Perfil f_perfil = new Perfil();
            Tema.ApplyTheme(f_perfil);
            f_perfil.Show();
            this.Hide();
        }

        private void ir_movimientos_Click(object sender, EventArgs e)
        {
            Partidas f_partidas = new Partidas();
            Tema.ApplyTheme(f_partidas);
            f_partidas.Show();
            this.Hide();
        }

        private void ir_login_Click(object sender, EventArgs e)
        {
            LOGIN f_login = new LOGIN();
            Tema.ApplyTheme(f_login);
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
            Tema.SetTheme(false);      // Cambia a tema claro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
            /*
            //FONDO
            this.BackColor = SystemColors.Control;

            //LABELS
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;

            //NEGRITA DE LOS BOTONES Y COLOR DE FUENTE
            ajedrez.Font = new Font(ajedrez.Font, FontStyle.Regular);
            ajedrez.ForeColor = Color.Black;

            ir_damas.Font = new Font(ajedrez.Font, FontStyle.Regular);
            ir_damas.ForeColor = Color.Black;

            ir_perfil.Font = new Font(ajedrez.Font, FontStyle.Regular);
            ir_perfil.ForeColor = Color.Black;

            ir_partidas.Font = new Font(ajedrez.Font, FontStyle.Regular);
            ir_partidas.ForeColor = Color.Black;

            ir_login.Font = new Font(ajedrez.Font, FontStyle.Regular);
            ir_login.ForeColor = Color.Black;

            //COLORES DE LOS BOTONES Y GROUPBOX
            groupBox1.BackColor = SystemColors.ControlLight;
            ajedrez.BackColor = ColorTranslator.FromHtml("#F0F6FF");
            ir_damas.BackColor = ColorTranslator.FromHtml("#DCE4F0");
            ir_perfil.BackColor = ColorTranslator.FromHtml("#DCE4F0");
            ir_partidas.BackColor = ColorTranslator.FromHtml("#F0F6FF");
            ir_login.BackColor = ColorTranslator.FromHtml("#DCE4F0");
            m_oscuro.BackColor = ColorTranslator.FromHtml("#4a8af4");

            */
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
            /*
            //FONDO
            this.BackColor = ColorTranslator.FromHtml("#0a1628");

            //LABELS
            label1.ForeColor = Color.White;
            label2.ForeColor = Color.White; 
            label3.ForeColor = ColorTranslator.FromHtml(" #9ab8e8");
            label4.ForeColor = ColorTranslator.FromHtml(" #9ab8e8");
            label5.ForeColor = ColorTranslator.FromHtml(" #9ab8e8");

            //NEGRITA DE LOS BOTONES Y COLOR DE FUENTE
            ajedrez.Font = new Font(ajedrez.Font, FontStyle.Bold);
            ajedrez.ForeColor = Color.White;

            ir_damas.Font = new Font(ajedrez.Font, FontStyle.Bold);
            ir_damas.ForeColor = Color.White;

            ir_perfil.Font = new Font(ajedrez.Font, FontStyle.Bold);
            ir_perfil.ForeColor = Color.White;

            ir_partidas.Font = new Font(ajedrez.Font, FontStyle.Bold);
            ir_partidas.ForeColor = Color.White;

            ir_login.Font = new Font(ajedrez.Font, FontStyle.Bold);
            ir_login.ForeColor = Color.White;


            //COLORES DE LOS BOTONES Y GROUPBOX
            groupBox1.BackColor = ColorTranslator.FromHtml(" #1a2d4a ");
            ajedrez.BackColor = ColorTranslator.FromHtml("#22b8f0");
            ir_damas.BackColor = ColorTranslator.FromHtml("#4a8af4");
            ir_perfil.BackColor = ColorTranslator.FromHtml("#4a8af4");
            ir_partidas.BackColor = ColorTranslator.FromHtml("#22b8f0");
            ir_login.BackColor = ColorTranslator.FromHtml("#4a8af4");
            m_oscuro.BackColor = ColorTranslator.FromHtml("#4a8af4");
            */

        }

        private void menu_Load(object sender, EventArgs e)
        {

        }
    }
    }

