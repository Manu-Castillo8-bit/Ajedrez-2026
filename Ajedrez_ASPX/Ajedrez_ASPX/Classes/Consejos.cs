using System;
using System.Collections.Generic;
using System.Linq;

namespace Ajedrez_ASPX
{
    /// <summary>
    /// Frases, consejos y reconocimiento de aperturas que se muestran en el
    /// label interactivo del tablero (estilo chess.com).
    /// </summary>
    public static class Consejos
    {
        private static readonly string[] Motivacionales =
        {
            "Cada gran maestro empezó jugando su primera partida.",
            "La paciencia es la mejor jugada en muchas posiciones.",
            "No necesitas ganar en las primeras jugadas, solo no perderlas.",
            "Antes de mover, mira el tablero entero una vez más.",
            "Un error del rival es una oportunidad para ti.",
            "La constancia vence al talento cuando el talento no es constante.",
            "Juega la posición, no solo las piezas.",
            "Piensa en el plan de tu rival antes que en el tuyo.",
            "Las mejores combinaciones nacen de la calma.",
            "No temas complicar la partida si la posición te favorece.",
            "Cree en tu cálculo y mantén la serenidad.",
            "Cada jugada razonada te acerca a jugar mejor.",
        };

        private static readonly string[] Estrategicos =
        {
            "Recuerda enrocar pronto para poner tu rey a salvo.",
            "Desarrolla caballos y alfiles antes de lanzar el ataque.",
            "Controla el centro: quien domina el centro domina el juego.",
            "Cuidado con las clavadas: una pieza atada no puede moverse.",
            "No muevas la misma pieza dos veces en la apertura salvo necesidad.",
            "Evita sacar la dama demasiado pronto en la apertura.",
            "Vigila las casillas débiles alrededor de tu rey.",
            "Antes de capturar, pregúntate si te pueden recapturar.",
            "Conecta tus torres: suelen brillar en la columna abierta.",
            "Un peón pasado en el final vale más que su peso en oro.",
            "Si tienes ventaja material, cambia piezas y simplifica.",
            "Cuidado con el ahogado: sin jaque, el rival puede quedar sin jugadas y empatar.",
        };

        // Aperturas reconocibles: se comparan por prefijo de la lista de jugadas (notación española).
        private static readonly (string Nombre, string[] Jugadas)[] Aperturas =
        {
            ("Apertura Ruy López", new[] { "e4", "e5", "Cf3", "Cc6", "Ab5" }),
            ("Apertura Italiana", new[] { "e4", "e5", "Cf3", "Cc6", "Ac4" }),
            ("Defensa de los Dos Caballos", new[] { "e4", "e5", "Cf3", "Cc6", "Ac4", "Cf6" }),
            ("Apertura Escocesa", new[] { "e4", "e5", "Cf3", "Cc6", "d4" }),
            ("Apertura del Peón de Rey", new[] { "e4", "e5", "Cf3", "Cc6" }),
            ("Defensa Siciliana", new[] { "e4", "c5" }),
            ("Defensa Francesa", new[] { "e4", "e6" }),
            ("Defensa Caro-Kann", new[] { "e4", "c6" }),
            ("Defensa Escandinava", new[] { "e4", "d5" }),
            ("Defensa Alekhine", new[] { "e4", "Cf6" }),
            ("Defensa Moderna", new[] { "e4", "g6" }),
            ("Defensa Pirc", new[] { "e4", "d6" }),
            ("Apertura Vienesa", new[] { "e4", "e5", "Cc3" }),
            ("Apertura del Peón de Rey", new[] { "e4", "e5" }),
            ("Gambito de Rey", new[] { "e4", "e5", "f4" }),
            ("Apertura Inglesa", new[] { "c4" }),
            ("Gambito de Dama", new[] { "d4", "d5", "c4" }),
            ("Defensa Nimzoindia", new[] { "d4", "Cf6", "c4", "e6", "Cc3", "Ab4" }),
            ("Defensa India de Rey", new[] { "d4", "Cf6", "c4", "g6" }),
            ("Defensa India de Dama", new[] { "d4", "Cf6", "c4", "e6" }),
            ("Sistema Londres", new[] { "d4", "d5", "Cf3", "Cf6", "Af4" }),
            ("Sistema Colle", new[] { "d4", "d5", "Cf3", "e6", "e3" }),
            ("Defensa Holandesa", new[] { "d4", "f5" }),
            ("Apertura del Peón de Dama", new[] { "d4", "d5" }),
            ("Apertura del Peón de Dama", new[] { "d4", "Cf6" }),
            ("Apertura del Peón de Dama", new[] { "d4" }),
            ("Apertura Reti", new[] { "Cf3", "d5", "c4" }),
            ("Apertura Reti", new[] { "Cf3" }),
        };

        public static string FraseMotivacional(Random rnd)
        {
            return Motivacionales[rnd.Next(Motivacionales.Length)];
        }

        public static string ConsejoEstrategico(Random rnd)
        {
            return Estrategicos[rnd.Next(Estrategicos.Length)];
        }

        public static string ReconocerApertura(IList<string> jugadas)
        {
            if (jugadas == null || jugadas.Count == 0) return null;

            string mejor = null;
            int mejorLargo = 0;

            foreach (var apertura in Aperturas)
            {
                string[] secuencia = apertura.Jugadas;
                if (secuencia.Length > jugadas.Count) continue;
                if (secuencia.Length < mejorLargo) continue;

                bool coincide = true;
                for (int i = 0; i < secuencia.Length; i++)
                {
                    if (!string.Equals(secuencia[i], jugadas[i], StringComparison.Ordinal))
                    {
                        coincide = false;
                        break;
                    }
                }

                if (coincide)
                {
                    mejor = apertura.Nombre;
                    mejorLargo = secuencia.Length;
                }
            }

            return mejor;
        }

        /// <summary>Mensaje contextual para la jugada recién realizada (o null).</summary>
        public static string Contexto(string san, bool captura, bool enroqueCorto, bool enroqueLargo,
                                      bool alPaso, bool coronacion)
        {
            if (alPaso)
                return "¡Captura al paso! Solo era legal justo después de ese doble avance.";
            if (coronacion)
                return "¡Peón coronado! La pieza más humilde se vuelve la más fuerte.";
            if (enroqueCorto)
                return "Enroque corto: tu rey queda más seguro y la torre entra en juego.";
            if (enroqueLargo)
                return "Enroque largo: el rey se esconde y la torre toma la columna central.";
            if (captura)
                return "Buena captura. Ahora comprueba que no te puedan recapturar.";
            return null;
        }

        public static readonly string MensajeJaque = "¡Jaque! Defiende a tu rey: móvelo, bloquea o captura la pieza atacante.";
    }
}
