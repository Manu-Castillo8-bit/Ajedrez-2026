using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql; // 🔥 IMPORTANTE: Necesitas este paquete
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Ajedrez
{
    public partial class REGISTRADOR : Form
    {
        // ============================================================
        // CONFIGURACIÓN DE SUPABASE - SESSION POOLER (IPv4)
        // ============================================================

        private const string SUPABASE_HOST = "aws-0-eu-west-1.pooler.supabase.com"; // ← 
        private const string SUPABASE_DATABASE = "postgres";
        private const string SUPABASE_USER = "postgres.swuneyyjcjzccbnsnhkv"; // ← 
        private const string SUPABASE_PASSWORD = "2788218@clases"; // ← Tu contraseña
        private const int SUPABASE_PORT = 5432;

        // Cadena de conexión con Session Pooler (IPv4)
        private static readonly string CONNECTION_STRING =
            $"Host={SUPABASE_HOST};Port={SUPABASE_PORT};Database={SUPABASE_DATABASE};Username={SUPABASE_USER};Password={SUPABASE_PASSWORD};SSL Mode=Require;Trust Server Certificate=true;";

        public REGISTRADOR()
        {
            InitializeComponent();

              
        }

        private void m_claro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(false);      // Cambia a tema claro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
        }

        private void m_oscuro_Click(object sender, EventArgs e)
        {
            Tema.SetTheme(true);       // Cambia a tema oscuro
            Tema.ApplyTheme(this);     // Aplica el tema al formulario actual
        }

        private void ir_login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           //NO SIRVE
        }

        private void ingresar_Click(object sender, EventArgs e)
        {
            LOGIN f_login = new LOGIN();
            f_login.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Este evento se dispara cuando cambia el estado
            if (checkBox1.Checked)
            {
                // Mostrar contraseña
                contraseña.UseSystemPasswordChar = false;
                contraseña.PasswordChar = '\0';


            }
            else
            {
                // Ocultar contraseña
                contraseña.UseSystemPasswordChar = true;
                checkBox1.Text = "👁️ Mostrar";

                // Restaurar el color de fondo según el tema
                if (Tema.IsDarkMode)
                    contraseña.BackColor = ColorTranslator.FromHtml("#1a2d4a");
                else
                    contraseña.BackColor = SystemColors.Window;
            }

            // Forzar actualización
            contraseña.Refresh();
        }


        //Async permite que la interfaz de usuario no se congele mientras se realiza la operación de registro en la base de datos.
        private async void g_usuario_Click(object sender, EventArgs e)
        {

            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(nombre.Text))
            {
                MessageBox.Show("Por favor, ingresa un nombre de usuario.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(contraseña.Text))
            {
                MessageBox.Show("Por favor, ingresa una contraseña.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                contraseña.Focus();
                return;
            }

            // Deshabilitar botón para evitar spam
            g_usuario.Enabled = false;
            g_usuario.Text = "Registrando...";

            try
            {
                // Usar la conexión PostgreSQL
                using (var conn = new Npgsql.NpgsqlConnection(CONNECTION_STRING))
                {
                    await conn.OpenAsync();

                    // Consulta SQL para insertar usuario
                    string sql = "INSERT INTO usuarios (nombre_usuario, contrasena) VALUES (@nombre, @contrasena)";

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        // Agregar parámetros (protección contra SQL Injection)
                        cmd.Parameters.AddWithValue("@nombre", nombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@contrasena", contraseña.Text);

                        int filasAfectadas = await cmd.ExecuteNonQueryAsync();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("¡Usuario registrado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            nombre.Clear();
                            contraseña.Clear();
                            nombre.Focus();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo registrar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Npgsql.PostgresException pgEx)
            {
                // Error específico de PostgreSQL/Supabase
                if (pgEx.SqlState == "23505") // Código de violación de clave única
                {
                    MessageBox.Show("El nombre de usuario ya existe. Por favor, elige otro.", "Usuario duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Error de base de datos: {pgEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Rehabilitar botón
                g_usuario.Enabled = true;
                g_usuario.Text = "Registrar";
            }
        }
        
        
        

        private void REGISTRADOR_Load(object sender, EventArgs e)
        {
            
        }
    }
}
