using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Panel administrativo web (portado desde WinForms AdminPanel).
    /// </summary>
    public partial class Admin : System.Web.UI.Page
    {
        private List<UsuarioCompleto> _usuarios = new List<UsuarioCompleto>();
        private List<PartidaRepository.Partida> _partidas = new List<PartidaRepository.Partida>();
        private List<PartidaRepository.Movimiento> _movimientos = new List<PartidaRepository.Movimiento>();
        private bool _cargado;

        private class FilaUsuario
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string Contrasena { get; set; }
            public double ELO { get; set; }
            public int Jugadas { get; set; }
            public int Ganadas { get; set; }
            public int Empatadas { get; set; }
            public int Perdidas { get; set; }
            public bool Activo { get; set; }
        }

        private class FilaPartida
        {
            public int Id { get; set; }
            public string Blancas { get; set; }
            public string Negras { get; set; }
            public string Ganador { get; set; }
            public string Estado { get; set; }
            public string Tipo { get; set; }
            public string Inicio { get; set; }
            public string Fin { get; set; }
        }

        private class FilaMovimiento
        {
            public int Id { get; set; }
            public int IdPartida { get; set; }
            public int NumMovimientos { get; set; }
            public string TurnoFinal { get; set; }
            public string MovimientoFinal { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Administradores.EsAdmin(Sesion.NombreUsuario))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "noadmin",
                        "alert('No tienes permisos de administrador.'); location='Menu.aspx';", true);
                    return;
                }

                CargarTodo();
                _cargado = true;
                ActualizarPestanas();
            }
            else
            {
                if (!Administradores.EsAdmin(Sesion.NombreUsuario))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "noadmin",
                        "alert('No tienes permisos de administrador.'); location='Menu.aspx';", true);
                    return;
                }
            }
        }

        private void CargarTodo()
        {
            _partidas = Nube.ListarTodasPartidas() ?? new List<PartidaRepository.Partida>();
            _usuarios = Nube.ListarUsuarios() ?? new List<UsuarioCompleto>();
            _movimientos = Nube.ListarTodosMovimientos() ?? new List<PartidaRepository.Movimiento>();

            InicializarFiltrosPartidas();
            AplicarFiltroPartidas();
            CargarUsuariosGrid();
            CargarMovimientosGrid();
        }

        private void CargarUsuariosGrid()
        {
            dgvUsuarios.DataSource = _usuarios.Select(u =>
            {
                int jugadas, ganadas, empatadas, perdidas;
                if (_partidas != null && _partidas.Count > 0)
                {
                    CalcularEstadisticas(u.nombre_usuario, _partidas,
                        out jugadas, out ganadas, out empatadas, out perdidas);
                }
                else
                {
                    jugadas = u.partidas_jugadas;
                    ganadas = u.partidas_ganadas;
                    empatadas = u.partidas_empatadas;
                    perdidas = u.partidas_perdidas;
                }
                return new FilaUsuario
                {
                    Id = u.id_usuario,
                    Nombre = u.nombre_usuario,
                    Email = u.email,
                    Contrasena = u.contrasena,
                    ELO = u.elo,
                    Jugadas = jugadas,
                    Ganadas = ganadas,
                    Empatadas = empatadas,
                    Perdidas = perdidas,
                    Activo = u.activo
                };
            }).ToList();
            dgvUsuarios.DataBind();
        }

        private void CargarMovimientosGrid()
        {
            dgvMovimientos.DataSource = _movimientos.Select(m => new FilaMovimiento
            {
                Id = m.id_movimiento,
                IdPartida = m.id_partida,
                NumMovimientos = m.num_movimientos,
                TurnoFinal = m.turno_final,
                MovimientoFinal = m.movimiento_final
            }).ToList();
            dgvMovimientos.DataBind();
        }

        private void InicializarFiltrosPartidas()
        {
            string actual = cmbPartidasTipo.SelectedValue;
            var tipos = _partidas
                .Select(p => p.tipo_juego)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(t => t)
                .ToList();

            cmbPartidasTipo.Items.Clear();
            cmbPartidasTipo.Items.Add(new ListItem("Todos", "Todos"));
            foreach (string t in tipos)
            {
                cmbPartidasTipo.Items.Add(new ListItem(t, t));
            }
            cmbPartidasTipo.SelectedValue = !string.IsNullOrEmpty(actual) && cmbPartidasTipo.Items.FindByValue(actual) != null ? actual : "Todos";
        }

        private void AplicarFiltroPartidas()
        {
            IEnumerable<PartidaRepository.Partida> q = _partidas;

            string jugador = (txtPartidasJugador.Text ?? string.Empty).Trim();
            if (jugador.Length > 0)
            {
                q = q.Where(p => Contiene(p.jugador_blancas, jugador) || Contiene(p.jugador_negras, jugador));
            }

            string resultado = cmbPartidasResultado.SelectedValue;
            if (!string.IsNullOrEmpty(resultado) && resultado != "Todos")
            {
                q = q.Where(p => CoincideResultado(p, resultado));
            }

            string tipo = cmbPartidasTipo.SelectedValue;
            if (!string.IsNullOrEmpty(tipo) && tipo != "Todos")
            {
                q = q.Where(p => NombreIgual(p.tipo_juego, tipo));
            }

            string desdeStr = txtPartidasDesde.Text;
            if (!string.IsNullOrEmpty(desdeStr) && DateTime.TryParse(desdeStr, out DateTime desde))
            {
                q = q.Where(p =>
                {
                    DateTime? f = FechaReferencia(p);
                    return f.HasValue && f.Value.Date >= desde.Date;
                });
            }

            string hastaStr = txtPartidasHasta.Text;
            if (!string.IsNullOrEmpty(hastaStr) && DateTime.TryParse(hastaStr, out DateTime hasta))
            {
                q = q.Where(p =>
                {
                    DateTime? f = FechaReferencia(p);
                    return f.HasValue && f.Value.Date <= hasta.Date;
                });
            }

            var lista = q.ToList();

            dgvPartidas.DataSource = lista.Select(p => new FilaPartida
            {
                Id = p.id_partida,
                Blancas = p.jugador_blancas,
                Negras = p.jugador_negras,
                Ganador = p.ganador,
                Estado = p.estado,
                Tipo = p.tipo_juego,
                Inicio = p.fecha_inicio.HasValue ? p.fecha_inicio.Value.ToString("dd/MM/yyyy HH:mm") : "",
                Fin = p.fecha_fin.HasValue ? p.fecha_fin.Value.ToString("dd/MM/yyyy HH:mm") : ""
            }).ToList();
            dgvPartidas.DataBind();

            MostrarStatsPartidas(lista, jugador);
        }

        private void MostrarStatsPartidas(List<PartidaRepository.Partida> lista, string jugador)
        {
            int total = lista.Count;
            int finalizadas = lista.Count(EsFinalizada);
            int gananBlancas = lista.Count(p => EsFinalizada(p) && NombreIgual(p.ganador, p.jugador_blancas));
            int gananNegras = lista.Count(p => EsFinalizada(p) && NombreIgual(p.ganador, p.jugador_negras));
            int empates = lista.Count(p => EsFinalizada(p) && EsEmpateGanador(p));
            int enCurso = total - finalizadas;

            string linea1 = string.Format(
                "TOTAL: {0} · Blancas ganan: {1} · Negras ganan: {2} · Empates: {3} · En curso: {4}",
                total, gananBlancas, gananNegras, empates, enCurso);

            string linea2;
            if (jugador.Length > 0)
            {
                int ganadas = lista.Count(p => EsVictoria(p, jugador));
                int perdidas = lista.Count(p => EsDerrota(p, jugador));
                int empatadas = lista.Count(p => EsFinalizada(p) && !EsVictoria(p, jugador) && !EsDerrota(p, jugador));
                linea2 = string.Format(
                    "{0} → Ganadas: {1} · Empatadas: {2} · Perdidas: {3} (finalizadas: {4})",
                    jugador, ganadas, empatadas, perdidas, ganadas + empatadas + perdidas);
            }
            else
            {
                linea2 = "Escribe un jugador arriba para ver sus GANADAS / EMPATADAS / PERDIDAS.";
            }

            lblPartidasStats.Text = linea1 + "<br/>" + linea2;
        }

        private static bool EsFinalizada(PartidaRepository.Partida p)
        {
            return string.Equals(p.estado, "finalizada", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EsEmpateGanador(PartidaRepository.Partida p)
        {
            return string.Equals(p.ganador, "empate", StringComparison.OrdinalIgnoreCase);
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
                   && !EsEmpateGanador(p)
                   && !string.Equals(p.ganador, usuario, StringComparison.OrdinalIgnoreCase);
        }

        private static bool CoincideResultado(PartidaRepository.Partida p, string resultado)
        {
            bool finalizada = EsFinalizada(p);
            switch (resultado)
            {
                case "Ganan blancas": return finalizada && NombreIgual(p.ganador, p.jugador_blancas);
                case "Ganan negras": return finalizada && NombreIgual(p.ganador, p.jugador_negras);
                case "Empate": return finalizada && EsEmpateGanador(p);
                case "En curso": return !finalizada;
                default: return true;
            }
        }

        private static DateTime? FechaReferencia(PartidaRepository.Partida p)
        {
            return p.fecha_inicio ?? p.fecha_fin;
        }

        private static bool NombreIgual(string a, string b)
        {
            return !string.IsNullOrWhiteSpace(a)
                   && string.Equals(a.Trim(), b, StringComparison.OrdinalIgnoreCase);
        }

        private static bool Contiene(string texto, string busqueda)
        {
            return !string.IsNullOrWhiteSpace(texto)
                   && texto.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void CalcularEstadisticas(string nombre, List<PartidaRepository.Partida> partidas,
            out int jugadas, out int ganadas, out int empatadas, out int perdidas)
        {
            jugadas = ganadas = empatadas = perdidas = 0;
            if (string.IsNullOrWhiteSpace(nombre) || partidas == null) return;

            foreach (PartidaRepository.Partida p in partidas)
            {
                if (!Participa(p, nombre)) continue;
                if (!EsFinalizada(p)) continue;

                jugadas++;
                if (EsVictoria(p, nombre)) ganadas++;
                else if (EsDerrota(p, nombre)) perdidas++;
                else empatadas++;
            }
        }

        private static bool Participa(PartidaRepository.Partida p, string nombre)
        {
            return NombreIgual(p.jugador_blancas, nombre) || NombreIgual(p.jugador_negras, nombre);
        }

        protected void btnTabPartidas_Click(object sender, EventArgs e)
        {
            mvTabs.ActiveViewIndex = 0;
            ActualizarPestanas();
            _partidas = Nube.ListarTodasPartidas() ?? _partidas;
            InicializarFiltrosPartidas();
            AplicarFiltroPartidas();
        }

        protected void btnTabUsuarios_Click(object sender, EventArgs e)
        {
            mvTabs.ActiveViewIndex = 1;
            ActualizarPestanas();
            _partidas = Nube.ListarTodasPartidas() ?? _partidas;
            _usuarios = Nube.ListarUsuarios() ?? _usuarios;
            CargarUsuariosGrid();
        }

        protected void btnTabMovimientos_Click(object sender, EventArgs e)
        {
            mvTabs.ActiveViewIndex = 2;
            ActualizarPestanas();
            _movimientos = Nube.ListarTodosMovimientos() ?? _movimientos;
            CargarMovimientosGrid();
        }

        private void ActualizarPestanas()
        {
            btnTabPartidas.CssClass = mvTabs.ActiveViewIndex == 0 ? "pestana activa" : "pestana";
            btnTabUsuarios.CssClass = mvTabs.ActiveViewIndex == 1 ? "pestana activa" : "pestana";
            btnTabMovimientos.CssClass = mvTabs.ActiveViewIndex == 2 ? "pestana activa" : "pestana";
        }

        protected void FiltroPartidas_Changed(object sender, EventArgs e)
        {
            AplicarFiltroPartidas();
        }

        protected void btnPartidasLimpiar_Click(object sender, EventArgs e)
        {
            txtPartidasJugador.Text = string.Empty;
            cmbPartidasResultado.SelectedValue = "Todos";
            cmbPartidasTipo.SelectedValue = "Todos";
            txtPartidasDesde.Text = string.Empty;
            txtPartidasHasta.Text = string.Empty;
            AplicarFiltroPartidas();
        }

        protected void btnPartidasRefrescar_Click(object sender, EventArgs e)
        {
            _partidas = Nube.ListarTodasPartidas() ?? new List<PartidaRepository.Partida>();
            InicializarFiltrosPartidas();
            AplicarFiltroPartidas();
        }

        protected void btnPartidasEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPartidas.SelectedIndex < 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "selpar", "alert('Selecciona una partida para eliminar.');", true);
                return;
            }
            int id = int.Parse(dgvPartidas.SelectedDataKey.Value.ToString());
            if (ClientScript != null)
            {
                // not used
            }
            // confirm via JS-like behavior: use return confirm in client? easier to just do server-side confirm via JS
            ClientScript.RegisterStartupScript(this.GetType(), "confpar", string.Format("if(confirm('¿Eliminar la partida y sus movimientos? Esta acción no se puede deshacer.')) {{ __doPostBack('{0}', 'delpar_{1}'); }}", btnPartidasEliminar.UniqueID, id), true);
        }

        protected void btnUsuariosRefrescar_Click(object sender, EventArgs e)
        {
            _partidas = Nube.ListarTodasPartidas() ?? _partidas;
            _usuarios = Nube.ListarUsuarios() ?? _usuarios;
            CargarUsuariosGrid();
        }

        private bool _esNuevoUsuario;
        private int _usuarioEditId;

        protected void btnUsuariosCrear_Click(object sender, EventArgs e)
        {
            _esNuevoUsuario = true;
            lblUsuarioTitulo.Text = "Nuevo usuario";
            txtUsuarioNombre.Text = string.Empty;
            txtUsuarioEmail.Text = string.Empty;
            txtUsuarioContrasena.Text = string.Empty;
            txtUsuarioElo.Text = "800";
            txtUsuarioJugadas.Text = "0";
            txtUsuarioGanadas.Text = "0";
            txtUsuarioEmpatadas.Text = "0";
            txtUsuarioPerdidas.Text = "0";
            chkUsuarioActivo.Checked = true;
            txtUsuarioContrasena.TextMode = TextBoxMode.Password;
            chkUsuarioVer.Checked = false;
            pnlUsuario.Visible = true;
        }

        protected void btnUsuariosEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedIndex < 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "selu", "alert('Selecciona un usuario para editar.');", true);
                return;
            }
            int id = int.Parse(dgvUsuarios.SelectedDataKey.Value.ToString());
            UsuarioCompleto original = _usuarios.FirstOrDefault(u => u.id_usuario == id);
            if (original == null) return;
            _esNuevoUsuario = false;
            _usuarioEditId = id;
            lblUsuarioTitulo.Text = "Editar usuario";
            int jugadas = original.partidas_jugadas, ganadas = original.partidas_ganadas, empatadas = original.partidas_empatadas, perdidas = original.partidas_perdidas;
            if (_partidas != null && _partidas.Count > 0)
            {
                CalcularEstadisticas(original.nombre_usuario, _partidas, out jugadas, out ganadas, out empatadas, out perdidas);
            }
            txtUsuarioNombre.Text = original.nombre_usuario ?? "";
            txtUsuarioEmail.Text = original.email ?? "";
            txtUsuarioContrasena.Text = original.contrasena ?? "";
            txtUsuarioElo.Text = original.elo.ToString("0");
            txtUsuarioJugadas.Text = jugadas.ToString();
            txtUsuarioGanadas.Text = ganadas.ToString();
            txtUsuarioEmpatadas.Text = empatadas.ToString();
            txtUsuarioPerdidas.Text = perdidas.ToString();
            chkUsuarioActivo.Checked = original.activo;
            txtUsuarioContrasena.TextMode = TextBoxMode.Password;
            chkUsuarioVer.Checked = false;
            pnlUsuario.Visible = true;
        }

        protected void btnUsuariosEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedIndex < 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "selue", "alert('Selecciona un usuario para eliminar.');", true);
                return;
            }
            int id = int.Parse(dgvUsuarios.SelectedDataKey.Value.ToString());
            ClientScript.RegisterStartupScript(this.GetType(), "confeu", string.Format("if(confirm('¿Eliminar el usuario seleccionado? Esta acción no se puede deshacer.')) {{ __doPostBack('{0}', 'delusr_{1}'); }}", btnUsuariosEliminar.UniqueID, id), true);
        }

        private int _resetUserId;

        protected void btnUsuariosReset_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedIndex < 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "selur", "alert('Selecciona un usuario para resetear su contraseña.');", true);
                return;
            }
            _resetUserId = int.Parse(dgvUsuarios.SelectedDataKey.Value.ToString());
            txtNuevaContrasena.Text = string.Empty;
            pnlReset.Visible = true;
        }

        protected void btnMovimientosRefrescar_Click(object sender, EventArgs e)
        {
            _movimientos = Nube.ListarTodosMovimientos() ?? _movimientos;
            CargarMovimientosGrid();
        }

        protected void btnMovimientosEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMovimientos.SelectedIndex < 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "selmv", "alert('Selecciona un movimiento para eliminar.');", true);
                return;
            }
            int id = int.Parse(dgvMovimientos.SelectedDataKey.Value.ToString());
            ClientScript.RegisterStartupScript(this.GetType(), "confmv", string.Format("if(confirm('¿Eliminar el movimiento seleccionado?')) {{ __doPostBack('{0}', 'delmov_{1}'); }}", btnMovimientosEliminar.UniqueID, id), true);
        }

        protected void ir_menu_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void btnSincronizar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarTodo();
                ClientScript.RegisterStartupScript(this.GetType(), "syncok", "alert('Datos sincronizados con la nube.');", true);
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "syncerr", "alert('No se pudo sincronizar (¿sin conexión?).');", true);
            }
        }

        protected void btnResetAceptar_Click(object sender, EventArgs e)
        {
            string nueva = txtNuevaContrasena.Text ?? string.Empty;
            if (string.IsNullOrEmpty(nueva))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "resv", "alert('La contraseña no puede estar vacía.');", true);
                pnlReset.Visible = true;
                return;
            }
            bool ok = Nube.ResetearContrasena(_resetUserId, nueva);
            pnlReset.Visible = false;
            if (!ok)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "resf", "alert('No se pudo cambiar la contraseña (¿sin conexión?).');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ress", "alert('Contraseña actualizada.');", true);
                _usuarios = Nube.ListarUsuarios() ?? _usuarios;
                CargarUsuariosGrid();
            }
        }

        protected void btnResetCancelar_Click(object sender, EventArgs e)
        {
            pnlReset.Visible = false;
        }

        protected void chkUsuarioVer_CheckedChanged(object sender, EventArgs e)
        {
            txtUsuarioContrasena.TextMode = chkUsuarioVer.Checked ? TextBoxMode.SingleLine : TextBoxMode.Password;
            pnlUsuario.Visible = true;
        }

        protected void btnUsuarioCancelar_Click(object sender, EventArgs e)
        {
            pnlUsuario.Visible = false;
        }

        private static double ParsearD(string texto, double porDefecto)
        {
            double v;
            if (double.TryParse(texto, out v)) return v;
            return porDefecto;
        }

        private static int ParsearI(string texto, int porDefecto)
        {
            int v;
            if (int.TryParse(texto, out v)) return v;
            return porDefecto;
        }

        protected void btnUsuarioGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuarioNombre.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "unom", "alert('El nombre de usuario es obligatorio.');", true);
                pnlUsuario.Visible = true;
                return;
            }
            if (_esNuevoUsuario && string.IsNullOrEmpty(txtUsuarioContrasena.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "upass", "alert('La contraseña es obligatoria para un usuario nuevo.');", true);
                pnlUsuario.Visible = true;
                return;
            }

            UsuarioCompleto usr = new UsuarioCompleto();
            if (!_esNuevoUsuario)
            {
                UsuarioCompleto original = _usuarios.FirstOrDefault(u => u.id_usuario == _usuarioEditId);
                if (original != null)
                {
                    usr.id_usuario = original.id_usuario;
                    usr.fecha_registro = original.fecha_registro;
                    usr.imagen_url = original.imagen_url;
                }
                else
                {
                    usr.id_usuario = _usuarioEditId;
                }
            }
            usr.nombre_usuario = txtUsuarioNombre.Text.Trim();
            usr.email = string.IsNullOrWhiteSpace(txtUsuarioEmail.Text) ? null : txtUsuarioEmail.Text.Trim();
            usr.contrasena = txtUsuarioContrasena.Text;
            usr.elo = ParsearD(txtUsuarioElo.Text, 800.0);
            usr.partidas_jugadas = ParsearI(txtUsuarioJugadas.Text, 0);
            usr.partidas_ganadas = ParsearI(txtUsuarioGanadas.Text, 0);
            usr.partidas_empatadas = ParsearI(txtUsuarioEmpatadas.Text, 0);
            usr.partidas_perdidas = ParsearI(txtUsuarioPerdidas.Text, 0);
            usr.activo = chkUsuarioActivo.Checked;

            bool ok;
            if (_esNuevoUsuario)
            {
                ok = Nube.CrearUsuario(usr);
                if (!ok)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ucre", "alert('No se pudo crear el usuario (¿ya existe o no hay conexión?).');", true);
                    pnlUsuario.Visible = true;
                    return;
                }
            }
            else
            {
                ok = Nube.ActualizarUsuarioCompleto(usr);
                if (!ok)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "uupd", "alert('No se pudo actualizar el usuario (¿sin conexión?).');", true);
                    pnlUsuario.Visible = true;
                    return;
                }
            }

            pnlUsuario.Visible = false;
            _usuarios = Nube.ListarUsuarios() ?? _usuarios;
            _partidas = Nube.ListarTodasPartidas() ?? _partidas;
            CargarUsuariosGrid();
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler source, string eventArgument)
        {
            base.RaisePostBackEvent(source, eventArgument);
            if (eventArgument != null && eventArgument.StartsWith("delpar_"))
            {
                int id = int.Parse(eventArgument.Substring("delpar_".Length));
                bool ok = Nube.EliminarPartida(id);
                if (!ok)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "dpf", "alert('No se pudo eliminar la partida (¿sin conexión?).');", true);
                    return;
                }
                _partidas = Nube.ListarTodasPartidas() ?? _partidas;
                _usuarios = Nube.ListarUsuarios() ?? _usuarios;
                _movimientos = Nube.ListarTodosMovimientos() ?? _movimientos;
                InicializarFiltrosPartidas();
                AplicarFiltroPartidas();
                CargarUsuariosGrid();
                CargarMovimientosGrid();
            }
            else if (eventArgument != null && eventArgument.StartsWith("delusr_"))
            {
                int id = int.Parse(eventArgument.Substring("delusr_".Length));
                bool ok = Nube.EliminarUsuario(id);
                if (!ok)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "duf", "alert('No se pudo eliminar el usuario (¿sin conexión?).');", true);
                    return;
                }
                _usuarios = Nube.ListarUsuarios() ?? _usuarios;
                CargarUsuariosGrid();
            }
            else if (eventArgument != null && eventArgument.StartsWith("delmov_"))
            {
                int id = int.Parse(eventArgument.Substring("delmov_".Length));
                bool ok = Nube.EliminarMovimiento(id);
                if (!ok)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "dmf", "alert('No se pudo eliminar el movimiento (¿sin conexión?).');", true);
                    return;
                }
                _movimientos = Nube.ListarTodosMovimientos() ?? _movimientos;
                CargarMovimientosGrid();
            }
        }
    }
}