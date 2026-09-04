using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
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
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
           

        }

        private async void menu_Load(object sender, EventArgs e)
        {
            // Activa el protocolo de seguridad HTTPS para .NET 4.7.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            lbl_usuario.Text = Sesion.NombreUsuario;

            // Cargar imagen de perfil desde Supabase
            await CargarUsuarioMenu();
        }

        // Recargar el nombre y la foto cada vez que vuelves al menú (para que sea dinámico)
        private async void menu_Activated(object sender, EventArgs e)
        {
            lbl_usuario.Text = Sesion.NombreUsuario;
            await CargarUsuarioMenu();
        }

        // Carga el nombre y la imagen del usuario activo desde Supabase
        public async Task CargarUsuarioMenu()
        {
            string nombreUsuario = Sesion.NombreUsuario;
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                lbl_usuario.Text = "Invitado";
                pic_usuario.Image = null;
                return;
            }

            lbl_usuario.Text = nombreUsuario;

            try
            {
                string url = $"{SupabaseConfig.SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}&select=imagen_url";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseConfig.SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseConfig.SupabaseKey}");

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        return;

                    string json = await response.Content.ReadAsStringAsync();
                    dynamic usuarios = JsonConvert.DeserializeObject(json);

                    if (usuarios != null && usuarios.Count > 0 && usuarios[0].imagen_url != null)
                    {
                        string urlImagen = usuarios[0].imagen_url.ToString();
                        if (string.IsNullOrWhiteSpace(urlImagen))
                            return;

                        HttpResponseMessage imgResponse = await client.GetAsync(urlImagen);
                        if (imgResponse.IsSuccessStatusCode)
                        {
                            byte[] bytes = await imgResponse.Content.ReadAsByteArrayAsync();
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                pic_usuario.Image = Image.FromStream(ms);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fallo al cargar usuario en menú: " + ex.Message);
            }
        }
    }
    }

