using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.UI.WebControls;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Perfil del usuario (equivalente a la ventana PERFIL del escritorio):
    /// foto de perfil, nombre, correo, ELO, descripción y estadísticas.
    /// </summary>
    public partial class Perfil : System.Web.UI.Page
    {
        private const int MaxDescripcion = 50;
        private const string SinDescripcion = "(sin descripción)";

        /// <summary>Tamaño máximo permitido para la foto de perfil (5 MB).</summary>
        private const int MaxBytesFoto = 5 * 1024 * 1024;

        private static string SupabaseUrl { get { return SupabaseConfig.SupabaseUrl; } }
        private static string SupabaseKey { get { return SupabaseConfig.SupabaseKey; } }

        // Estado de edición de nombre/correo (equivale a editandoPerfil del escritorio).
        private bool Editando
        {
            get { return ViewState["Editando"] is bool && (bool)ViewState["Editando"]; }
            set { ViewState["Editando"] = value; }
        }

        // Estado de edición de la descripción (equivale al diálogo PedirDescripcion).
        private bool EditandoDescripcion
        {
            get { return ViewState["EditandoDescripcion"] is bool && (bool)ViewState["EditandoDescripcion"]; }
            set { ViewState["EditandoDescripcion"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "PERFIL";

            // El perfil es de la cuenta: sin sesión iniciada se va al login
            if (!Acceso.ExigirSesion(this)) return;

            // Activa el protocolo de seguridad obligatorio para HTTPS en .NET 4.7.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (!IsPostBack)
                CargarPerfil();

            // Muestra u oculta los campos de edición según el estado guardado.
            pnlEdicion.Visible = Editando;
            btnCancelar.Visible = Editando;
            pnlDescripcion.Visible = EditandoDescripcion;

            // Contador de caracteres de la descripción (máx. 50, como el diálogo del escritorio).
            string contadorScript =
                "var c=document.getElementById('" + lblContador.ClientID + "');" +
                "if(c)c.textContent=this.value.length+'/50';";
            txtDescripcion.Attributes["onkeyup"] = contadorScript;
            txtDescripcion.Attributes["oninput"] = contadorScript;
            lblContador.Text = txtDescripcion.Text.Length + "/" + MaxDescripcion;

            // La confirmación solo aparece con sesión iniciada (el escritorio primero
            // avisa si no hay sesión y después pregunta).
            btnEliminarFoto.OnClientClick = string.IsNullOrEmpty(Sesion.NombreUsuario)
                ? ""
                : "return confirm('¿Eliminar la última foto de perfil guardada? Esta acción no se puede deshacer.');";
        }

        // ---------------- Navegación ----------------

        protected void btnMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        // ---------------- Carga del perfil ----------------

        // Consulta y muestra los datos del usuario activo (como CargarPerfilUsuario).
        private void CargarPerfil()
        {
            string usuario = Sesion.NombreUsuario;

            // Las estadísticas salen de las partidas finalizadas, igual que en el escritorio.
            PartidaRepository.Estadisticas est = PartidaRepository.EstadisticasDe(usuario);
            victorias.Text = est.Ganadas.ToString();
            statVictorias.Text = est.Ganadas.ToString();
            statDerrotas.Text = est.Perdidas.ToString();
            statEmpates.Text = est.Empates.ToString();
            statTotal.Text = est.Total.ToString();

            if (string.IsNullOrEmpty(usuario))
            {
                // Sin sesión: se muestran valores vacíos.
                lblNombre.Text = "";
                lblCorreo.Text = "";
                lblElo.Text = "0";
                lblDescripcion.Text = MostrarDescripcion("");
                imgPerfil.Visible = false;
                pnlFotoVacia.Visible = true;
                return;
            }

            try
            {
                // Mismas columnas que consulta el escritorio:
                // imagen_url, email, elo, partidas_jugadas, partidas_ganadas,
                // partidas_perdidas, descripcion.
                UsuarioCompleto d = Nube.ObtenerUsuarioCompleto(usuario);

                if (d != null)
                {
                    Sesion.CorreoUsuario = d.email ?? "";
                    Sesion.NombreUsuario = usuario;

                    lblNombre.Text = usuario;
                    lblCorreo.Text = CorreoMostrar(d.email);
                    lblElo.Text = d.elo.ToString("0");
                    lblDescripcion.Text = MostrarDescripcion(d.descripcion);

                    if (!string.IsNullOrWhiteSpace(d.imagen_url))
                    {
                        imgPerfil.ImageUrl = d.imagen_url;
                        imgPerfil.Visible = true;
                        pnlFotoVacia.Visible = false;
                    }
                    else
                    {
                        imgPerfil.Visible = false;
                        pnlFotoVacia.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra la razón exacta si falla en lugar de borrar la pantalla en silencio
                Console.WriteLine("Fallo al cargar imagen: " + ex.Message);
            }
        }

        // ---------------- Edición de nombre y correo ----------------

        // Lápiz junto al correo: permite editar el nombre y el correo.
        protected void btnEditarCorreo_Click(object sender, EventArgs e)
        {
            Editando = true;
            pnlEdicion.Visible = true;
            btnCancelar.Visible = true;

            txtNombre.Text = Sesion.NombreUsuario;
            txtCorreo.Text = Sesion.CorreoUsuario;
            lblMensaje.Text = "";
        }

        // Cancelar edición: restaura el estado anterior y oculta los campos.
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Editando = false;
            pnlEdicion.Visible = false;
            btnCancelar.Visible = false;
            txtNombre.Text = "";
            txtCorreo.Text = "";
            lblMensaje.Text = "";
        }

        // Botón Guardar: guarda nombre, correo y (si se eligió) la foto.
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            bool conFoto = fuFoto.HasFile;

            if (!Editando && !conFoto)
            {
                lblMensaje.Text = "Pulsa el lápiz para editar tu nombre o correo.";
                return;
            }

            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrEmpty(usuario))
            {
                lblMensaje.Text = "No hay una sesión iniciada. Vuelve a iniciar sesión.";
                return;
            }

            // Nombre y correo editables (se muestran con editar_correo_btn).
            string nuevoNombre = Editando ? (txtNombre.Text ?? "").Trim() : usuario;
            string nuevoCorreo = Editando ? (txtCorreo.Text ?? "").Trim() : Sesion.CorreoUsuario;
            if (string.IsNullOrEmpty(nuevoNombre))
                nuevoNombre = usuario;
            if (!string.IsNullOrEmpty(nuevoCorreo) && !EsCorreoValido(nuevoCorreo))
            {
                lblMensaje.Text = "El correo electrónico no tiene un formato válido.";
                return;
            }

            try
            {
                UsuarioCompleto d = Nube.ObtenerUsuarioCompleto(usuario);
                if (d == null)
                    throw new Exception("No se pudo cargar el usuario de la base de datos.");

                // La foto solo se sube si el usuario seleccionó una nueva.
                string urlImagen = null; // null = conservar la foto actual

                if (conFoto)
                {
                    string extension = Path.GetExtension(fuFoto.FileName);
                    if (string.IsNullOrEmpty(extension))
                        extension = ".jpg";
                    if (!EsExtensionValida(extension))
                    {
                        lblMensaje.Text = "Formato de imagen no válido. Usa archivos jpg, jpeg, png, bmp o gif.";
                        return;
                    }

                    // Validar el tamaño antes de leer el archivo: una imagen demasiado
                    // grande corta la petición y detiene la depuración sin control.
                    if (fuFoto.PostedFile != null && fuFoto.PostedFile.ContentLength > MaxBytesFoto)
                    {
                        lblMensaje.Text = "La imagen supera el tamaño máximo permitido (5 MB).";
                        return;
                    }

                    string nombreArchivo = "avatar_" + nuevoNombre + extension;

                    // Eliminar la foto anterior (si es distinta) para no acumular archivos en Storage.
                    string archivoAnterior = ExtraerNombreArchivo(d.imagen_url);
                    if (!string.IsNullOrEmpty(archivoAnterior) &&
                        !string.Equals(archivoAnterior, nombreArchivo, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            EliminarArchivoStorage(archivoAnterior);
                        }
                        catch
                        {
                            // Si no se puede borrar la anterior, continuar con el guardado de la nueva
                        }
                    }

                    // A. Subir/sobrescribir imagen a Supabase Storage (bucket "perfiles")
                    byte[] bytesImagen = LeerArchivo(fuFoto);
                    SubirImagenStorage(nombreArchivo, bytesImagen, GetMime(extension));

                    // B. URL pública de la imagen
                    urlImagen = SupabaseUrl + "/storage/v1/object/public/perfiles/" + nombreArchivo;
                }

                // C. Actualizar nombre y correo en la tabla 'usuarios'
                d.nombre_usuario = nuevoNombre;
                d.email = string.IsNullOrEmpty(nuevoCorreo) ? null : nuevoCorreo;
                d.contrasena = string.Empty; // no se cambia la contraseña desde aquí

                if (!Nube.ActualizarUsuarioCompleto(d))
                    throw new Exception("Error DB: no se pudo actualizar el usuario.");

                if (urlImagen != null)
                {
                    Nube.ActualizarImagen(d.id_usuario, urlImagen);
                    Sesion.ImagenUrl = urlImagen;
                }

                // Guardar también en la sesión para mostrarla de inmediato
                Sesion.NombreUsuario = nuevoNombre;
                Sesion.CorreoUsuario = string.IsNullOrEmpty(nuevoCorreo) ? "" : nuevoCorreo;

                // Actualizar etiquetas visibles del perfil
                lblNombre.Text = nuevoNombre;
                lblCorreo.Text = CorreoMostrar(nuevoCorreo);

                // Ocultar campos de edición
                Editando = false;
                pnlEdicion.Visible = false;
                btnCancelar.Visible = false;
                txtNombre.Text = "";
                txtCorreo.Text = "";

                lblMensaje.Text = "¡Perfil guardado correctamente!";

                // Recargar datos/imagen desde el servidor
                CargarPerfil();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al guardar el perfil: " + ex.Message;
            }
        }

        // ---------------- Eliminar foto ----------------

        // Botón Eliminar la última foto guardada (libera espacio en Supabase).
        protected void btnEliminarFoto_Click(object sender, EventArgs e)
        {
            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrEmpty(usuario))
            {
                lblMensaje.Text = "No hay una sesión iniciada. Vuelve a iniciar sesión.";
                return;
            }

            try
            {
                UsuarioCompleto d = Nube.ObtenerUsuarioCompleto(usuario);
                if (d == null)
                    throw new Exception("No se pudo cargar el usuario de la base de datos.");

                string nombreArchivo = ExtraerNombreArchivo(d.imagen_url ?? "");
                if (string.IsNullOrEmpty(nombreArchivo))
                {
                    lblMensaje.Text = "No hay ninguna foto guardada para eliminar.";
                    return;
                }

                // A. Borrar el archivo del almacenamiento (si falla, igual quitamos la referencia)
                bool borradoFisico = EliminarArchivoStorage(nombreArchivo);

                // B. Quitar la referencia en la tabla 'usuarios'
                if (!Nube.ActualizarImagen(d.id_usuario, ""))
                    throw new Exception("Error DB: no se pudo quitar la imagen del usuario.");

                // C. Limpiar la interfaz y la sesión
                Sesion.ImagenUrl = "";
                imgPerfil.ImageUrl = "";
                imgPerfil.Visible = false;
                pnlFotoVacia.Visible = true;

                if (borradoFisico)
                {
                    lblMensaje.Text = "Foto de perfil eliminada correctamente.";
                }
                else
                {
                    lblMensaje.Text = "La foto se quitó de tu perfil, pero el archivo no se pudo borrar del almacenamiento.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al eliminar la foto: " + ex.Message;
            }
        }

        // ---------------- Descripción del perfil (máx. 50 caracteres) ----------------

        protected void btnEditarDescripcion_Click(object sender, EventArgs e)
        {
            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                lblMensaje.Text = "Inicia sesión para editar tu descripción.";
                return;
            }

            string actual = lblDescripcion.Text;
            if (actual == SinDescripcion) actual = "";

            EditandoDescripcion = true;
            pnlDescripcion.Visible = true;
            txtDescripcion.Text = actual;
            lblContador.Text = actual.Length + "/" + MaxDescripcion;
            lblMensaje.Text = "";
        }

        protected void btnGuardarDesc_Click(object sender, EventArgs e)
        {
            string usuario = Sesion.NombreUsuario;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                lblMensaje.Text = "Inicia sesión para editar tu descripción.";
                return;
            }

            try
            {
                UsuarioCompleto d = Nube.ObtenerUsuarioCompleto(usuario);
                if (d == null)
                    throw new Exception("No se pudo cargar el usuario de la base de datos.");

                string nuevo = (txtDescripcion.Text ?? "").Trim();
                if (!Nube.ActualizarDescripcion(d.id_usuario, nuevo))
                    throw new Exception("Error DB: no se pudo guardar la descripción.");

                lblDescripcion.Text = MostrarDescripcion(nuevo);
                EditandoDescripcion = false;
                pnlDescripcion.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "No se pudo guardar la descripción: " + ex.Message;
            }
        }

        protected void btnCancelarDesc_Click(object sender, EventArgs e)
        {
            EditandoDescripcion = false;
            pnlDescripcion.Visible = false;
            txtDescripcion.Text = "";
            lblContador.Text = "0/" + MaxDescripcion;
            lblMensaje.Text = "";
        }

        // ---------------- Auxiliares ----------------

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

        private static string MostrarDescripcion(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? SinDescripcion : texto;
        }

        private static bool EsExtensionValida(string extension)
        {
            switch (extension.ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".bmp":
                case ".gif":
                    return true;
                default:
                    return false;
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

        private static byte[] LeerArchivo(FileUpload archivo)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                archivo.FileContent.CopyTo(ms);
                return ms.ToArray();
            }
        }

        // Extrae el nombre del archivo de una URL del bucket "perfiles".
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

        // Sube la imagen al bucket "perfiles" (POST y, si ya existe, PUT con upsert),
        // igual que en el escritorio.
        private static void SubirImagenStorage(string nombreArchivo, byte[] bytesImagen, string mime)
        {
            string urlStorage = SupabaseUrl + "/storage/v1/object/perfiles/" + nombreArchivo;

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + SupabaseKey);
                client.DefaultRequestHeaders.Add("x-upsert", "true");

                // El mismo contenido no puede reutilizarse en dos peticiones, así
                // que el PUT de reserva crea su propio ByteArrayContent.
                HttpResponseMessage responseStorage;
                using (ByteArrayContent content = new ByteArrayContent(bytesImagen))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(mime);
                    responseStorage = client.PostAsync(urlStorage, content).GetAwaiter().GetResult();
                }

                // Si el archivo ya existe, intentar con PUT (upsert de reemplazo)
                if (!responseStorage.IsSuccessStatusCode)
                {
                    using (ByteArrayContent contentPut = new ByteArrayContent(bytesImagen))
                    {
                        contentPut.Headers.ContentType = new MediaTypeHeaderValue(mime);
                        HttpRequestMessage requestPut = new HttpRequestMessage(new HttpMethod("PUT"), urlStorage)
                        {
                            Content = contentPut
                        };
                        requestPut.Headers.Add("x-upsert", "true");
                        responseStorage = client.SendAsync(requestPut).GetAwaiter().GetResult();
                    }
                }

                if (!responseStorage.IsSuccessStatusCode)
                {
                    string err = responseStorage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new Exception("Error Storage (" + responseStorage.StatusCode + "): " + err);
                }
            }
        }

        // Borra un archivo del bucket "perfiles" (endpoint de objeto individual).
        // Devuelve true si el archivo quedó eliminado (o ya no existía); false si falló.
        private static bool EliminarArchivoStorage(string nombreArchivo)
        {
            string urlStorage = SupabaseUrl + "/storage/v1/object/perfiles/" + Uri.EscapeDataString(nombreArchivo);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + SupabaseKey);

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("DELETE"), urlStorage);
                HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                    return true;

                // Si el archivo ya no existe, lo consideramos eliminado.
                if ((int)response.StatusCode == 404)
                    return true;

                // 401/403 => falta la política DELETE en el bucket (Supabase → Storage → Policies).
                return false;
            }
        }
    }
}
