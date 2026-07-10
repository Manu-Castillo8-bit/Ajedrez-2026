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

        // Método para cambiar el tema
        public static void SetTheme(bool isDark)
        {
            IsDarkMode = isDark;
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

        private static void ApplyDarkTheme(Form form)
        {
            // FONDO
            form.BackColor = ColorTranslator.FromHtml("#0a1628");

            // Aplicar a todos los controles (incluyendo los que están dentro de otros contenedores)
            ApplyDarkThemeToControls(form.Controls);
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
                            button.BackColor = ColorTranslator.FromHtml("#4a8af4");
                            break;
                        case "m_oscuro":
                            button.BackColor = ColorTranslator.FromHtml("#4a8af4");
                            break;
                        case "m_claro":
                            button.BackColor = SystemColors.Control;
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

