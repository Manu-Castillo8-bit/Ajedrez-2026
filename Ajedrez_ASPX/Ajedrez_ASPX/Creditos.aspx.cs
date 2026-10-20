using System;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Créditos / agradecimientos (equivalente a la ventana Créditos del escritorio).
    /// </summary>
    public partial class Creditos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Créditos";

            // Mismo texto que la etiqueta lblCuerpo de la ventana del escritorio.
            lblCuerpo.Text =
                "A Jonathan Palacios, por su valiosa ayuda y consejería en el ajedrez.\r\n\r\n" +
                "Al profe Arcides.\r\n\r\n" +
                "Y a cada maestro, por compartirnos un poco de sus conocimientos.\r\n\r\n" +
                "¡Muchas gracias!";
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            // El escritorio cierra el diálogo y vuelve al menú.
            Response.Redirect("Menu.aspx");
        }
    }
}
