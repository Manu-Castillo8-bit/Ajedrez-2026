using System;
using System.Linq;

namespace Ajedrez
{
    /// <summary>
    /// Define quién puede abrir el panel administrativo. Por ahora es una lista
    /// fija en el código: agrega aquí los nombres de usuario (tal como están en
    /// la columna "nombre_usuario" de la base de datos) que serán administradores.
    /// La comparación no distingue mayúsculas.
    /// </summary>
    public static class Administradores
    {
        // ⚠️ EDITA ESTA LISTA con el/los usuarios que deben ser administradores.
        private static readonly string[] Nombres = { "admin", "admin_manu" };

        public static bool EsAdmin(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario)) return false;
            return Nombres.Any(n => string.Equals(n, usuario, StringComparison.OrdinalIgnoreCase));
        }
    }
}
