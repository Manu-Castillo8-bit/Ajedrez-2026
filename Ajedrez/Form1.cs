using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int n;
        PictureBox[,] P;

        string color = "r", k = "", B1 = "", B2 = "", k2="";

        private void partidas_Click(object sender, EventArgs e)
        {

        }

        private void m_claro_Click(object sender, EventArgs e)
        {

        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {

        }

        private void ir_menu_Click(object sender, EventArgs e)
        {
            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }
        

        int blancas =0, negras=0;
        private void Form1_Load(object sender, EventArgs e)
        {
            n = 8;
            P = new PictureBox[n, n];
            int left = 2, top = 2;
            Color[] colors = new Color[] { Color.White, Color.Black };

            for (int i = 0; i < n; i++)
            {
                left = 2;


                if (i % 2 == 0)
                {
                    colors[0] = Color.Black;  // Invertido
                    colors[1] = Color.White;  // Invertido
                }
                else
                {
                    colors[0] = Color.White;  // Invertido
                    colors[1] = Color.Black;  // Invertido
                }


                for (int j = 0; j < n; j++)
                {
                    P[i, j] = new PictureBox();
                    P[i, j].BackColor = colors[(j % 2 == 0) ? 0 : 1];
                    P[i, j].Location = new Point(left, top);
                    P[i, j].Size = new Size(60, 60);
                    left += 60;
                    P[i, j].Name = i + " " + j;

                    if (i < (n / 2) - 1 && P[i, j].BackColor == Color.Black)
                    { P[i, j].Image = Properties.Resources.peon; P[i, j].Name += " r"; }

                    else if (i > (n / 2) && P[i, j].BackColor == Color.Black)
                    { P[i, j].Image = Properties.Resources.peon_negro_pixel; P[i, j].Name += " g"; }
                    P[i, j].SizeMode = PictureBoxSizeMode.Zoom;
                    P[i, j].MouseHover += (sender2, e2) =>
                    {
                        PictureBox p = sender2 as PictureBox;
                        if (p.Image != null) p.BackColor = Color.FromArgb(255, 64, 64, 64);
                    };

                    P[i, j].MouseLeave += (sender2, e2) =>
                    {
                        PictureBox p = sender2 as PictureBox;
                        if (p.Image != null) p.BackColor = Color.Black;
                    };

                    P[i, j].Click += (sender3, e3) =>
                    {
                        PictureBox p = sender3 as PictureBox;
                        if (p.Image != null)
                        {
                            int c = -1, x, y;

                            F();
                            if (p.Name.Split(' ')[2] == "b")
                            {
                                if (color == "r") color = "g";
                                else color = "r";

                                x = Convert.ToInt32(k.Split(' ')[0]);
                                y = Convert.ToInt32(k.Split(' ')[1]);

                                B1 = "";
                                B2 = "";

                                if (k.Split(' ')[2] == "r")
                                {
                                    p.Image = Properties.Resources.peon; //r = peon_blanco
                                    p.Name = p.Name.Replace("b", "r");
                                }
                                else
                                    if (k.Split(' ')[2] == "g")
                                    {
                                        p.Image = Properties.Resources.peon_negro_pixel; //g = peon_negro
                                        p.Name = p.Name.Replace("b", "g");
                                    }

                                P[x, y].Image = null;

                                if (k2 != "")
                                {
                                    x = Convert.ToInt32(k2.Split(' ')[0]);
                                    y = Convert.ToInt32(k2.Split(' ')[1]);

                                    P[x, y].Image = null;

                                    if (k2.Split(' ')[2] == "r") blancas++;
                                    else negras++;

                                    p_blancas.Text = negras + "";
                                    p_negras.Text = blancas + "";

                                    k2 = "";
                                }
                            }


                            else


                            if (p.Name.Split(' ')[2] == color) //Minuto 8:41
                            {
                                x = Convert.ToInt32(p.Name.Split(' ')[0]);
                                y = Convert.ToInt32(p.Name.Split(' ')[1]);
                                k = p.Name;


                                if (p.Name.Split(' ')[2] == "r") c = 1;

                                try
                                {
                                    if (P[x + c, y + 1].Image == null)
                                    {
                                        P[x + c, y + 1].Image = Properties.Resources.Movimiento; //b min 9:49
                                        P[x + c, y + 1].Name = (x + c) + " " + (y + 1) + " b";
                                        B1 = (x + c) + " " + (y + 1);
                                    }

                                    else
                                        if (P[x + c, y + 1].Name.Split(' ')[2] != p.Name.Split(' ')[2] && P[x + (c * 2), y + 2].Image == null)
                                        {
                                            P[x + (c * 2), y + 2].Image = Properties.Resources.Movimiento;
                                            P[x + (c * 2), y + 2].Name = (x + (c * 2)) + " " + (y + 2) + " b";
                                            B1 = (x + (c * 2)) + " " + (y + 2);
                                            k2 = (x + c) + " " + (y + 1) + " " + P[x + c, y + 1].Name.Split(' ')[2];
                                        }
                                }
                                catch { }

                                try
                                {
                                    if (P[x + c, y - 1].Image == null)
                                    {
                                        P[x + c, y - 1].Image = Properties.Resources.Movimiento;
                                        P[x + c, y - 1].Name = (x + c) + " " + (y - 1) + " b";
                                        B2 = (x + c) + " " + (y - 1);
                                    }

                                    else
                                        if (P[x + c, y - 1].Name.Split(' ')[2] != p.Name.Split(' ')[2] &&
                                            P[x + (c * 2), y - 2].Image == null)
                                        {

                                            P[x + (c * 2), y - 2].Image = Properties.Resources.Movimiento;


                                            P[x + (c * 2), y - 2].Name = (x + (c * 2)) + " " + (y - 2) + " b";


                                            B2 = (x + (c * 2)) + " " + (y - 2);


                                            k2 = (x + c) + " " + (y - 1) + " " + P[x + c, y - 1].Name.Split(' ')[2];
                                        }

                                }
                                catch { }

                            }
                        }
                    };

                        G.Controls.Add(P[i, j]);
                    }
                top += 60;
                }
            }

        public void F()
        {
            if (B1 != "")
            {
                int x, y;
                x = Convert.ToInt32(B1.Split(' ')[0]);
                y = Convert.ToInt32(B1.Split(' ')[1]);
                P[x, y].Image = null;
            }

            if (B2 != "")
            {
                int x, y;
                x = Convert.ToInt32(B2.Split(' ')[0]);
                y = Convert.ToInt32(B2.Split(' ')[1]);
                P[x, y].Image = null;
            }
        }


    }
    }




