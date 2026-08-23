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
            Tema.SetTheme(false);      // Cambia a tema claro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
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

                            // 1. GUARDAMOS EL NOMBRE ANTES DE QUE F() LO LIMPIE
                            string nombreClic = p.Name;

                            F();

                            // 2. USAMOS nombreClic EN LUGAR DE p.Name PARA EVALUAR
                            if (nombreClic.Split(' ').Length > 2 && nombreClic.Split(' ')[2] == "b")
                            {
                                if (color == "r") color = "g";
                                else color = "r";

                                int origX = Convert.ToInt32(k.Split(' ')[0]);
                                int origY = Convert.ToInt32(k.Split(' ')[1]);
                                int destX = Convert.ToInt32(nombreClic.Split(' ')[0]);
                                int destY = Convert.ToInt32(nombreClic.Split(' ')[1]);

                                B1 = "";
                                B2 = "";

                                if (k.Split(' ')[2] == "r")
                                {
                                    p.Image = Properties.Resources.peon;
                                    p.Name = destX + " " + destY + " r";
                                }
                                else if (k.Split(' ')[2] == "g")
                                {
                                    p.Image = Properties.Resources.peon_negro_pixel;
                                    p.Name = destX + " " + destY + " g";
                                }

                                // Limpiar casilla de origen
                                P[origX, origY].Image = null;
                                P[origX, origY].Name = origX + " " + origY;

                                // Si se movió 2 filas, significa que hubo una captura (salto)
                                if (Math.Abs(destX - origX) == 2)
                                {
                                    int capX = (origX + destX) / 2;
                                    int capY = (origY + destY) / 2;

                                    if (P[capX, capY].Name.Split(' ').Length > 2)
                                    {
                                        string capColor = P[capX, capY].Name.Split(' ')[2];
                                        if (capColor == "r") blancas++;
                                        else if (capColor == "g") negras++;

                                        p_blancas.Text = negras + "";
                                        p_negras.Text = blancas + "";
                                    }

                                    // Eliminar la pieza capturada en el punto medio
                                    P[capX, capY].Image = null;
                                    P[capX, capY].Name = capX + " " + capY;
                                }
                            }
                            else if (nombreClic.Split(' ').Length > 2 && nombreClic.Split(' ')[2] == color)
                            {
                                x = Convert.ToInt32(nombreClic.Split(' ')[0]);
                                y = Convert.ToInt32(nombreClic.Split(' ')[1]);
                                k = nombreClic;

                                if (nombreClic.Split(' ')[2] == "r") c = 1;

                                // Diagonal Derecha
                                try
                                {
                                    if (P[x + c, y + 1].Image == null)
                                    {
                                        P[x + c, y + 1].Image = Properties.Resources.Movimiento;
                                        P[x + c, y + 1].Name = (x + c) + " " + (y + 1) + " b";
                                        B1 = (x + c) + " " + (y + 1);
                                    }
                                    else if (P[x + c, y + 1].Name.Split(' ')[2] != nombreClic.Split(' ')[2] && P[x + (c * 2), y + 2].Image == null)
                                    {
                                        P[x + (c * 2), y + 2].Image = Properties.Resources.Movimiento;
                                        P[x + (c * 2), y + 2].Name = (x + (c * 2)) + " " + (y + 2) + " b";
                                        B1 = (x + (c * 2)) + " " + (y + 2);
                                    }
                                }
                                catch { }

                                // Diagonal Izquierda
                                try
                                {
                                    if (P[x + c, y - 1].Image == null)
                                    {
                                        P[x + c, y - 1].Image = Properties.Resources.Movimiento;
                                        P[x + c, y - 1].Name = (x + c) + " " + (y - 1) + " b";
                                        B2 = (x + c) + " " + (y - 1);
                                    }
                                    else if (P[x + c, y - 1].Name.Split(' ')[2] != nombreClic.Split(' ')[2] && P[x + (c * 2), y - 2].Image == null)
                                    {
                                        P[x + (c * 2), y - 2].Image = Properties.Resources.Movimiento;
                                        P[x + (c * 2), y - 2].Name = (x + (c * 2)) + " " + (y - 2) + " b";
                                        B2 = (x + (c * 2)) + " " + (y - 2);
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




