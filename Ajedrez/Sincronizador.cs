using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ajedrez
{
    /// <summary>
    /// Mantiene al día la base local (SQLite, la fuente de verdad) y Supabase.
    ///
    /// Orden de trabajo: primero se sube lo que el usuario hizo en esta máquina
    /// (PUSH) y después se baja lo que cambió en el servidor (PULL). Las reglas de
    /// conflicto son sencillas:
    ///   - Si una fila local tiene cambios sin enviar (pendiente), manda lo local.
    ///   - Si la fila local ya está sincronizada, manda el servidor.
    ///
    /// Nunca lanza excepciones: si algo falla, se queda pendiente y se reintenta
    /// en la siguiente ronda. Todo se ejecuta fuera del hilo de la interfaz.
    /// </summary>
    public static class Sincronizador
    {
        /// <summary>
        /// Evita que dos sincronizaciones se pisen (el menú, el guardado de una
        /// partida y el registro de resultado pueden pedirla a la vez). Quien
        /// pierde la carrera simplemente no hace nada esa ronda.
        /// </summary>
        private static readonly SemaphoreSlim Cerrojo = new SemaphoreSlim(1, 1);

        /// <summary>Lanza una sincronización en segundo plano, sin bloquear la interfaz.</summary>
        public static void EnSegundoPlano(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario)) return;
            if (!BaseDatosLocal.Disponible) return;

            string u = usuario;
            Task.Run(() =>
            {
                try { Sincronizar(u); }
                catch { /* nunca debe tumbar la app */ }
            });
        }

        public static void Sincronizar(string usuario)
        {
            if (!BaseDatosLocal.Disponible) return;
            if (!Cerrojo.Wait(0)) return;

            try
            {
                EmpujarPartidas();
                EmpujarMovimientos();
                EmpujarUsuarios();

                if (!string.IsNullOrWhiteSpace(usuario))
                {
                    TraerPartidas(usuario);
                    TraerMovimientos(usuario);
                    TraerUsuario(usuario);
                }
            }
            catch
            {
                // Se reintenta en la próxima ronda.
            }
            finally
            {
                Cerrojo.Release();
            }
        }

        // ── PUSH ──

        private static void EmpujarPartidas()
        {
            foreach (var p in BaseDatosLocal.PartidasPendientes())
            {
                if (p.ServerId == null || p.ServerId.Value <= 0)
                {
                    int id = Nube.InsertarPartida(p);
                    if (id > 0) BaseDatosLocal.MarcarPartidaSincronizada(p.IdLocal, id);
                }
                else if (Nube.ActualizarPartida(p.ServerId.Value, p))
                {
                    BaseDatosLocal.MarcarPartidaSincronizada(p.IdLocal, p.ServerId.Value);
                }
            }
        }

        private static void EmpujarMovimientos()
        {
            foreach (var m in BaseDatosLocal.MovimientosPendientes())
            {
                var partida = BaseDatosLocal.PartidaPorIdLocal(m.IdPartidaLocal);
                if (partida == null || partida.ServerId == null || partida.ServerId.Value <= 0)
                    continue; // la partida aún no tiene id del servidor

                if (m.ServerId == null || m.ServerId.Value <= 0)
                {
                    int id = Nube.InsertarMovimiento(partida.ServerId.Value, m);
                    if (id > 0) BaseDatosLocal.MarcarMovimientoSincronizado(m.IdLocal, id);
                }
                else if (Nube.ActualizarMovimiento(m.ServerId.Value, m))
                {
                    BaseDatosLocal.MarcarMovimientoSincronizado(m.IdLocal, m.ServerId.Value);
                }
            }
        }

        private static void EmpujarUsuarios()
        {
            foreach (var u in BaseDatosLocal.UsuariosPendientes())
            {
                if (u.ServerId == null || u.ServerId.Value <= 0) continue;
                if (Nube.ActualizarUsuario(u.ServerId.Value, u))
                    BaseDatosLocal.MarcarUsuarioSincronizado(u.IdLocal, u.ServerId.Value);
            }
        }

        // ── PULL ──

        private static void TraerPartidas(string usuario)
        {
            var remotas = Nube.ListarPartidas(null, usuario);
            if (remotas == null) return;

            foreach (var sp in remotas)
            {
                if (sp.id_partida <= 0) continue;

                var local = BaseDatosLocal.PartidaPorServerId(sp.id_partida);
                if (local == null)
                    BaseDatosLocal.InsertarPartidaDesdeServidor(sp);
                else if (local.SyncState != EstadoSync.Pendiente)
                    BaseDatosLocal.ActualizarPartidaDesdeServidor(local, sp);
            }
        }

        private static void TraerMovimientos(string usuario)
        {
            var locales = BaseDatosLocal.ListarPartidas(null, usuario);
            var ids = locales
                .Where(p => p.ServerId.HasValue && p.ServerId.Value > 0)
                .Select(p => p.ServerId.Value)
                .ToList();
            if (ids.Count == 0) return;

            var remotos = Nube.MovimientosDePartidas(ids);
            foreach (var sm in remotos)
            {
                if (sm.id_movimiento <= 0) continue;

                var partida = BaseDatosLocal.PartidaPorServerId(sm.id_partida);
                if (partida == null) continue;

                var local = BaseDatosLocal.MovimientoPorServerId(sm.id_movimiento);
                if (local == null)
                    BaseDatosLocal.InsertarMovimientoDesdeServidor(partida.IdLocal, sm);
                else if (local.SyncState != EstadoSync.Pendiente)
                    BaseDatosLocal.ActualizarMovimientoDesdeServidor(local, sm);
            }
        }

        private static void TraerUsuario(string usuario)
        {
            var su = Nube.ObtenerUsuario(usuario);
            if (su == null) return;

            var local = BaseDatosLocal.ObtenerUsuario(usuario);
            if (local == null)
                BaseDatosLocal.InsertarUsuarioDesdeServidor(su);
            else if (local.SyncState != EstadoSync.Pendiente)
                BaseDatosLocal.ActualizarUsuarioDesdeServidor(local, su);
        }
    }
}
