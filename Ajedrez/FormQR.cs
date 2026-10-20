using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;

namespace Ajedrez
{
    public class FormQR : Form
    {
        private const string Url = "http://ajedrez-2026.somee.com";

        private PictureBox picQR;
        private Label lblTexto;
        private Button btnAbrir;

        public FormQR()
        {
            Text = "Jugar en línea";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(340, 440);

            lblTexto = new Label
            {
                Text = "Escanea el código con tu celular\no abre la página en este equipo",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            picQR = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(20, 70),
                Size = new Size(300, 300),
                Cursor = Cursors.Hand
            };
            picQR.Click += (s, e) => AbrirPagina();

            btnAbrir = new Button
            {
                Text = "Abrir en el navegador",
                Location = new Point(20, 385),
                Size = new Size(300, 35)
            };
            btnAbrir.Click += (s, e) => AbrirPagina();

            Controls.Add(picQR);
            Controls.Add(btnAbrir);
            Controls.Add(lblTexto);

            GenerarQR();
        }

        private void GenerarQR()
        {
            using (var generador = new QRCodeGenerator())
            using (QRCodeData datos = generador.CreateQrCode(Url, QRCodeGenerator.ECCLevel.Q))
            using (var qr = new QRCode(datos))
            {
                picQR.Image = qr.GetGraphic(20);
            }
        }

        private void AbrirPagina()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el sitio: " + ex.Message);
            }
        }

    }
}
