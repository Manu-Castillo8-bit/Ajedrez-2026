using System.Web;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Control de acceso de la aplicación. Se considera "invitado" a quien no
    /// tiene usuario en la sesión. Las páginas que requieren cuenta (jugar,
    /// perfil, partidas, admin) redirigen al login para que no se jueguen ni
    /// se guarden partidas sin identificar.
    /// </summary>
    public static class Acceso
    {
        /// <summary>True si hay una sesión iniciada.</summary>
        public static bool HaySesion
        {
            get { return !string.IsNullOrWhiteSpace(Sesion.NombreUsuario); }
        }

        /// <summary>
        /// Exige sesión iniciada. Si es invitado, redirige al login con la
        /// página de destino y devuelve false (el llamador debe hacer return).
        /// </summary>
        public static bool ExigirSesion(System.Web.UI.Page pagina)
        {
            if (HaySesion) return true;

            string destino = pagina.Request.AppRelativeCurrentExecutionFilePath;
            pagina.Response.Redirect("~/Default.aspx?returnUrl=" + HttpUtility.UrlEncode(destino));
            return false;
        }
    }
}
