using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajedrez
{
    /// <summary>
    /// Panel administrativo: gestiona los usuarios, partidas y movimientos que
    /// hay en Supabase (la nube), donde están TODOS los datos del sistema.
    /// Solo entra aquí un administrador (ver <see cref="Administradores"/>).
    /// </summary>
    public partial class AdminPanel : Form
    {
        private List<UsuarioCompleto> _usuarios = new List<UsuarioCompleto>();
        private List<PartidaRepository.Partida> _partidas = new List<PartidaRepository.Partida>();
        private List<PartidaRepository.Movimiento> _movimientos = new List<PartidaRepository.Movimiento>();
        private bool _cargado;
        private bool _filtrosPartidasListos;

        public AdminPanel()
        {
            InitializeComponent();
        }

        // ── Filas que se muestran en las tablas ──

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

        // ── Carga ──

        private async void AdminPanel_Load(object sender, EventArgs e)
        {
            if (!Administradores.EsAdmin(Sesion.NombreUsuario))
            {
                MessageBox.Show("No tienes permisos de administrador.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BeginInvoke(new Action(Close));
                return;
            }

            Tema.ApplyTheme(this);

            await CargarPartidasAsync();
            await CargarUsuariosAsync();
            await CargarMovimientosAsync();
            _cargado = true;
        }

        private async Task CargarUsuariosAsync()
        {
            btnUsuariosRefrescar.Enabled = false;
            var lista = await Task.Run(() => Nube.ListarUsuarios());
            _usuarios = lista ?? new List<UsuarioCompleto>();
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
            btnUsuariosRefrescar.Enabled = true;
        }

        private async Task CargarPartidasAsync()
        {
            btnPartidasRefrescar.Enabled = false;
            var lista = await Task.Run(() => Nube.ListarTodasPartidas());
            _partidas = lista ?? new List<PartidaRepository.Partida>();
            InicializarFiltrosPartidas();
            AplicarFiltroPartidas();
            btnPartidasRefrescar.Enabled = true;
        }

        private async Task CargarMovimientosAsync()
        {
            btnMovimientosRefrescar.Enabled = false;
            var lista = await Task.Run(() => Nube.ListarTodosMovimientos());
            _movimientos = lista ?? new List<PartidaRepository.Movimiento>();
            dgvMovimientos.DataSource = _movimientos.Select(m => new FilaMovimiento
            {
                Id = m.id_movimiento,
                IdPartida = m.id_partida,
                NumMovimientos = m.num_movimientos,
                TurnoFinal = m.turno_final,
                MovimientoFinal = m.movimiento_final
            }).ToList();
            btnMovimientosRefrescar.Enabled = true;
        }

        // ── Usuarios ──

        private async void btnUsuariosRefrescar_Click(object sender, EventArgs e)
        {
            await CargarPartidasAsync();
            await CargarUsuariosAsync();
        }

        private async void btnUsuariosCrear_Click(object sender, EventArgs e)
        {
            using (UsuarioEditor dlg = new UsuarioEditor(null))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                bool ok = await Task.Run(() => Nube.CrearUsuario(dlg.Usuario));
                if (!ok)
                {
                    Aviso("No se pudo crear el usuario (¿ya existe o no hay conexión?).");
                    return;
                }
            }
            await CargarUsuariosAsync();
        }

        private async void btnUsuariosEditar_Click(object sender, EventArgs e)
        {
            int? id = IdSeleccionado(dgvUsuarios);
            if (id == null) { Aviso("Selecciona un usuario para editar."); return; }

            UsuarioCompleto original = _usuarios.FirstOrDefault(u => u.id_usuario == id.Value);
            if (original == null) return;

            // Estadísticas reales calculadas de las partidas (las mismas que
            // muestra la tabla); los valores guardados en la nube pueden estar
            // desactualizados.
            int jugadas = original.partidas_jugadas;
            int ganadas = original.partidas_ganadas;
            int empatadas = original.partidas_empatadas;
            int perdidas = original.partidas_perdidas;
            if (_partidas != null && _partidas.Count > 0)
            {
                CalcularEstadisticas(original.nombre_usuario, _partidas,
                    out jugadas, out ganadas, out empatadas, out perdidas);
            }

            // Copia para no dejar cambios a medias si el usuario cancela.
            UsuarioCompleto copia = new UsuarioCompleto
            {
                id_usuario = original.id_usuario,
                nombre_usuario = original.nombre_usuario,
                email = original.email,
                contrasena = original.contrasena,
                fecha_registro = original.fecha_registro,
                elo = original.elo,
                partidas_jugadas = jugadas,
                partidas_ganadas = ganadas,
                partidas_empatadas = empatadas,
                partidas_perdidas = perdidas,
                activo = original.activo,
                imagen_url = original.imagen_url
            };

            using (UsuarioEditor dlg = new UsuarioEditor(copia))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                bool ok = await Task.Run(() => Nube.ActualizarUsuarioCompleto(dlg.Usuario));
                if (!ok)
                {
                    Aviso("No se pudo actualizar el usuario (¿sin conexión?).");
                    return;
                }
            }
            await CargarUsuariosAsync();
        }

        private async void btnUsuariosEliminar_Click(object sender, EventArgs e)
        {
            int? id = IdSeleccionado(dgvUsuarios);
            if (id == null) { Aviso("Selecciona un usuario para eliminar."); return; }

            if (MessageBox.Show("¿Eliminar el usuario seleccionado? Esta acción no se puede deshacer.",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            bool ok = await Task.Run(() => Nube.EliminarUsuario(id.Value));
            if (!ok) { Aviso("No se pudo eliminar el usuario (¿sin conexión?)."); return; }
            await CargarUsuariosAsync();
        }

        private async void btnUsuariosReset_Click(object sender, EventArgs e)
        {
            int? id = IdSeleccionado(dgvUsuarios);
            if (id == null) { Aviso("Selecciona un usuario para resetear su contraseña."); return; }

            string nueva = PedirTexto("Resetear contraseña", "Nueva contraseña:");
            if (nueva == null) return;
            if (string.IsNullOrEmpty(nueva)) { Aviso("La contraseña no puede estar vacía."); return; }

            bool ok = await Task.Run(() => Nube.ResetearContrasena(id.Value, nueva));
            if (!ok) { Aviso("No se pudo cambiar la contraseña (¿sin conexión?)."); return; }
            MessageBox.Show("Contraseña actualizada.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await CargarUsuariosAsync();
        }

        // ── Partidas ──

        private async void btnPartidasRefrescar_Click(object sender, EventArgs e)
        {
            await CargarPartidasAsync();
        }

        private async void btnPartidasEliminar_Click(object sender, EventArgs e)
        {
            int? id = IdSeleccionado(dgvPartidas);
            if (id == null) { Aviso("Selecciona una partida para eliminar."); return; }

            if (MessageBox.Show("¿Eliminar la partida y sus movimientos? Esta acción no se puede deshacer.",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            bool ok = await Task.Run(() => Nube.EliminarPartida(id.Value));
            if (!ok) { Aviso("No se pudo eliminar la partida (¿sin conexión?)."); return; }

            await CargarPartidasAsync();
            await CargarUsuariosAsync();
            await CargarMovimientosAsync();
        }

        // ── Filtros y estadísticas de partidas ──

        private void InicializarFiltrosPartidas()
        {
            if (!_filtrosPartidasListos)
            {
                cmbPartidasResultado.Items.Clear();
                cmbPartidasResultado.Items.AddRange(new object[]
                {
                    "Todos", "Ganan blancas", "Ganan negras", "Empate", "En curso"
                });
                cmbPartidasResultado.SelectedIndex = 0;
                dtpPartidasDesde.Checked = false;
                dtpPartidasHasta.Checked = false;
                _filtrosPartidasListos = true;
            }

            string actual = cmbPartidasTipo.SelectedItem as string;
            var tipos = _partidas
                .Select(p => p.tipo_juego)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(t => t)
                .ToList();

            cmbPartidasTipo.Items.Clear();
            cmbPartidasTipo.Items.Add("Todos");
            foreach (string t in tipos) cmbPartidasTipo.Items.Add(t);
            int idx = actual != null ? cmbPartidasTipo.Items.IndexOf(actual) : -1;
            cmbPartidasTipo.SelectedIndex = idx >= 0 ? idx : 0;
        }

        private void FiltroPartidas_Changed(object sender, EventArgs e)
        {
            AplicarFiltroPartidas();
        }

        private void btnPartidasLimpiar_Click(object sender, EventArgs e)
        {
            txtPartidasJugador.Text = string.Empty;
            if (cmbPartidasResultado.Items.Count > 0) cmbPartidasResultado.SelectedIndex = 0;
            if (cmbPartidasTipo.Items.Count > 0) cmbPartidasTipo.SelectedIndex = 0;
            dtpPartidasDesde.Checked = false;
            dtpPartidasHasta.Checked = false;
            AplicarFiltroPartidas();
        }

        private void AplicarFiltroPartidas()
        {
            if (dgvPartidas == null || cmbPartidasResultado == null) return;

            IEnumerable<PartidaRepository.Partida> q = _partidas;

            string jugador = txtPartidasJugador != null
                ? (txtPartidasJugador.Text ?? string.Empty).Trim() : string.Empty;
            if (jugador.Length > 0)
                q = q.Where(p => Contiene(p.jugador_blancas, jugador)
                              || Contiene(p.jugador_negras, jugador));

            string resultado = cmbPartidasResultado.SelectedItem as string;
            if (!string.IsNullOrEmpty(resultado) && resultado != "Todos")
                q = q.Where(p => CoincideResultado(p, resultado));

            string tipo = cmbPartidasTipo != null ? cmbPartidasTipo.SelectedItem as string : null;
            if (!string.IsNullOrEmpty(tipo) && tipo != "Todos")
                q = q.Where(p => NombreIgual(p.tipo_juego, tipo));

            if (dtpPartidasDesde != null && dtpPartidasDesde.Checked)
            {
                DateTime desde = dtpPartidasDesde.Value.Date;
                q = q.Where(p =>
                {
                    DateTime? f = FechaReferencia(p);
                    return f.HasValue && f.Value.Date >= desde;
                });
            }
            if (dtpPartidasHasta != null && dtpPartidasHasta.Checked)
            {
                DateTime hasta = dtpPartidasHasta.Value.Date;
                q = q.Where(p =>
                {
                    DateTime? f = FechaReferencia(p);
                    return f.HasValue && f.Value.Date <= hasta;
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

            MostrarStatsPartidas(lista, jugador);
        }

        private void MostrarStatsPartidas(List<PartidaRepository.Partida> lista, string jugador)
        {
            if (lblPartidasStats == null) return;

            int total = lista.Count;
            int finalizadas = lista.Count(EsFinalizada);
            int gananBlancas = lista.Count(p => EsFinalizada(p) && NombreIgual(p.ganador, p.jugador_blancas));
            int gananNegras = lista.Count(p => EsFinalizada(p) && NombreIgual(p.ganador, p.jugador_negras));
            int empates = lista.Count(p => EsFinalizada(p) && EsEmpateGanador(p));
            int enCurso = total - finalizadas;

            string linea1 = string.Format(
                "TOTAL: {0}   ·   Blancas ganan: {1}   ·   Negras ganan: {2}   ·   Empates: {3}   ·   En curso: {4}",
                total, gananBlancas, gananNegras, empates, enCurso);

            string linea2;
            if (jugador.Length > 0)
            {
                int ganadas = lista.Count(p => EsVictoria(p, jugador));
                int perdidas = lista.Count(p => EsDerrota(p, jugador));
                int empatadas = lista.Count(p => EsFinalizada(p)
                                                 && !EsVictoria(p, jugador)
                                                 && !EsDerrota(p, jugador));
                linea2 = string.Format(
                    "{0}  →  Ganadas: {1}   ·   Empatadas: {2}   ·   Perdidas: {3}   (finalizadas: {4})",
                    jugador, ganadas, empatadas, perdidas, ganadas + empatadas + perdidas);
            }
            else
            {
                linea2 = "Escribe un jugador arriba para ver sus GANADAS / EMPATADAS / PERDIDAS.";
            }

            lblPartidasStats.Text = linea1 + Environment.NewLine + linea2;
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

        // ── Movimientos ──

        private async void btnMovimientosRefrescar_Click(object sender, EventArgs e)
        {
            await CargarMovimientosAsync();
        }

        private async void btnMovimientosEliminar_Click(object sender, EventArgs e)
        {
            int? id = IdSeleccionado(dgvMovimientos);
            if (id == null) { Aviso("Selecciona un movimiento para eliminar."); return; }

            if (MessageBox.Show("¿Eliminar el movimiento seleccionado?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            bool ok = await Task.Run(() => Nube.EliminarMovimiento(id.Value));
            if (!ok) { Aviso("No se pudo eliminar el movimiento (¿sin conexión?)."); return; }
            await CargarMovimientosAsync();
        }

        // ── Navegación y utilidades ──

        private void ir_menu_Click(object sender, EventArgs e)
        {
            menu f_menu = new menu();
            Tema.ApplyTheme(f_menu);
            f_menu.Show();
            this.Hide();
        }

        private async void btnSincronizar_Click(object sender, EventArgs e)
        {
            btnSincronizar.Enabled = false;
            try
            {
                await Task.Run(() => Sincronizador.Sincronizar(Sesion.NombreUsuario));
                await CargarPartidasAsync();
                await CargarUsuariosAsync();
                await CargarMovimientosAsync();
                MessageBox.Show("Datos sincronizados con la nube.", "Sincronizado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                Aviso("No se pudo sincronizar (¿sin conexión?).");
            }
            finally
            {
                btnSincronizar.Enabled = true;
            }
        }

        private async void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_cargado) return;
            if (tabs.SelectedTab == tabPartidas) await CargarPartidasAsync();
            else if (tabs.SelectedTab == tabUsuarios)
            {
                await CargarPartidasAsync();
                await CargarUsuariosAsync();
            }
            else if (tabs.SelectedTab == tabMovimientos) await CargarMovimientosAsync();
        }

        private static int? IdSeleccionado(DataGridView g)
        {
            if (g.SelectedRows.Count == 0) return null;
            object v = g.SelectedRows[0].Cells["Id"].Value;
            if (v == null) return null;
            return Convert.ToInt32(v);
        }

        private static void Aviso(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Pequeño diálogo para pedir un texto (usado al resetear la contraseña).
        private static string PedirTexto(string titulo, string etiqueta)
        {
            using (Form f = new Form())
            {
                f.Text = titulo;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ClientSize = new Size(360, 130);
                f.MaximizeBox = false;
                f.MinimizeBox = false;

                Label l = new Label { Text = etiqueta, AutoSize = true, Location = new Point(14, 18) };
                TextBox t = new TextBox { Location = new Point(14, 42), Width = 330 };
                Button ok = new Button { Text = "Aceptar", DialogResult = DialogResult.OK, Location = new Point(160, 84), Width = 90 };
                Button cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(256, 84), Width = 88 };

                f.Controls.Add(l);
                f.Controls.Add(t);
                f.Controls.Add(ok);
                f.Controls.Add(cancel);
                f.AcceptButton = ok;
                f.CancelButton = cancel;
                Tema.ApplyTheme(f);

                return f.ShowDialog() == DialogResult.OK ? t.Text : null;
            }
        }
    }
}
