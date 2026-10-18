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
        private bool editandoPerfil = false;
        private ToolTip _tipFoto;

        private static string SupabaseUrl => SupabaseConfig.SupabaseUrl;
        private static string SupabaseKey => SupabaseConfig.SupabaseKey;

        public Perfil()
        {
            InitializeComponent();

            // Activa el protocolo de seguridad obligatorio para HTTPS en .NET 4.7.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Tema.ApplyTheme(this);

            _tipFoto = new ToolTip();
            _tipFoto.SetToolTip(pictureBox_perfil, "Haz clic en la foto para cambiarla");
            _tipFoto.SetToolTip(editar_btn, "Subir o cambiar la foto de perfil");
            _tipFoto.SetToolTip(editar_correo_btn, "Editar nombre y correo");
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

        // 1. Lápiz junto a la foto: permite subir/cambiar la imagen de perfil.
        private void editar_btn_Click(object sender, EventArgs e)
        {
            SeleccionarFoto();
        }

        // 1c. Clic en la foto de perfil: también permite elegir una nueva imagen.
        private void pictureBox_perfil_Click(object sender, EventArgs e)
        {
            SeleccionarFoto();
        }

        // 1d. Lápiz junto al correo: permite editar el nombre y el correo.
        private void editar_correo_btn_Click(object sender, EventArgs e)
        {
            guardarImagenOriginal();
            editandoPerfil = true;

            nombre_txt.Text = Sesion.NombreUsuario;
            correo_txt.Text = Sesion.CorreoUsuario;
            nombre_txt.Visible = true;
            correo_txt.Visible = true;
            cancelar_btn.Visible = true;
        }

        // Abre el diálogo para elegir una foto y la deja lista para guardar.
        private void SeleccionarFoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Seleccionar imagen";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (imagenOriginal == null)
                guardarImagenOriginal();

            editandoPerfil = true;
            rutaImagenSeleccionada = openFileDialog.FileName;
            pictureBox_perfil.Image = Image.FromFile(rutaImagenSeleccionada);
            pictureBox_perfil.SizeMode = PictureBoxSizeMode.Zoom;

            // Conservar los datos actuales para no sobrescribirlos al guardar.
            nombre_txt.Text = Sesion.NombreUsuario;
            correo_txt.Text = Sesion.CorreoUsuario;

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
            editandoPerfil = false;
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

        // 2. Botón Guardar: guarda nombre, correo y (si se eligió) la foto.
        private async void guardar_btn_Click(object sender, EventArgs e)
        {
            if (!editandoPerfil)
            {
                MessageBox.Show("Pulsa el lápiz para editar tu nombre o correo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombreUsuario = Sesion.NombreUsuario;
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                MessageBox.Show("No hay una sesión iniciada. Vuelve a iniciar sesión.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nombre y correo editables (se muestran con editar_btn)
            string nuevoNombre = (nombre_txt.Text ?? "").Trim();
            string nuevoCorreo = (correo_txt.Text ?? "").Trim();
            if (string.IsNullOrEmpty(nuevoNombre))
                nuevoNombre = nombreUsuario;
            if (!string.IsNullOrEmpty(nuevoCorreo) && !EsCorreoValido(nuevoCorreo))
            {
                MessageBox.Show("El correo electrónico no tiene un formato válido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // La foto solo se sube si el usuario seleccionó una nueva.
            bool cambiarImagen = !string.IsNullOrEmpty(rutaImagenSeleccionada);

            try
            {
                string urlImagen = null; // null = conservar la foto actual

                if (cambiarImagen)
                {
                    string extension = Path.GetExtension(rutaImagenSeleccionada);
                    if (string.IsNullOrEmpty(extension))
                        extension = ".jpg";
                    string nombreArchivo = $"avatar_{nuevoNombre}{extension}";

                    // Eliminar la foto anterior (si es distinta) para no acumular archivos en Storage
                    string archivoAnterior = ExtraerNombreArchivo(await ObtenerImagenUrlUsuario(nombreUsuario));
                    if (!string.IsNullOrEmpty(archivoAnterior) &&
                        !string.Equals(archivoAnterior, nombreArchivo, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            await EliminarArchivoStorage(archivoAnterior);
                        }
                        catch
                        {
                            // Si no se puede borrar la anterior, continuar con el guardado de la nueva
                        }
                    }

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
                    urlImagen = $"{SupabaseUrl}/storage/v1/object/public/perfiles/{nombreArchivo}";
                }

                // C. Actualizar datos en la tabla 'usuarios'
                string urlTabla = $"{SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");
                    client.DefaultRequestHeaders.Add("Prefer", "return=minimal");

                    var datosActualizar = new Dictionary<string, object>
                    {
                        { "email", string.IsNullOrEmpty(nuevoCorreo) ? null : nuevoCorreo },
                        { "nombre_usuario", nuevoNombre }
                    };
                    if (urlImagen != null)
                        datosActualizar["imagen_url"] = urlImagen;

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
                if (urlImagen != null)
                    Sesion.ImagenUrl = urlImagen;
                Sesion.NombreUsuario = nuevoNombre;
                Sesion.CorreoUsuario = string.IsNullOrEmpty(nuevoCorreo) ? "" : nuevoCorreo;

                // Actualizar etiquetas visibles del perfil
                nombre.Text = nuevoNombre;
                correo.Text = CorreoMostrar(nuevoCorreo);

                // Ocultar campos de edición
                rutaImagenSeleccionada = "";
                editandoPerfil = false;
                nombre_txt.Visible = false;
                correo_txt.Visible = false;
                cancelar_btn.Visible = false;
                if (imagenOriginal != null)
                {
                    imagenOriginal.Dispose();
                    imagenOriginal = null;
                }

                MessageBox.Show("¡Perfil guardado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar datos/imagen desde el servidor
                await CargarPerfilUsuario(nuevoNombre);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el perfil: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2b. Botón Eliminar la última foto guardada (libera espacio en Supabase)
        private async void eliminar_foto_btn_Click(object sender, EventArgs e)
        {
            string nombreUsuario = Sesion.NombreUsuario;
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                MessageBox.Show("No hay una sesión iniciada. Vuelve a iniciar sesión.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Eliminar la última foto de perfil guardada? Esta acción no se puede deshacer.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                string urlActual = await ObtenerImagenUrlUsuario(nombreUsuario);
                string nombreArchivo = ExtraerNombreArchivo(urlActual);

                if (string.IsNullOrEmpty(nombreArchivo))
                {
                    MessageBox.Show("No hay ninguna foto guardada para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // A. Borrar el archivo del almacenamiento (si falla, igual quitamos la referencia)
                bool borradoFisico = await EliminarArchivoStorage(nombreArchivo);

                // B. Quitar la referencia en la tabla 'usuarios'
                string urlTabla = $"{SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");
                    client.DefaultRequestHeaders.Add("Prefer", "return=minimal");

                    string jsonBody = JsonConvert.SerializeObject(new { imagen_url = (string)null });
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

                // C. Limpiar la interfaz y la sesión
                Sesion.ImagenUrl = "";
                pictureBox_perfil.Image = null;

                if (borradoFisico)
                {
                    MessageBox.Show("Foto de perfil eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("La foto se quitó de tu perfil, pero el archivo no se pudo borrar del almacenamiento.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar la foto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Devuelve la URL de la imagen guardada del usuario (o "" si no tiene)
        private static async Task<string> ObtenerImagenUrlUsuario(string nombreUsuario)
        {
            string urlConsulta = $"{SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}&select=imagen_url";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");

                HttpResponseMessage response = await client.GetAsync(urlConsulta);
                if (!response.IsSuccessStatusCode)
                    return "";

                string jsonResult = await response.Content.ReadAsStringAsync();
                dynamic usuarios = JsonConvert.DeserializeObject(jsonResult);

                if (usuarios != null && usuarios.Count > 0 && usuarios[0].imagen_url != null)
                    return usuarios[0].imagen_url.ToString();
            }

            return "";
        }

        // Extrae el nombre del archivo de una URL del bucket "perfiles"
        private static string ExtraerNombreArchivo(string urlImagen)
        {
            if (string.IsNullOrEmpty(urlImagen))
                return "";

            int idx = urlImagen.IndexOf("/perfiles/", StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
                return "";

            string resto = urlImagen.Substring(idx + "/perfiles/".Length);
            int q = resto.IndexOf('?');
            if (q >= 0)
                resto = resto.Substring(0, q);

            return resto.Trim('/');
        }

        // Borra un archivo del bucket "perfiles" (endpoint de objeto individual).
        // Devuelve true si el archivo quedó eliminado (o ya no existía); false si falló.
        private static async Task<bool> EliminarArchivoStorage(string nombreArchivo)
        {
            string urlStorage = $"{SupabaseUrl}/storage/v1/object/perfiles/{Uri.EscapeDataString(nombreArchivo)}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("DELETE"), urlStorage);
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    return true;

                // Si el archivo ya no existe, lo consideramos eliminado.
                if ((int)response.StatusCode == 404)
                    return true;

                // 401/403 => falta la política DELETE en el bucket (Supabase → Storage → Policies).
                return false;
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

        // ---------------- Descripción del perfil (máx. 50 caracteres) ----------------

        private const int MaxDescripcion = 50;
        private const string SinDescripcion = "(sin descripción)";

        private static string MostrarDescripcion(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? SinDescripcion : texto;
        }

        private async void editar_descripcion_btn_Click(object sender, EventArgs e)
        {
            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("Inicia sesión para editar tu descripción.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string actual = descripcion.Text;
            if (actual == SinDescripcion) actual = "";

            string nuevo = PedirDescripcion(actual);
            if (nuevo == null) return; // el usuario canceló

            try
            {
                string urlTabla = $"{SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(usuario)}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");
                    client.DefaultRequestHeaders.Add("Prefer", "return=minimal");

                    var datosActualizar = new
                    {
                        descripcion = string.IsNullOrWhiteSpace(nuevo) ? null : nuevo
                    };
                    string jsonBody = JsonConvert.SerializeObject(datosActualizar);
                    StringContent contentJson = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), urlTabla)
                    {
                        Content = contentJson
                    };

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        string err = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error DB ({response.StatusCode}): {err}");
                    }
                }

                descripcion.Text = MostrarDescripcion(nuevo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar la descripción: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Pequeño diálogo modal con un TextBox limitado a MaxDescripcion caracteres.
        // Devuelve el texto introducido o null si el usuario cancela.
        private string PedirDescripcion(string actual)
        {
            using (Form dlg = new Form())
            {
                dlg.Text = "Descripción";
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.MinimizeBox = false;
                dlg.MaximizeBox = false;
                dlg.ShowInTaskbar = false;
                dlg.ClientSize = new Size(340, 156);

                Label etiqueta = new Label();
                etiqueta.Name = "etiqueta_descripcion";
                etiqueta.Text = "Escribe una descripción corta para tu perfil (máximo " + MaxDescripcion + " caracteres):";
                etiqueta.AutoSize = false;
                etiqueta.Location = new Point(12, 12);
                etiqueta.Size = new Size(316, 40);

                TextBox texto = new TextBox();
                texto.Name = "texto_descripcion";
                texto.MaxLength = MaxDescripcion;
                texto.Location = new Point(12, 58);
                texto.Size = new Size(316, 20);
                texto.Text = actual ?? "";

                Label contador = new Label();
                contador.Name = "contador_descripcion";
                contador.AutoSize = true;
                contador.Location = new Point(12, 82);
                contador.Text = texto.Text.Length + "/" + MaxDescripcion;

                texto.TextChanged += (s, ev) => { contador.Text = texto.Text.Length + "/" + MaxDescripcion; };

                Button guardar = new Button();
                guardar.Name = "bt_guardar_desc";
                guardar.Text = "GUARDAR";
                guardar.DialogResult = DialogResult.OK;
                guardar.Location = new Point(172, 114);
                guardar.Size = new Size(75, 28);

                Button cancelar = new Button();
                cancelar.Name = "bt_cancelar_desc";
                cancelar.Text = "CANCELAR";
                cancelar.DialogResult = DialogResult.Cancel;
                cancelar.Location = new Point(253, 114);
                cancelar.Size = new Size(75, 28);

                dlg.Controls.Add(etiqueta);
                dlg.Controls.Add(texto);
                dlg.Controls.Add(contador);
                dlg.Controls.Add(guardar);
                dlg.Controls.Add(cancelar);
                dlg.AcceptButton = guardar;
                dlg.CancelButton = cancelar;

                Tema.ApplyTheme(dlg);

                return dlg.ShowDialog(this) == DialogResult.OK ? texto.Text.Trim() : null;
            }
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

            // Las estadísticas (victorias/derrotas/partidas) salen de la base
            // local, así que se muestran aunque falle la conexión.
            MostrarEstadisticasPartidas(nombreUsuario);

            try
            {
                string urlConsulta = $"{SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}"
                                   + "&select=imagen_url,email,elo,partidas_jugadas,partidas_ganadas,partidas_perdidas,descripcion";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseKey}");

                    HttpResponseMessage response = await client.GetAsync(urlConsulta);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        var usuarios = JsonConvert.DeserializeObject<List<DatosPerfil>>(jsonResult);

                        if (usuarios != null && usuarios.Count > 0)
                        {
                            DatosPerfil d = usuarios[0];

                            // Rellenar nombre y correo (etiquetas y sesión)
                            string email = d.email ?? "";
                            Sesion.CorreoUsuario = email;
                            Sesion.NombreUsuario = nombreUsuario;
                            nombre.Text = nombreUsuario;
                            correo.Text = CorreoMostrar(email);
                            descripcion.Text = MostrarDescripcion(d.descripcion);

                            // Rellenar el ELO y las estadísticas en sus etiquetas
                            MostrarElo(d);
                            MostrarEstadisticasPartidas(nombreUsuario);

                            string urlImagen = d.imagen_url;

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
            catch (Exception ex)
            {
                // Muestra la razón exacta si falla en lugar de borrar la pantalla en silencio
                Console.WriteLine("Fallo al cargar imagen: " + ex.Message);
            }

            pictureBox_perfil.Image = null;
        }

        // Rellena el ELO en las etiquetas del perfil
        private void MostrarElo(DatosPerfil d)
        {
            double eloValor = d.elo ?? 0;
            elo.Text = eloValor.ToString("0");
            elo_menu2.Text = eloValor.ToString("0");
        }

        // Rellena victorias, derrotas y partidas con las partidas reales jugadas
        // con esta cuenta (base local), igual que la ventana de Partidas.
        // Solo se cuentan las partidas finalizadas.
        private void MostrarEstadisticasPartidas(string usuario)
        {
            List<PartidaRepository.Partida> partidas = PartidaRepository.ListarPartidas(null, usuario);
            int jugadas = partidas.Count(EsFinalizada);
            int ganadas = partidas.Count(p => EsVictoria(p, usuario));
            int perdidas = partidas.Count(p => EsDerrota(p, usuario));

            victorias.Text = ganadas.ToString();
            victorias_menu2.Text = ganadas.ToString();
            partidas_menu2.Text = jugadas.ToString();
            derrotas_menu2.Text = perdidas.ToString();
        }

        private static bool EsFinalizada(PartidaRepository.Partida p)
        {
            return string.Equals(p.estado, "finalizada", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EsVictoria(PartidaRepository.Partida p, string usuario)
        {
            return EsFinalizada(p)
                   && !string.IsNullOrWhiteSpace(p.ganador)
                   && string.Equals(p.ganador, usuario, StringComparison.OrdinalIgnoreCase);
        }

        private static bool EsDerrota(PartidaRepository.Partida p, string usuario)
        {
            return EsFinalizada(p)
                   && !string.IsNullOrWhiteSpace(p.ganador)
                   && !string.Equals(p.ganador, "empate", StringComparison.OrdinalIgnoreCase)
                   && !string.Equals(p.ganador, usuario, StringComparison.OrdinalIgnoreCase);
        }

        // Datos del usuario que llegan de Supabase para el perfil
        private class DatosPerfil
        {
            public string imagen_url { get; set; }
            public string email { get; set; }
            public double? elo { get; set; }
            public int? partidas_jugadas { get; set; }
            public int? partidas_ganadas { get; set; }
            public int? partidas_perdidas { get; set; }
            public string descripcion { get; set; }
        }
    }
}