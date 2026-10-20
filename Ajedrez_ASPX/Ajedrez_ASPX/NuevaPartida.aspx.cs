using System;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Diálogo de configuración de partida nueva (equivalente a NombresPartida
    /// del escritorio): nombres, color, modo soplar (damas) e IA (ajedrez).
    /// </summary>
    public partial class NuevaPartida : System.Web.UI.Page
    {
        private string Tipo
        {
            get
            {
                string t = ViewState["Tipo"] as string;
                if (string.IsNullOrWhiteSpace(t)) t = "damas";
                return t;
            }
            set { ViewState["Tipo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string q = Request.QueryString["tipo"];
                Tipo = (string.Equals(q, "ajedrez", StringComparison.OrdinalIgnoreCase)) ? "ajedrez" : "damas";

                lblTipo.Text = Tipo.ToUpper();
                txtLocal.Text = string.IsNullOrWhiteSpace(Sesion.NombreUsuario) ? "Jugador" : Sesion.NombreUsuario;
                txtLocal.Focus();

                // El modo de captura (soplar) solo aplica a las damas;
                // el modo IA/local solo al ajedrez (como en el escritorio).
                bool esAjedrez = Tipo == "ajedrez";
                pnlModoDamas.Visible = !esAjedrez;
                pnlModoAjedrez.Visible = esAjedrez;
                if (!esAjedrez) rbLocal.Checked = true;
            }
        }

        protected void Modo_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarNivel();
        }

        private void ActualizarNivel()
        {
            bool contraIA = rbIA.Checked;
            cmbNivel.Enabled = contraIA;

            if (contraIA)
            {
                txtOponente.Text = "Stockfish";
                txtOponente.ReadOnly = true;
            }
            else
            {
                if (txtOponente.Text == "Stockfish")
                    txtOponente.Text = "";
                txtOponente.ReadOnly = false;
            }
        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            string local = txtLocal.Text.Trim();
            string visitante = txtOponente.Text.Trim();

            if (local.Length == 0)
            {
                lblMensaje.Text = "Ingresa el nombre del jugador.";
                txtLocal.Focus();
                return;
            }

            if (visitante.Length == 0)
            {
                lblMensaje.Text = "Ingresa el nombre del oponente.";
                txtOponente.Focus();
                return;
            }

            bool esAjedrez = Tipo == "ajedrez";
            if (esAjedrez) ActualizarNivel();

            var cfg = new PartidaConfig
            {
                Tipo = Tipo,
                // El local elige color: si juega con blancas sus piezas van abajo.
                Blancas = rbBlancas.Checked ? local : visitante,
                Negras = rbBlancas.Checked ? visitante : local,
                Soplar = !esAjedrez && rbSoplar.Checked,
                ModoIA = esAjedrez && rbIA.Checked,
                NivelIA = esAjedrez && rbIA.Checked ? PartidaConfig.NivelDesdeIndice(int.Parse(cmbNivel.SelectedValue)) : 0
            };

            PartidaSesion.Actual = cfg;
            Response.Redirect(esAjedrez ? "Ajedrez.aspx" : "Damas.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            PartidaSesion.Limpiar();
            Response.Redirect("Menu.aspx");
        }
    }
}
