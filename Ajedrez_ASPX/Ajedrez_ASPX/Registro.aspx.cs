using System;

namespace Ajedrez_ASPX
{
    /// <summary>Registro de usuarios (equivalente a REGISTRADOR del escritorio).</summary>
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Mostrar/ocultar contraseña (equivale a checkBox1_CheckedChanged).
                chkVer.Attributes["onclick"] =
                    "var t=document.getElementById('" + txtContrasena.ClientID + "');" +
                    " t.type=this.checked?'text':'password';";
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
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

            btnRegistrar.Enabled = false;
            btnRegistrar.Text = "Registrando...";

            try
            {
                // Comprobar si ya existe un usuario con el mismo nombre
                // (sin distinguir mayúsculas/minúsculas ni espacios sobrantes).
                if (Nube.ExisteUsuario(txtNombre.Text.Trim()))
                {
                    lblMensaje.Text = "El nombre de usuario ya existe. Por favor, elige otro.";
                    txtNombre.Focus();
                    return;
                }

                if (Nube.CrearUsuarioBasico(txtNombre.Text.Trim(), txtContrasena.Text))
                {
                    string usuario = txtNombre.Text.Trim();
                    ClientScript.RegisterStartupScript(GetType(), "registrado",
                        "alert('¡Usuario registrado exitosamente!\\n\\nAhora inicia sesión con tus credenciales.');",
                        true);
                    // Llevar al login con el usuario ya escrito (como el escritorio),
                    // conservando el destino al que se quería llegar.
                    string destino = Request.QueryString["returnUrl"];
                    string url = "Default.aspx?usuario=" + Uri.EscapeDataString(usuario);
                    if (!string.IsNullOrWhiteSpace(destino))
                        url += "&returnUrl=" + Server.UrlEncode(destino);
                    Response.Redirect(url);
                }
                else
                {
                    lblMensaje.Text = "No se pudo registrar el usuario.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al registrar: " + ex.Message;
            }
            finally
            {
                if (!Response.IsRequestBeingRedirected)
                {
                    btnRegistrar.Enabled = true;
                    btnRegistrar.Text = "Registrar";
                }
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}
