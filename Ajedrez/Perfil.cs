using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Ajedrez
{
    public partial class Perfil : Form
    {
        private string rutaImagenSeleccionada = "";

        private static string SupabaseUrl => SupabaseConfig.SupabaseUrl;
        private static string SupabaseKey => SupabaseConfig.SupabaseKey;

        public Perfil()
        {
            InitializeComponent();

            // Activa el protocolo de seguridad obligatorio para HTTPS en .NET 4.7.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Tema.ApplyTheme(this);
        }

        private void ir_menu_Click(object sender, EventArgs e)
        {
            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }

        private void m_claro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(false);
            Tema.ApplyTheme(this);
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);
            Tema.ApplyTheme(this);
        }

        private async void Perfil_Load(object sender, EventArgs e)
        {
            picture_victorias.Visible = true;
            picture_elo.Visible = true;
            picture_derrotas.Visible = true;
            picture_partidas.Visible = true;

            // Cargar la foto del usuario activo
            await CargarPerfilUsuario(Sesion.NombreUsuario);
        }

        // 1. Botón Seleccionar / Editar Imagen (también muestra campos de nombre y correo)
        private void editar_btn_Click(object sender, EventArgs e)
        {
            // Guardar el estado original por si el usuario cancela
            guardarImagenOriginal();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Seleccionar imagen";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                rutaImagenSeleccionada = openFileDialog.FileName;
                pictureBox_perfil.Image = Image.FromFile(rutaImagenSeleccionada);
                pictureBox_perfil.SizeMode = PictureBoxSizeMode.Zoom;
            }

            // Mostrar campos para editar nombre y correo
            nombre_txt.Text = Sesion.NombreUsuario;
            correo_txt.Text = Sesion.CorreoUsuario;
            nombre_txt.Visible = true;
            correo_txt.Visible = true;
            cancelar_btn.Visible = true;
        }

        // 1b. Cancelar edición: restaura el estado anterior y oculta los campos
        private void cancelar_btn_Click(object sender, EventArgs e)
        {
            rutaImagenSeleccionada = "";

            if (imagenOriginal != null)
            {
                pictureBox_perfil.Image = imagenOriginal;
                pictureBox_perfil.SizeMode = PictureBoxSizeMode.Zoom;
                imagenOriginal = null;
            }

            nombre_txt.Text = Sesion.NombreUsuario;
            correo_txt.Text = Sesion.CorreoUsuario;
            nombre_txt.Visible = false;
            correo_txt.Visible = false;
            cancelar_btn.Visible = false;
        }

        private Image imagenOriginal = null;

        // Guarda una copia de la imagen que se muestra antes de editar
        private void guardarImagenOriginal()
        {
            if (imagenOriginal != null)
            {
                imagenOriginal.Dispose();
                imagenOriginal = null;
            }
            if (pictureBox_perfil.Image != null)
            {
                imagenOriginal = (Image)pictureBox_perfil.Image.Clone();
            }
        }

        // 2. Botón Guardar Imagen en Supabase
        private async void guardar_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rutaImagenSeleccionada))
            {
                MessageBox.Show("Por favor selecciona una imagen primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string nombreUsuario = Sesion.NombreUsuario;
                if (string.IsNullOrEmpty(nombreUsuario))
                {
                    MessageBox.Show("No hay una sesión iniciada. Vuelve a iniciar sesión.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Nombre y correo editables (se muestran con editar_btn)
                string nuevoNombre = nombre_txt.Text.Trim();
                string nuevoCorreo = correo_txt.Text.Trim();
                if (string.IsNullOrEmpty(nuevoNombre))
                    nuevoNombre = nombreUsuario;
                if (!string.IsNullOrEmpty(nuevoCorreo) && !EsCorreoValido(nuevoCorreo))
                {
                    MessageBox.Show("El correo electrónico no tiene un formato válido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string extension = Path.GetExtension(rutaImagenSeleccionada);
                if (string.IsNullOrEmpty(extension))
                    extension = ".jpg";
                string nombreArchivo = $"avatar_{nuevoNombre}{extension}";

                // A. Subir/sobrescribir imagen a Supabase Storage (bucket "perfiles")
                byte[] bytesImagen = File.ReadAllBytes(rutaImagenSeleccionada);
                string urlStorage = $"{SupabaseUrl}/storage/v1/object/perfiles/{nombreArchivo}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");
                    client.DefaultRequestHeaders.Add("x-upsert", "true");

                    ByteArrayContent content = new ByteArrayContent(bytesImagen);
                    // Usar el tipo MIME correcto según la extensión
                    content.Headers.ContentType = new MediaTypeHeaderValue(GetMime(extension));

                    HttpResponseMessage responseStorage = await client.PostAsync(urlStorage, content);

                    // Si el archivo ya existe, intentar con PUT (upsert de reemplazo)
                    if (!responseStorage.IsSuccessStatusCode)
                    {
                        HttpRequestMessage requestPut = new HttpRequestMessage(new HttpMethod("PUT"), urlStorage)
                        {
                            Content = content
                        };
                        requestPut.Headers.Add("x-upsert", "true");
                        responseStorage = await client.SendAsync(requestPut);
                    }

                    if (!responseStorage.IsSuccessStatusCode)
                    {
                        string err = await responseStorage.Content.ReadAsStringAsync();
                        throw new Exception($"Error Storage ({responseStorage.StatusCode}): {err}");
                    }
                }

                // B. URL Pública de la imagen
                string urlPublica = $"{SupabaseUrl}/storage/v1/object/public/perfiles/{nombreArchivo}";

                // C. Actualizar datos en la tabla 'usuarios'
                string urlTabla = $"{SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");
                    client.DefaultRequestHeaders.Add("Prefer", "return=minimal");

                    var datosActualizar = new
                    {
                        imagen_url = urlPublica,
                        email = string.IsNullOrEmpty(nuevoCorreo) ? null : nuevoCorreo,
                        nombre_usuario = nuevoNombre
                    };
                    string jsonBody = JsonConvert.SerializeObject(datosActualizar);
                    StringContent contentJson = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), urlTabla)
                    {
                        Content = contentJson
                    };

                    HttpResponseMessage responseTabla = await client.SendAsync(request);

                    if (!responseTabla.IsSuccessStatusCode)
                    {
                        string err = await responseTabla.Content.ReadAsStringAsync();
                        throw new Exception($"Error DB ({responseTabla.StatusCode}): {err}");
                    }
                }

                // Guardar también en la sesión para mostrarla de inmediato
                Sesion.ImagenUrl = urlPublica;
                Sesion.NombreUsuario = nuevoNombre;
                Sesion.CorreoUsuario = string.IsNullOrEmpty(nuevoCorreo) ? "" : nuevoCorreo;

                // Actualizar etiquetas visibles del perfil
                nombre.Text = nuevoNombre;
                correo.Text = CorreoMostrar(nuevoCorreo);

                // Ocultar campos de edición
                nombre_txt.Visible = false;
                correo_txt.Visible = false;
                cancelar_btn.Visible = false;
                if (imagenOriginal != null)
                {
                    imagenOriginal.Dispose();
                    imagenOriginal = null;
                }

                MessageBox.Show("¡Perfil guardado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar imagen desde el servidor
                await CargarPerfilUsuario(nuevoNombre);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool EsCorreoValido(string correo)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(correo);
                return addr.Address == correo;
            }
            catch
            {
                return false;
            }
        }

        private static string CorreoMostrar(string correo)
        {
            return string.IsNullOrEmpty(correo) ? "Sin correo" : correo;
        }

        private static string GetMime(string extension)
        {
            switch (extension.ToLowerInvariant())
            {
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".bmp": return "image/bmp";
                case ".jpg":
                case ".jpeg":
                default: return "image/jpeg";
            }
        }

        // 3. Consultar y mostrar la foto del usuario activo de forma segura
        public async Task CargarPerfilUsuario(string nombreUsuario)
        {
            rutaImagenSeleccionada = "";

            if (string.IsNullOrEmpty(nombreUsuario))
            {
                pictureBox_perfil.Image = null;
                return;
            }

            try
            {
                string urlConsulta = $"{SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}&select=imagen_url,email";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");

                    HttpResponseMessage response = await client.GetAsync(urlConsulta);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        dynamic usuarios = JsonConvert.DeserializeObject(jsonResult);

                        if (usuarios != null && usuarios.Count > 0)
                        {
                            // Rellenar nombre y correo (etiquetas y sesión)
                            string email = usuarios[0].email != null ? usuarios[0].email.ToString() : "";
                            Sesion.CorreoUsuario = email ?? "";
                            Sesion.NombreUsuario = nombreUsuario;
                            nombre.Text = nombreUsuario;
                            correo.Text = CorreoMostrar(email);

                            if (usuarios[0].imagen_url != null)
                            {
                                string urlImagen = usuarios[0].imagen_url.ToString();

                                if (!string.IsNullOrWhiteSpace(urlImagen))
                                {
                                    // Descarga limpia de la imagen mediante HttpClient
                                    HttpResponseMessage imgResponse = await client.GetAsync(urlImagen);
                                    if (imgResponse.IsSuccessStatusCode)
                                    {
                                        byte[] bytes = await imgResponse.Content.ReadAsByteArrayAsync();
                                        using (MemoryStream ms = new MemoryStream(bytes))
                                        {
                                            var img = Image.FromStream(ms);
                                            pictureBox_perfil.Image = img;
                                            pictureBox_perfil.SizeMode = PictureBoxSizeMode.Zoom;
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra la razón exacta si falla en lugar de borrar la pantalla en silencio
                Console.WriteLine("Fallo al cargar imagen: " + ex.Message);
            }

            pictureBox_perfil.Image = null;
        }
    }
}