using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Ajedrez
{
    public partial class menu : Form
    {

        public menu()
        {
            InitializeComponent();

            Tema.ApplyTheme(this);
        }



        private void ajedrez_Click(object sender, EventArgs e)
        {
            string usuario = Sesion.NombreUsuario;
            string tipo = "ajedrez";

            // ¿Hay una partida de ajedrez sin terminar? Ofrecer continuarla.
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                PartidaRepository.Partida enCurso = PartidaRepository.BuscarPartidaEnCurso(tipo, usuario);
                if (enCurso != null && ContinuarODescartar(enCurso, tipo))
                    return;
            }

            using (NombresPartida dlg = new NombresPartida("ajedrez"))
            {
                Tema.ApplyTheme(dlg);
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                string blancas = dlg.LocalEsBlancas ? dlg.NombreLocal : dlg.NombreOponente;
                string negras = dlg.LocalEsBlancas ? dlg.NombreOponente : dlg.NombreLocal;
                Ajedrez_tablero f_ajedrez = new Ajedrez_tablero(blancas, negras);

                if (dlg.ModoIA)
                {
                    // La IA juega con el color contrario al del jugador local
                    string colorIA = dlg.LocalEsBlancas ? "black" : "white";
                    f_ajedrez.ConfigurarIA(true, colorIA, dlg.NivelIA);
                }

                Tema.ApplyTheme(f_ajedrez);
                f_ajedrez.Show();
                this.Hide();
            }
        }

        private void ir_damas_Click(object sender, EventArgs e)
        {
            string usuario = Sesion.NombreUsuario;
            string tipo = "damas";

            // ¿Hay una partida de damas sin terminar? Ofrecer continuarla.
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                PartidaRepository.Partida enCurso = PartidaRepository.BuscarPartidaEnCurso(tipo, usuario);
                if (enCurso != null && ContinuarODescartar(enCurso, tipo))
                    return;
            }

            using (NombresPartida dlg = new NombresPartida("damas"))
            {
                Tema.ApplyTheme(dlg);
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                string blancas_d = dlg.LocalEsBlancas ? dlg.NombreLocal : dlg.NombreOponente;
                string negras_d = dlg.LocalEsBlancas ? dlg.NombreOponente : dlg.NombreLocal;
                Form1 f_damas = new Form1(blancas_d, negras_d, dlg.ModoSoplar);
                Tema.ApplyTheme(f_damas);
                f_damas.Show();
                this.Hide();
            }
        }

        /// <summary>
        /// Gestiona una partida que quedó "en_curso".
        /// - Solo pregunta si de verdad hay una posición guardada que recuperar.
        /// - Si se recupera (el jugador elige continuar) devuelve true.
        /// - Si no hay nada que continuar, o el jugador decide no continuarla,
        ///   la partida se finaliza para que NO vuelva a preguntarse al entrar.
        /// </summary>
        private bool ContinuarODescartar(PartidaRepository.Partida enCurso, string tipo)
        {
            PartidaRepository.Movimiento mov;
            if (!PartidaRepository.ObtenerMovimientoSeguro(enCurso.id_partida, out mov))
                return false; // no se pudo consultar: se reintenta la próxima vez

            string estado = (mov != null && mov.tablero != null)
                ? mov.tablero.ToString(Formatting.None) : null;

            // Sin posición guardada (o sin jugadas) no hay nada que continuar.
            if (string.IsNullOrWhiteSpace(estado) || mov.num_movimientos <= 0)
            {
                PartidaRepository.FinalizarPartida(enCurso.id_partida, null);
                return false;
            }

            DialogResult r = MessageBox.Show(
                "Tienes una partida de " + tipo + " sin terminar. ¿Deseas continuarla?\n\n" +
                "Sí = Continuar la partida guardada\n" +
                "No = Cerrarla (no se volverá a preguntar)",
                "Continuar partida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r != DialogResult.Yes)
            {
                // No desea continuarla ahora: se cierra para no volver a preguntar.
                PartidaRepository.FinalizarPartida(enCurso.id_partida, null);
                return false;
            }

            if (tipo == "ajedrez")
            {
                Ajedrez_tablero f_cont = new Ajedrez_tablero(enCurso.jugador_blancas, enCurso.jugador_negras);
                f_cont.ContinuarPartida(enCurso.id_partida, estado);
                Tema.ApplyTheme(f_cont);
                f_cont.Show();
            }
            else
            {
                Form1 f_cont = new Form1(enCurso.jugador_blancas, enCurso.jugador_negras, enCurso.soplar);
                f_cont.ContinuarPartida(enCurso.id_partida, estado);
                Tema.ApplyTheme(f_cont);
                f_cont.Show();
            }

            this.Hide();
            return true;
        }

        private void ir_perfil_Click(object sender, EventArgs e)
        {
            Perfil f_perfil = new Perfil();
            Tema.ApplyTheme(f_perfil);
            f_perfil.Show();
            this.Hide();
        }

        private void ir_movimientos_Click(object sender, EventArgs e)
        {
            Partidas f_partidas = new Partidas();
            Tema.ApplyTheme(f_partidas);
            f_partidas.Show();
            this.Hide();
        }

        private void ir_login_Click(object sender, EventArgs e)
        {
            LOGIN f_login = new LOGIN();
            Tema.ApplyTheme(f_login);
            f_login.Show();
            this.Hide();
        }

        private void ir_admin_Click(object sender, EventArgs e)
        {
            AdminPanel f_admin = new AdminPanel();
            Tema.ApplyTheme(f_admin);
            f_admin.Show();
            this.Hide();
        }

        private void ir_creditos_Click(object sender, EventArgs e)
        {
            using (Creditos f_creditos = new Creditos())
            {
                f_creditos.ShowDialog(this);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
          //NO SIRVE
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //NO SIRVE

        }

        private void label5_Click(object sender, EventArgs e)
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

        private async void menu_Load(object sender, EventArgs e)
        {
            // Activa el protocolo de seguridad HTTPS para .NET 4.7.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            lbl_usuario.Text = Sesion.NombreUsuario;

            // El botón de administración solo aparece para administradores.
            ir_admin.Visible = Administradores.EsAdmin(Sesion.NombreUsuario);

            // Sube pendientes y trae novedades del servidor sin bloquear la interfaz.
            Sincronizador.EnSegundoPlano(Sesion.NombreUsuario);

            // Cargar imagen de perfil desde Supabase
            await CargarUsuarioMenu();
        }

        // Recargar el nombre y la foto cada vez que vuelves al menú (para que sea dinámico)
        private async void menu_Activated(object sender, EventArgs e)
        {
            lbl_usuario.Text = Sesion.NombreUsuario;
            await CargarUsuarioMenu();
        }

        // Carga el nombre y la imagen del usuario activo desde Supabase
        public async Task CargarUsuarioMenu()
        {
            string nombreUsuario = Sesion.NombreUsuario;
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                lbl_usuario.Text = "Invitado";
                pic_usuario.Image = null;
                label4.Text = "0";
                label5.Text = "0";
                return;
            }

            lbl_usuario.Text = nombreUsuario;

            try
            {
                string url = $"{SupabaseConfig.SupabaseUrl}/rest/v1/usuarios?nombre_usuario=eq.{Uri.EscapeDataString(nombreUsuario)}&select=imagen_url,elo,partidas_ganadas";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", SupabaseConfig.SupabaseKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseConfig.SupabaseKey}");

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        return;

                    string json = await response.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<DatosMenu>>(json);

                    if (lista != null && lista.Count > 0)
                    {
                        DatosMenu d = lista[0];

                        // Mostrar el ELO y las victorias del usuario en sus etiquetas
                        label4.Text = (d.elo ?? 0).ToString("0");
                        label5.Text = (d.partidas_ganadas ?? 0).ToString();

                        string urlImagen = d.imagen_url;
                        if (string.IsNullOrWhiteSpace(urlImagen))
                            return;

                        HttpResponseMessage imgResponse = await client.GetAsync(urlImagen);
                        if (imgResponse.IsSuccessStatusCode)
                        {
                            byte[] bytes = await imgResponse.Content.ReadAsByteArrayAsync();
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                pic_usuario.Image = Image.FromStream(ms);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fallo al cargar usuario en menú: " + ex.Message);
            }
        }

        // Datos del usuario que llegan de Supabase para el menú
        private class DatosMenu
        {
            public string imagen_url { get; set; }
            public double? elo { get; set; }
            public int? partidas_ganadas { get; set; }
        }
    }
    }

