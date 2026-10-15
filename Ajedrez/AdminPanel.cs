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

            await CargarUsuariosAsync();
            await CargarPartidasAsync();
            await CargarMovimientosAsync();
        }

        private async Task CargarUsuariosAsync()
        {
            btnUsuariosRefrescar.Enabled = false;
            var lista = await Task.Run(() => Nube.ListarUsuarios());
            _usuarios = lista ?? new List<UsuarioCompleto>();
            dgvUsuarios.DataSource = _usuarios.Select(u => new FilaUsuario
            {
                Id = u.id_usuario,
                Nombre = u.nombre_usuario,
                Email = u.email,
                Contrasena = u.contrasena,
                ELO = u.elo,
                Jugadas = u.partidas_jugadas,
                Ganadas = u.partidas_ganadas,
                Empatadas = u.partidas_empatadas,
                Perdidas = u.partidas_perdidas,
                Activo = u.activo
            }).ToList();
            btnUsuariosRefrescar.Enabled = true;
        }

        private async Task CargarPartidasAsync()
        {
            btnPartidasRefrescar.Enabled = false;
            var lista = await Task.Run(() => Nube.ListarTodasPartidas());
            _partidas = lista ?? new List<PartidaRepository.Partida>();
            dgvPartidas.DataSource = _partidas.Select(p => new FilaPartida
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

            // Copia para no dejar cambios a medias si el usuario cancela.
            UsuarioCompleto copia = new UsuarioCompleto
            {
                id_usuario = original.id_usuario,
                nombre_usuario = original.nombre_usuario,
                email = original.email,
                contrasena = original.contrasena,
                fecha_registro = original.fecha_registro,
                elo = original.elo,
                partidas_jugadas = original.partidas_jugadas,
                partidas_ganadas = original.partidas_ganadas,
                partidas_empatadas = original.partidas_empatadas,
                partidas_perdidas = original.partidas_perdidas,
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
            await CargarMovimientosAsync();
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
