using System;

namespace Ajedrez_ASPX
{
    /// <summary>Menú principal (equivalente al formulario menu del escritorio).</summary>
    public partial class Menu : System.Web.UI.Page
    {
        // Tipo de partida pendiente de continuar/abrir ("ajedrez" o "damas").
        private string TipoPendiente
        {
            get { return ViewState["TipoPendiente"] as string ?? ""; }
            set { ViewState["TipoPendiente"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarUsuarioMenu();
            btnAdmin.Visible = Administradores.EsAdmin(Sesion.NombreUsuario);
        }

        // Recarga el nombre, la foto, el ELO y las victorias (como menu_Activated).
        private void CargarUsuarioMenu()
        {
            string nombreUsuario = Sesion.NombreUsuario;
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                lblUsuario.Text = "Invitado";
                imgUsuario.Visible = false;
                lblElo.Text = "0";
                lblVictorias.Text = "0";
                return;
            }

            lblUsuario.Text = nombreUsuario;

            // Las victorias se calculan desde las partidas, igual que en Partidas y Perfil.
            lblVictorias.Text = PartidaRepository.EstadisticasDe(nombreUsuario).Ganadas.ToString();

            var d = Nube.ObtenerDatosMenu(nombreUsuario);
            if (d != null)
            {
                lblElo.Text = d.elo.ToString("0");
                if (!string.IsNullOrWhiteSpace(d.imagen_url))
                {
                    imgUsuario.ImageUrl = d.imagen_url;
                    imgUsuario.Visible = true;
                }
                else
                {
                    imgUsuario.Visible = false;
                }
            }
        }

        // ---------- AJEDRÉS / DAMAS: continuar o empezar ----------

        protected void btnAjedrez_Click(object sender, EventArgs e)
        {
            PrepararPartida("ajedrez");
        }

        protected void btnDamas_Click(object sender, EventArgs e)
        {
            PrepararPartida("damas");
        }

        /// <summary>
        /// Gestiona una partida que quedó "en_curso" (igual que ContinuarODescartar
        /// del escritorio): si hay posición guardada se pregunta; si no, o si el
        /// jugador dice que no, se finaliza para no volver a preguntar.
        /// </summary>
        private void PrepararPartida(string tipo)
        {
            string usuario = Sesion.NombreUsuario;
            PartidaRepository.Partida enCurso = string.IsNullOrWhiteSpace(usuario)
                ? null
                : PartidaRepository.BuscarPartidaEnCurso(tipo, usuario);

            if (enCurso == null)
            {
                AbrirNuevaPartida(tipo);
                return;
            }

            PartidaRepository.Movimiento mov;
            if (!PartidaRepository.ObtenerMovimientoSeguro(enCurso.id_partida, out mov))
                return; // no se pudo consultar: se reintenta la próxima vez

            string estado = (mov != null && mov.tablero != null)
                ? mov.tablero.ToString(Newtonsoft.Json.Formatting.None) : null;

            // Sin posición guardada (o sin jugadas) no hay nada que continuar.
            if (string.IsNullOrWhiteSpace(estado) || mov == null || mov.num_movimientos <= 0)
            {
                PartidaRepository.FinalizarPartida(enCurso.id_partida, null);
                AbrirNuevaPartida(tipo);
                return;
            }

            TipoPendiente = tipo;
            pnlContinuarTexto.InnerText =
                "Tienes una partida de " + tipo + " sin terminar. ¿Deseas continuarla?\n\n" +
                "Sí = Continuar la partida guardada\n" +
                "No = Cerrarla (no se volverá a preguntar)";
            pnlContinuar.Visible = true;
            btnAjedrez.Enabled = false;
            btnDamas.Enabled = false;
        }

        protected void btnContinuarSi_Click(object sender, EventArgs e)
        {
            string tipo = TipoPendiente;
            string usuario = Sesion.NombreUsuario;
            PartidaRepository.Partida enCurso = PartidaRepository.BuscarPartidaEnCurso(tipo, usuario);
            pnlContinuar.Visible = false;
            btnAjedrez.Enabled = true;
            btnDamas.Enabled = true;
            if (enCurso == null) { Response.Redirect(tipo == "ajedrez" ? "Ajedrez.aspx" : "Damas.aspx"); return; }

            PartidaRepository.Movimiento mov;
            PartidaRepository.ObtenerMovimientoSeguro(enCurso.id_partida, out mov);
            string estado = (mov != null && mov.tablero != null)
                ? mov.tablero.ToString(Newtonsoft.Json.Formatting.None) : null;

            PartidaSesion.Actual = new PartidaConfig
            {
                Tipo = tipo,
                Blancas = enCurso.jugador_blancas,
                Negras = enCurso.jugador_negras,
                Soplar = enCurso.soplar,
                IdPartida = enCurso.id_partida,
                EstadoInicial = estado
            };
            Response.Redirect(tipo == "ajedrez" ? "Ajedrez.aspx" : "Damas.aspx");
        }

        protected void btnContinuarNo_Click(object sender, EventArgs e)
        {
            string tipo = TipoPendiente;
            string usuario = Sesion.NombreUsuario;
            PartidaRepository.Partida enCurso = PartidaRepository.BuscarPartidaEnCurso(tipo, usuario);
            if (enCurso != null)
                PartidaRepository.FinalizarPartida(enCurso.id_partida, null);

            pnlContinuar.Visible = false;
            btnAjedrez.Enabled = true;
            btnDamas.Enabled = true;
            AbrirNuevaPartida(tipo);
        }

        private void AbrirNuevaPartida(string tipo)
        {
            PartidaSesion.Limpiar();
            Response.Redirect("NuevaPartida.aspx?tipo=" + tipo);
        }

        // ---------- Navegación ----------

        protected void btnPerfil_Click(object sender, EventArgs e)
        {
            Response.Redirect("Perfil.aspx");
        }

        protected void btnPartidas_Click(object sender, EventArgs e)
        {
            Response.Redirect("Partidas.aspx");
        }

        protected void btnCreditos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Creditos.aspx");
        }

        protected void btnAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin.aspx");
        }

        protected void btnCambiarUsuario_Click(object sender, EventArgs e)
        {
            Sesion.CerrarSesion();
            Response.Redirect("Default.aspx");
        }
    }
}
