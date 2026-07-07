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
            this.BackColor = SystemColors.Control;
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            checkBox1.ForeColor = Color.Black;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            pictureBox5.Visible = false;
            pictureBox4.Visible = false;
            pictureBox6.Visible = false;
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
          this.BackColor = Color.FromArgb(30, 30, 30);
            label1.ForeColor = SystemColors.Control;
            label2.ForeColor = SystemColors.Control;
            label3.ForeColor = SystemColors.Control;
            label4.ForeColor = SystemColors.Control;
            checkBox1.ForeColor = Color.White;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox5.Visible = true;
            pictureBox4.Visible = true;
            pictureBox6.Visible = true;
        }

        private void ir_login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LOGIN f_login = new LOGIN();
            f_login.Show();
            this.Hide();
        }
    }
}
