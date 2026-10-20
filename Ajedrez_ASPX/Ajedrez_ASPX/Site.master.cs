using System;

namespace Ajedrez_ASPX
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Aplicar();
        }

        protected void btnTemaClaro_Click(object sender, EventArgs e)
        {
            Sesion.TemaOscuro = false;
            Aplicar();
        }

        protected void btnTemaOscuro_Click(object sender, EventArgs e)
        {
            Sesion.TemaOscuro = true;
            Aplicar();
        }

        private void Aplicar()
        {
            bool oscuro = Sesion.TemaOscuro;
            cuerpo.Attributes["class"] = oscuro ? "tema-oscuro" : "tema-claro";
            btnTemaClaro.Visible = oscuro;
            btnTemaOscuro.Visible = !oscuro;
        }
    }
}
