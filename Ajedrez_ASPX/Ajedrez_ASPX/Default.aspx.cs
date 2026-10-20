using System;

namespace Ajedrez_ASPX
{
    /// <summary>Inicio de sesión (equivalente al formulario LOGIN del escritorio).</summary>
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Mostrar/ocultar contraseña (equivale a checkBox1_CheckedChanged).
                chkVer.Attributes["onclick"] =
                    "var t=document.getElementById('" + txtContrasena.ClientID + "');" +
                    " t.type=this.checked?'text':'password';";

                // Tras un registro se precarga el usuario (igual que PrecargarUsuario).
                string usuario = Request.QueryString["usuario"];
                if (!string.IsNullOrWhiteSpace(usuario))
                {
                    txtNombre.Text = usuario;
                }
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            // Validar campos vacíos (mismos mensajes que el escritorio).
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                lblMensaje.Text = "Por favor, ingresa un nombre de usuario.";
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                lblMensaje.Text = "Por favor, ingresa una contraseña.";
                txtContrasena.Focus();
                return;
            }

            btnIngresar.Enabled = false;
            btnIngresar.Text = "Verificando...";

            try
            {
                var u = Nube.ValidarUsuario(txtNombre.Text.Trim(), txtContrasena.Text);
                if (u != null)
                {
                    Sesion.NombreUsuario = u.nombre_usuario;
                    Sesion.ImagenUrl = string.Empty;

                    // Volver a la página que exigía sesión (si viene indicada)
                    string destino = Request.QueryString["returnUrl"];
                    if (!string.IsNullOrWhiteSpace(destino) && EsDestinoSeguro(destino))
                        Response.Redirect(destino);
                    else
                        Response.Redirect("Menu.aspx");
                    return;
                }

                lblMensaje.Text = "❌ Usuario o contraseña incorrectos.";
            }
            catch
            {
                lblMensaje.Text = "No hay conexión con el servidor. Inténtalo de nuevo.";
            }
            finally
            {
                if (!Response.IsRequestBeingRedirected)
                {
                    btnIngresar.Enabled = true;
                    btnIngresar.Text = "Verificar";
                }
            }
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            string destino = Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(destino) && EsDestinoSeguro(destino))
                Response.Redirect("Registro.aspx?returnUrl=" + Server.UrlEncode(destino));
            else
                Response.Redirect("Registro.aspx");
        }

        /// <summary>
        /// Solo se aceptan destinos locales (rutas relativas sin esquema ni "//"),
        /// para evitar redirecciones abiertas hacia sitios externos.
        /// </summary>
        private static bool EsDestinoSeguro(string destino)
        {
            if (string.IsNullOrWhiteSpace(destino)) return false;
            if (destino.StartsWith("//") || destino.StartsWith("\\")) return false;
            if (destino.IndexOf("://") >= 0) return false;
            return destino.EndsWith(".aspx", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
