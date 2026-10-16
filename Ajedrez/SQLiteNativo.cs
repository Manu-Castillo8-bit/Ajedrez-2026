using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ajedrez
{
    /// <summary>
    /// Prepara SQLite para funcionar en .NET Framework. La librería
    /// sqlite-net-pcl usa el proveedor e_sqlite3, que es una DLL nativa que en
    /// escritorio NO se resuelve sola desde la carpeta "runtimes". Por eso, antes
    /// de tocar ninguna base de datos, se apunta la búsqueda de DLLs a la carpeta
    /// de la arquitectura correcta (win-x64 o win-x86) y se inicializa el
    /// proveedor una sola vez.
    /// </summary>
    internal static class SQLiteNativo
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool SetDllDirectory(string lpPathName);

        private const int SinIniciar = 0;
        private const int Iniciado = 1;
        private const int Fallido = 2;

        private static readonly object Cerrojo = new object();
        private static int _estado = SinIniciar;

        /// <summary>
        /// Deja SQLite listo para usarse. Es idempotente y segura desde varios
        /// hilos. Devuelve false si no se pudo preparar (por ejemplo, si falta
        /// la DLL nativa), en cuyo caso la app debe seguir funcionando sin la
        /// base de datos local.
        /// </summary>
        public static bool Asegurar()
        {
            if (_estado == Iniciado) return true;
            if (_estado == Fallido) return false;

            lock (Cerrojo)
            {
                if (_estado == Iniciado) return true;
                if (_estado == Fallido) return false;

                try
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string rid = Environment.Is64BitProcess ? "win-x64" : "win-x86";
                    string rutaNativa = Path.Combine(baseDir, "runtimes", rid, "native");

                    // Si existe la subcarpeta por arquitectura se usa esa; si no,
                    // se busca junto al ejecutable.
                    SetDllDirectory(Directory.Exists(rutaNativa) ? rutaNativa : baseDir);

                    SQLitePCL.Batteries_V2.Init();
                    _estado = Iniciado;
                    return true;
                }
                catch
                {
                    _estado = Fallido;
                    return false;
                }
            }
        }
    }
}
