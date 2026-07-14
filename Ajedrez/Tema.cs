using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajedrez
{
    public static class Tema
        {
        // Variable simple para guardar el estado del tema
        public static bool IsDarkMode { get; private set; } = false;

        // Diccionario para almacenar los pares de PictureBox (claro, oscuro)
        private static Dictionary<string, (string lightControl, string darkControl)> _pictureBoxMappings
            = new Dictionary<string, (string, string)>();

        // Método para cambiar el tema
        public static void SetTheme(bool isDark)
        {
            IsDarkMode = isDark;
        }

        // Método para registrar un par de PictureBox
        public static void RegisterPictureBoxPair(string lightPictureBoxName, string darkPictureBoxName)
        {
            if (!_pictureBoxMappings.ContainsKey(lightPictureBoxName))
            {
                _pictureBoxMappings.Add(lightPictureBoxName, (lightPictureBoxName, darkPictureBoxName));
            }
            else
            {
                _pictureBoxMappings[lightPictureBoxName] = (lightPictureBoxName, darkPictureBoxName);
            }
        }

        // NUEVO: Método para registrar múltiples pares de PictureBox a la vez
        public static void RegisterPictureBoxPairs(params (string light, string dark)[] pairs)
        {
            foreach (var pair in pairs)
            {
                RegisterPictureBoxPair(pair.light, pair.dark);
            }
        }

        // NUEVO: Método para registrar múltiples pares con listas separadas
        public static void RegisterMultiplePictureBoxPairs(string[] lightControls, string[] darkControls)
        {
            if (lightControls.Length != darkControls.Length)
            {
                throw new ArgumentException("Los arreglos deben tener la misma longitud");
            }

            for (int i = 0; i < lightControls.Length; i++)
            {
                RegisterPictureBoxPair(lightControls[i], darkControls[i]);
            }
        }

        // Método para aplicar el tema a cualquier formulario
        public static void ApplyTheme(Form form)
        {
            if (IsDarkMode)
            {
                ApplyDarkTheme(form);
            }
            else
            {
                ApplyLightTheme(form);
            }
        }

        // Método para actualizar la visibilidad de los PictureBox
        private static void UpdatePictureBoxVisibility(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Buscar los PictureBox registrados
                if (control is PictureBox pictureBox)
                {
                    foreach (var mapping in _pictureBoxMappings.Values)
                    {
                        if (pictureBox.Name == mapping.lightControl)
                        {
                            // PictureBox de modo claro
                            pictureBox.Visible = !IsDarkMode;
                        }
                        else if (pictureBox.Name == mapping.darkControl)
                        {
                            // PictureBox de modo oscuro
                            pictureBox.Visible = IsDarkMode;
                        }
                    }
                }

                // Si el control tiene hijos, procesarlos también
                if (control.HasChildren)
                {
                    UpdatePictureBoxVisibility(control.Controls);
                }
            }
        }

        private static void ApplyDarkTheme(Form form)
        {
            // FONDO
            form.BackColor = ColorTranslator.FromHtml("#0a1628");

            // Aplicar a todos los controles (incluyendo los que están dentro de otros contenedores)
            ApplyDarkThemeToControls(form.Controls);

            // Actualizar visibilidad de PictureBox
            UpdatePictureBoxVisibility(form.Controls);
        }

        private static void ApplyDarkThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // LABELS
                if (control is Label label)
                {
                    if (label.Name == "label1" || label.Name == "label2")
                    {
                        label.ForeColor = Color.White;
                    }
                    else if (label.Name == "label3" || label.Name == "label4" || label.Name == "label5")
                    {
                        label.ForeColor = ColorTranslator.FromHtml("#9ab8e8");
                    }
                    else
                    {
                        label.ForeColor = Color.White;
                    }
                }

                // LINKLABEL - Para ir_registro
                else if (control is LinkLabel linkLabel)
                {
                    // Guardar el texto del enlace para forzar la actualización
                    string textoOriginal = linkLabel.Text;

                    // Aplicar colores para el tema actual
                    if (IsDarkMode)
                    {
                        linkLabel.LinkColor = ColorTranslator.FromHtml("#4a8af4");
                        linkLabel.ActiveLinkColor = ColorTranslator.FromHtml("#22b8f0");
                        linkLabel.VisitedLinkColor = ColorTranslator.FromHtml("#6a5acd");
                        linkLabel.ForeColor = Color.White;
                    }
                    else
                    {
                        linkLabel.LinkColor = ColorTranslator.FromHtml("#0066CC");
                        linkLabel.ActiveLinkColor = ColorTranslator.FromHtml("#FF0000");
                        linkLabel.VisitedLinkColor = ColorTranslator.FromHtml("#800080");
                        linkLabel.ForeColor = Color.Black;
                    }
                    linkLabel.BackColor = Color.Transparent;

                    // Forzar la actualización del control
                    linkLabel.Invalidate();
                    linkLabel.Update();
                }
                // CHECKBOX - Para checkBox1
                else if (control is CheckBox checkBox)
                {
                    checkBox.ForeColor = Color.White;
                    checkBox.BackColor = Color.Transparent;
                }
                // RADIOBUTTON
                else if (control is RadioButton radioButton)
                {
                    radioButton.ForeColor = Color.White;
                    radioButton.BackColor = Color.Transparent;
                }
                // BUTTONS
                else if (control is Button button)
                {
                    button.ForeColor = Color.White;
                    button.Font = new Font(button.Font, FontStyle.Bold);

                    // Colores específicos para cada botón del menú
                    switch (button.Name)
                    {
                        case "ajedrez":
                            button.BackColor = ColorTranslator.FromHtml("#22b8f0");
                            break;
                        case "ir_damas":
                            button.BackColor = ColorTranslator.FromHtml("#4a8af4");
                            break;
                        case "ir_perfil":
                            button.BackColor = ColorTranslator.FromHtml("#4a8af4");
                            break;
                        case "ir_partidas":
                            button.BackColor = ColorTranslator.FromHtml("#22b8f0");
                            break;
                        case "ir_login":
                            button.BackColor = ColorTranslator.FromHtml("#18c484");
                            break;
                        case "m_oscuro":
                            button.BackColor = ColorTranslator.FromHtml("#4a8af4");
                            break;
                        case "m_claro":
                            button.BackColor = SystemColors.Control;
                            break;
                        case "registrarse":
                            button.BackColor = ColorTranslator.FromHtml("#22b8f0");
                            break;
                        case "ingresar":
                            button.BackColor = ColorTranslator.FromHtml("#22b8f0");
                            break;
                        default:
                            button.BackColor = ColorTranslator.FromHtml("#4a8af4");
                            break;
                    }
                }
                // GROUPBOX
                else if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = ColorTranslator.FromHtml("#1a2d4a");
                    groupBox.ForeColor = Color.White; // También cambiamos el texto del GroupBox
                }
                // TEXTBOX
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = ColorTranslator.FromHtml("#1a2d4a");
                    textBox.ForeColor = Color.White;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }
                // PANEL
                else if (control is Panel panel)
                {
                    panel.BackColor = ColorTranslator.FromHtml("#0f1f3a");
                }

                // Si el control tiene hijos (como GroupBox, Panel, etc.), aplicar también a ellos
                if (control.HasChildren)
                {
                    ApplyDarkThemeToControls(control.Controls);
                }
            }
        }

        private static void ApplyLightTheme(Form form)
        {
            // FONDO
            form.BackColor = SystemColors.Control;

            // Aplicar a todos los controles (incluyendo los que están dentro de otros contenedores)
            ApplyLightThemeToControls(form.Controls);

            // Actualizar visibilidad de PictureBox
            UpdatePictureBoxVisibility(form.Controls);
        }

        private static void ApplyLightThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // LABELS
                if (control is Label label)
                {
                    label.ForeColor = Color.Black;
                }
                // LINKLABEL - Para ir_registro
                else if (control is LinkLabel linkLabel)
                {
                    // Colores para modo claro
                    linkLabel.LinkColor = ColorTranslator.FromHtml("#0066CC"); // Azul estándar
                    linkLabel.ActiveLinkColor = ColorTranslator.FromHtml("#FF0000"); // Rojo al hacer hover
                    linkLabel.VisitedLinkColor = ColorTranslator.FromHtml("#800080"); // Púrpura para enlaces visitados
                    linkLabel.ForeColor = Color.Black;
                    linkLabel.BackColor = Color.Transparent;
                }
                // CHECKBOX - Para checkBox1
                else if (control is CheckBox checkBox)
                {
                    checkBox.ForeColor = Color.Black;
                    checkBox.BackColor = Color.Transparent;
                }
                // RADIOBUTTON
                else if (control is RadioButton radioButton)
                {
                    radioButton.ForeColor = Color.Black;
                    radioButton.BackColor = Color.Transparent;
                }
                // BUTTONS
                else if (control is Button button)
                {
                    button.ForeColor = Color.Black;
                    button.Font = new Font(button.Font, FontStyle.Regular);

                    // Colores específicos para cada botón del menú
                    switch (button.Name)
                    {
                        case "ajedrez":
                            button.BackColor = ColorTranslator.FromHtml("#F0F6FF");
                            break;
                        case "ir_damas":
                            button.BackColor = ColorTranslator.FromHtml("#DCE4F0");
                            break;
                        case "ir_perfil":
                            button.BackColor = ColorTranslator.FromHtml("#DCE4F0");
                            break;
                        case "ir_partidas":
                            button.BackColor = ColorTranslator.FromHtml("#F0F6FF");
                            break;
                        case "ir_login":
                            button.BackColor = ColorTranslator.FromHtml("#DCE4F0");
                            break;
                        case "m_oscuro":
                            button.BackColor = ColorTranslator.FromHtml("#4a8af4");
                            break;
                        case "m_claro":
                            button.BackColor = SystemColors.Control;
                            break;
                        default:
                            button.BackColor = SystemColors.Control;
                            break;
                    }
                }
                // GROUPBOX
                else if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = SystemColors.ControlLight;
                    groupBox.ForeColor = Color.Black;
                }
                // TEXTBOX
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = SystemColors.Window;
                    textBox.ForeColor = SystemColors.WindowText;
                    textBox.BorderStyle = BorderStyle.Fixed3D;
                }
                // PANEL
                else if (control is Panel panel)
                {
                    panel.BackColor = SystemColors.Control;
                }

                // Si el control tiene hijos (como GroupBox, Panel, etc.), aplicar también a ellos
                if (control.HasChildren)
                {
                    ApplyLightThemeToControls(control.Controls);
                }
            }
        }
    }
    }
    
    


