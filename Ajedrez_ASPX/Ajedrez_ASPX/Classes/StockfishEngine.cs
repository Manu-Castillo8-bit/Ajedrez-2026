using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace Ajedrez_ASPX
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
    /// En la versión web la descarga es síncrona: la página avisa al jugador
    /// con un label antes de lanzarla.
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
        public static string Descargar()
        {
            try { Directory.CreateDirectory(Carpeta); }
            catch { return null; }

            string zip = Path.Combine(Path.GetTempPath(), "stockfish_sf16.zip");

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (WebClient wc = new WebClient())
                    wc.DownloadFile(Url, zip);

                string tmp = Path.Combine(Path.GetTempPath(), "sf16_" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(tmp);
                ExtraerZip(zip, tmp);

                string encontrado = Directory.GetFiles(tmp, "stockfish*.exe", SearchOption.AllDirectories).FirstOrDefault();
                if (encontrado == null)
                    throw new FileNotFoundException("El paquete descargado no contiene el ejecutable de Stockfish.");

                File.Copy(encontrado, RutaExe, true);

                try { Directory.Delete(tmp, true); } catch { }
                try { File.Delete(zip); } catch { }
            }
            catch
            {
                return null;
            }

            return File.Exists(RutaExe) ? RutaExe : null;
        }

        /// <summary>
        /// Extrae un ZIP. System.IO.Compression.FileSystem no está referenciado
        /// en el csproj web (que no se puede editar), así que se carga por
        /// reflexión desde el GAC de .NET Framework.
        /// </summary>
        private static void ExtraerZip(string zip, string destino)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.Load(
                "System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            Type tipo = asm.GetType("System.IO.Compression.ZipFile");
            System.Reflection.MethodInfo metodo = tipo.GetMethod("ExtractToDirectory",
                new Type[] { typeof(string), typeof(string) });
            metodo.Invoke(null, new object[] { zip, destino });
        }
    }
}
