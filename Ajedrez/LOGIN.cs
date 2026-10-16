using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Npgsql; // 🔥 AGREGAR ESTO

namespace Ajedrez
{
    public partial class LOGIN : Form
    {
        

        // 🔥 IMPORTANTE: Reemplaza estos valores con los de tu proyecto
        private const string SUPABASE_HOST = "aws-0-eu-west-1.pooler.supabase.com"; // ← Cambia según tu región
        private const string SUPABASE_DATABASE = "postgres";
        private const string SUPABASE_USER = "postgres.swuneyyjcjzccbnsnhkv"; // ← Cambia por tu usuario
        private const string SUPABASE_PASSWORD = "2788218@clases"; // ← Tu contraseña
        private const int SUPABASE_PORT = 5432;

        // Cadena de conexión con Session Pooler (IPv4)
        private static readonly string CONNECTION_STRING =
            $"Host={SUPABASE_HOST};Port={SUPABASE_PORT};Database={SUPABASE_DATABASE};Username={SUPABASE_USER};Password={SUPABASE_PASSWORD};SSL Mode=Require;Trust Server Certificate=true;Timeout=5;";
     

        public LOGIN()
        {
            InitializeComponent();
            // Opción 1: Registrar cada par individualmente
            Tema.RegisterPictureBoxPair("pictureBox1", "pictureBox2");
            Tema.RegisterPictureBoxPair("pictureBox3", "pictureBox4");
            Tema.RegisterPictureBoxPair( "pictureBox5","pictureBox6");
           

            // Aplicar el tema inicial
            Tema.ApplyTheme(this);

        }

        private void LOGIN_Load(object sender, EventArgs e)
        {
          
        }

        private void nombre_TextChanged(object sender, EventArgs e)
        {
            //NO SIRVE

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

        private async void continuar_Click(object sender, EventArgs e)
        {
            // ============================================================
            // BOTÓN PARA VERIFICAR SI EL USUARIO EXISTE
            // ============================================================

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

            // Deshabilitar botón mientras se verifica
            ingresar_.Enabled = false;
            ingresar_.Text = "Verificando...";

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(CONNECTION_STRING))
                {
                    await conn.OpenAsync();

                    // Consulta para verificar si existe el usuario
                    string sql = "SELECT nombre_usuario FROM usuarios WHERE nombre_usuario = @nombre AND contrasena = @contrasena";

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@contrasena", contraseña.Text);

                        string usuarioEncontrado = (await cmd.ExecuteScalarAsync()) as string;

                        if (!string.IsNullOrEmpty(usuarioEncontrado))
                        {
                            // Se guarda la credencial (con la contraseña hasheada, no en
                            // texto plano) para poder iniciar sesión sin conexión luego.
                            BaseDatosLocal.GuardarCredencial(usuarioEncontrado,
                                BaseDatosLocal.HashContrasena(contraseña.Text));

                            EntrarAlMenu(usuarioEncontrado);
                        }
                        else
                        {
                            MessageBox.Show("❌ Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Sin conexión (o el servidor no responde): se valida contra la copia
                // local guardada en el último inicio de sesión con internet.
                string usuario = nombre.Text.Trim();
                if (BaseDatosLocal.ValidarCredencial(usuario, BaseDatosLocal.HashContrasena(contraseña.Text)))
                {
                    EntrarAlMenu(usuario);
                }
                else
                {
                    MessageBox.Show(
                        "No hay conexión y no se pudieron validar las credenciales en este equipo.\n\n" +
                        "Inicia sesión una vez con internet para poder hacerlo sin conexión después.",
                        "Sin conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                // Rehabilitar botón
                ingresar_.Enabled = true;
                ingresar_.Text = "Verificar";
            }
        
        }

        private void EntrarAlMenu(string usuario)
        {
            Sesion.NombreUsuario = usuario;
            Sesion.ImagenUrl = string.Empty;

            menu f_menu = new menu();
            f_menu.Show();
            this.Hide();
        }

        private void ir_registro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        //NO SIRVE
        }

        private void registrarse_Click(object sender, EventArgs e)
        {
            REGISTRADOR f_registro = new REGISTRADOR();
            f_registro.Show();

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
                checkBox1.Text = "Ver contraseña";

                // Restaurar el color de fondo según el tema
                if (Tema.IsDarkMode)
                    contraseña.BackColor = ColorTranslator.FromHtml("#1a2d4a");
                else
                    contraseña.BackColor = SystemColors.Window;
            }

            // Forzar actualización
            contraseña.Refresh();
        }

        private void contraseña_TextChanged(object sender, EventArgs e)
        {
           //NO SIRVE
        }
    }
}
