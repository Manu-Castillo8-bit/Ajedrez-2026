using System;
using System.Web;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Datos de la sesión del usuario actual. Equivalente web de la clase
    /// estática Sesion del proyecto de escritorio: aquí vive en Session de ASP.NET
    /// para que cada peticion tenga su propio estado.
    /// </summary>
    public static class Sesion
    {
        private static HttpContext C { get { return HttpContext.Current; } }

        private static string Leer(string clave)
        {
            object v = C != null && C.Session != null ? C.Session[clave] : null;
            return v as string ?? string.Empty;
        }

        private static void Escribir(string clave, string valor)
        {
            if (C != null && C.Session != null) C.Session[clave] = valor ?? string.Empty;
        }

        /// <summary>Nombre de usuario autenticado (vacio si es invitado).</summary>
        public static string NombreUsuario
        {
            get { return Leer("Usuario"); }
            set { Escribir("Usuario", value); }
        }

        public static string CorreoUsuario
        {
            get { return Leer("Correo"); }
            set { Escribir("Correo", value); }
        }

        public static string ImagenUrl
        {
            get { return Leer("Imagen"); }
            set { Escribir("Imagen", value); }
        }

        /// <summary>Tema oscuro activo (lo usan las paginas y el Site.Master).</summary>
        public static bool TemaOscuro
        {
            get
            {
                object v = C != null && C.Session != null ? C.Session["TemaOscuro"] : null;
                return v is bool && (bool)v;
            }
            set
            {
                if (C != null && C.Session != null) C.Session["TemaOscuro"] = value;
            }
        }

        /// <summary>Restablece los datos de la sesion al cerrar sesion.</summary>
        public static void CerrarSesion()
        {
            NombreUsuario = string.Empty;
            CorreoUsuario = string.Empty;
            ImagenUrl = string.Empty;
            if (C != null && C.Session != null)
            {
                C.Session.Remove("PartidaConfig");
                C.Session.Remove("EstadoAjedrez");
                C.Session.Remove("EstadoDamas");
                C.Session.Abandon();
            }
        }
    }
}
