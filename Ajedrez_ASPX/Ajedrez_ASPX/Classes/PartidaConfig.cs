using System;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Configuración de una partida nueva (o en curso que se va a continuar).
    /// La crea NuevaPartida.aspx (o el menú al continuar) y la consumen
    /// Ajedrez.aspx y Damas.aspx en su primer carga.
    /// </summary>
    [Serializable]
    public class PartidaConfig
    {
        /// <summary>"ajedrez" o "damas".</summary>
        public string Tipo { get; set; }

        public string Blancas { get; set; }
        public string Negras { get; set; }

        /// <summary>Modo soplar (solo damas): resaltar capturas obligatorias.</summary>
        public bool Soplar { get; set; }

        /// <summary>La partida se juega contra Stockfish (solo ajedrez).</summary>
        public bool ModoIA { get; set; }

        /// <summary>Nivel del motor 1..20 (Fácil=1, Media=5, Difícil=12, Experto=20).</summary>
        public int NivelIA { get; set; }

        /// <summary>Id de la partida en la nube; null si todavía no se ha creado.</summary>
        public int? IdPartida { get; set; }

        /// <summary>JSON del estado del tablero guardado (para continuar).</summary>
        public string EstadoInicial { get; set; }

        /// <summary>Convierte el índice del combo de nivel del escritorio (0..3) al nivel real.</summary>
        public static int NivelDesdeIndice(int indice)
        {
            switch (indice)
            {
                case 0: return 1;   // Fácil
                case 2: return 12;  // Difícil
                case 3: return 20;  // Experto
                default: return 5;  // Media
            }
        }
    }

    public static class PartidaSesion
    {
        private const string Clave = "PartidaConfig";

        public static PartidaConfig Actual
        {
            get
            {
                object v = System.Web.HttpContext.Current.Session[Clave];
                return v as PartidaConfig;
            }
            set { System.Web.HttpContext.Current.Session[Clave] = value; }
        }

        public static void Limpiar()
        {
            System.Web.HttpContext.Current.Session.Remove(Clave);
        }
    }
}
