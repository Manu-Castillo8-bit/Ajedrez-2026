using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajedrez
{
    /// <summary>
    /// Envoltorio mínimo del motor de ajedrez Stockfish. Se comunica por el
    /// protocolo UCI a través de stdin/stdout del proceso.
    /// </summary>
    public class StockfishEngine : IDisposable
    {
        private Process _proc;
        private StreamWriter _entrada;
        private StreamReader _salida;
        private int _nivelActual = -1;

        public string Ruta { get; private set; }

        public StockfishEngine(string ruta)
        {
            Ruta = ruta;
        }

        /// <summary>Arranca el proceso y realiza el handshake UCI.</summary>
        public void Iniciar()
        {
            if (_proc != null && !_proc.HasExited) return;

            ProcessStartInfo psi = new ProcessStartInfo(Ruta);
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;

            _proc = Process.Start(psi);
            _entrada = _proc.StandardInput;
            _salida = _proc.StandardOutput;
            _entrada.AutoFlush = true;

            _entrada.WriteLine("uci");
            LeerHasta("uciok");

            _entrada.WriteLine("isready");
            LeerHasta("readyok");
        }

        /// <summary>Ajusta la fuerza del motor (Skill Level 0..20).</summary>
        public void EstablecerNivel(int skill)
        {
            if (skill < 0) skill = 0;
            if (skill > 20) skill = 20;
            if (_nivelActual == skill) return;

            _entrada.WriteLine("setoption name Skill Level value " + skill);
            _entrada.WriteLine("isready");
            LeerHasta("readyok");
            _nivelActual = skill;
        }

        /// <summary>Devuelve el mejor movimiento en formato UCI (p.ej. "e2e4" o "e7e8q").</summary>
        public string MejorMovimiento(string fen, int tiempoMs)
        {
            _entrada.WriteLine("position fen " + fen);
            _entrada.WriteLine("go movetime " + tiempoMs);

            string linea;
            while ((linea = _salida.ReadLine()) != null)
            {
                if (linea.StartsWith("bestmove", StringComparison.Ordinal))
                {
                    string[] partes = linea.Split(' ');
                    if (partes.Length >= 2 && partes[1] != "(none)")
                        return partes[1];
                    return null;
                }
            }
            return null;
        }

        private void LeerHasta(string marcador)
        {
            string linea;
            while ((linea = _salida.ReadLine()) != null)
            {
                if (linea.StartsWith(marcador, StringComparison.Ordinal))
                    return;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_entrada != null)
                {
                    _entrada.WriteLine("quit");
                    _entrada.Dispose();
                }
            }
            catch { }

            try
            {
                if (_proc != null && !_proc.HasExited)
                    _proc.Kill();
            }
            catch { }

            try { if (_proc != null) _proc.Dispose(); } catch { }
            _proc = null;
        }
    }

    /// <summary>
    /// Localiza, y si es necesario descarga, el ejecutable de Stockfish.
    /// </summary>
    public static class StockfishDescarga
    {
        public const string Url =
            "https://github.com/official-stockfish/Stockfish/releases/download/sf_16/stockfish-windows-x86-64.zip";

        public static string Carpeta
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Ajedrez", "stockfish");
            }
        }

        public static string RutaExe
        {
            get { return Path.Combine(Carpeta, "stockfish.exe"); }
        }

        /// <summary>Busca stockfish.exe en las ubicaciones conocidas; null si no está.</summary>
        public static string Buscar()
        {
            string[] candidatas = new string[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stockfish.exe"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Stockfish", "stockfish.exe"),
                RutaExe
            };

            foreach (string c in candidatas)
            {
                try { if (File.Exists(c)) return c; }
                catch { }
            }
            return null;
        }

        /// <summary>Descarga y extrae Stockfish. Devuelve la ruta del exe o null si falla.</summary>
        public static string Descargar(IWin32Window propietario)
        {
            try { Directory.CreateDirectory(Carpeta); }
            catch { return null; }

            string zip = Path.Combine(Path.GetTempPath(), "stockfish_sf16.zip");
            Exception error = null;

            using (DescargaForm prog = new DescargaForm())
            {
                Task.Run(delegate
                {
                    try
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        using (WebClient wc = new WebClient())
                            wc.DownloadFile(Url, zip);

                        string tmp = Path.Combine(Path.GetTempPath(), "sf16_" + Guid.NewGuid().ToString("N"));
                        Directory.CreateDirectory(tmp);
                        ZipFile.ExtractToDirectory(zip, tmp);

                        string encontrado = Directory.GetFiles(tmp, "stockfish*.exe", SearchOption.AllDirectories).FirstOrDefault();
                        if (encontrado == null)
                            throw new FileNotFoundException("El paquete descargado no contiene el ejecutable de Stockfish.");

                        File.Copy(encontrado, RutaExe, true);

                        try { Directory.Delete(tmp, true); } catch { }
                        try { File.Delete(zip); } catch { }
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                    finally
                    {
                        try { prog.BeginInvoke(new Action(delegate { prog.Close(); })); }
                        catch { }
                    }
                });

                prog.ShowDialog(propietario);
            }

            if (error != null)
                return null;

            return File.Exists(RutaExe) ? RutaExe : null;
        }

        /// <summary>Formulario modal simple con barra de progreso indeterminada.</summary>
        private class DescargaForm : Form
        {
            public DescargaForm()
            {
                Text = "Descargando Stockfish";
                FormBorderStyle = FormBorderStyle.FixedDialog;
                StartPosition = FormStartPosition.CenterParent;
                MaximizeBox = false;
                MinimizeBox = false;
                ControlBox = false;
                ClientSize = new Size(360, 90);

                Label lbl = new Label();
                lbl.Text = "Descargando Stockfish (~35 MB) desde el sitio oficial…";
                lbl.AutoSize = false;
                lbl.Location = new Point(14, 16);
                lbl.Size = new Size(332, 20);

                ProgressBar pb = new ProgressBar();
                pb.Style = ProgressBarStyle.Marquee;
                pb.MarqueeAnimationSpeed = 30;
                pb.Location = new Point(14, 50);
                pb.Size = new Size(332, 20);

                Controls.Add(lbl);
                Controls.Add(pb);
            }
        }
    }
}
