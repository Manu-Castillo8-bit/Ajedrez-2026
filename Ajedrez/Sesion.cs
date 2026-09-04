using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajedrez
{
    public static class Sesion
    {
        // Nombre de usuario autenticado (se asigna al validar las credenciales en el Form de LOGIN)
        public static string NombreUsuario { get; set; } = string.Empty;

        // Información adicional del usuario activo (opcional para mostrar en la interfaz)
        public static string CorreoUsuario { get; set; } = string.Empty;
        public static string ImagenUrl { get; set; } = string.Empty;

        /// <summary>
        /// Restablece los datos de la sesión al cerrar sesión.
        /// </summary>
        public static void CerrarSesion()
        {
            NombreUsuario = string.Empty;
            CorreoUsuario = string.Empty;
            ImagenUrl = string.Empty;
        }
    }
}
