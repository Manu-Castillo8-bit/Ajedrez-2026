using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ajedrez
{
    public static class GestorFuentes
    {

        private static PrivateFontCollection _coleccion = new PrivateFontCollection();

        public static Font ObtenerFuente(float tamano, FontStyle estilo = FontStyle.Regular)
        {
            // Solo carga la fuente en memoria la primera vez que se llama
            if (_coleccion.Families.Length == 0)
            {
                // Revisa que coincide con Namespace.NombreArchivo
                string rutaRecurso = "Ajedrez.fuente_04b.TTF";

                Assembly ensamblado = Assembly.GetExecutingAssembly();
                using (Stream stream = ensamblado.GetManifestResourceStream(rutaRecurso))
                {
                    if (stream == null)
                    {
                        throw new Exception($"No se pudo encontrar el recurso incrustado: {rutaRecurso}");
                    }

                    byte[] datosFuente = new byte[stream.Length];
                    stream.Read(datosFuente, 0, (int)stream.Length);

                    IntPtr ptrData = Marshal.AllocHGlobal(datosFuente.Length);
                    Marshal.Copy(datosFuente, 0, ptrData, datosFuente.Length);
                    _coleccion.AddMemoryFont(ptrData, datosFuente.Length);
                    Marshal.FreeHGlobal(ptrData);
                }
            }

            return new Font(_coleccion.Families[0], tamano, estilo);
        }
    }
}

