using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SQLite;

namespace Ajedrez
{
    /// <summary>Estados de sincronización de una fila local.</summary>
    public static class EstadoSync
    {
        public const string Pendiente = "pendiente";
        public const string Sincronizada = "sincronizada";
        public const string Eliminado = "eliminado";
    }

    // ── ENTIDADES LOCALES (espejo de las tablas de Supabase) ──

    [Table("partida_local")]
    public class PartidaLocal
    {
        [PrimaryKey, AutoIncrement]
        public int IdLocal { get; set; }
        public int? ServerId { get; set; }
        public string JugadorBlancas { get; set; }
        public string JugadorNegras { get; set; }
        public string Ganador { get; set; }
        public string Estado { get; set; }
        public string Modo { get; set; }
        public bool Soplar { get; set; }
        public string TipoJuego { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string SyncState { get; set; } = EstadoSync.Pendiente;
        public DateTime Modificado { get; set; } = DateTime.Now;
    }

    [Table("movimiento_local")]
    public class MovimientoLocal
    {
        [PrimaryKey, AutoIncrement]
        public int IdLocal { get; set; }
        public int? ServerId { get; set; }
        public int IdPartidaLocal { get; set; }
        public int NumMovimientos { get; set; }
        public string TurnoFinal { get; set; }
        public string MovimientoFinal { get; set; }
        public string Tablero { get; set; }
        public string SyncState { get; set; } = EstadoSync.Pendiente;
        public DateTime Modificado { get; set; } = DateTime.Now;
    }

    [Table("usuario_local")]
    public class UsuarioLocal
    {
        [PrimaryKey, AutoIncrement]
        public int IdLocal { get; set; }
        public int? ServerId { get; set; }
        public string NombreUsuario { get; set; }
        public double Elo { get; set; }
        public int PartidasJugadas { get; set; }
        public int PartidasGanadas { get; set; }
        public int PartidasEmpatadas { get; set; }
        public int PartidasPerdidas { get; set; }
        public string SyncState { get; set; } = EstadoSync.Sincronizada;
        public DateTime Modificado { get; set; } = DateTime.Now;
    }

    [Table("credencial_local")]
    public class CredencialLocal
    {
        [PrimaryKey]
        public string NombreUsuario { get; set; }
        public string HashContrasena { get; set; }
        public DateTime Modificado { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Base de datos local (SQLite). Es la fuente de verdad de la app: todas las
    /// lecturas se resuelven aquí y las escrituras se marcan como pendientes
    /// hasta que el <see cref="Sincronizador"/> las envía a Supabase. Si SQLite no
    /// está disponible, todos los métodos degradan de forma segura (devolviendo
    /// null/vacío) para que la app siga funcionando contra la nube.
    /// </summary>
    public static class BaseDatosLocal
    {
        private const string NombreDb = "ajedrez_offline.db3";
        private static readonly object Cerrojo = new object();
        private static SQLiteConnection _conexion;
        private static bool _intentado;

        public static string RutaDb
        {
            get
            {
                string carpeta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ajedrez");
                Directory.CreateDirectory(carpeta);
                return Path.Combine(carpeta, NombreDb);
            }
        }

        private static SQLiteConnection Conexion()
        {
            if (_conexion != null) return _conexion;

            lock (Cerrojo)
            {
                if (_conexion != null) return _conexion;
                if (_intentado) return _conexion;
                _intentado = true;

                try
                {
                    if (!SQLiteNativo.Asegurar()) return null;

                    var db = new SQLiteConnection(RutaDb,
                        SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
                    db.CreateTable<PartidaLocal>();
                    db.CreateTable<MovimientoLocal>();
                    db.CreateTable<UsuarioLocal>();
                    db.CreateTable<CredencialLocal>();
                    _conexion = db;
                }
                catch
                {
                    _conexion = null;
                }

                return _conexion;
            }
        }

        public static bool Disponible { get { return Conexion() != null; } }

        /// <summary>Ejecuta una consulta bajo el cerrojo de la base, sin lanzar.</summary>
        public static T Con<T>(Func<SQLiteConnection, T> accion, T porDefecto)
        {
            try
            {
                var db = Conexion();
                if (db == null) return porDefecto;
                lock (Cerrojo) { return accion(db); }
            }
            catch
            {
                return porDefecto;
            }
        }

        public static void Con(Action<SQLiteConnection> accion)
        {
            Con(db => { accion(db); return 0; }, 0);
        }

        // ── PARTIDAS ──

        public static int InsertarPartida(PartidaLocal p)
        {
            return Con(db =>
            {
                p.IdLocal = 0;
                if (string.IsNullOrEmpty(p.SyncState)) p.SyncState = EstadoSync.Pendiente;
                p.Modificado = DateTime.Now;
                db.Insert(p);
                return p.IdLocal;
            }, -1);
        }

        public static void ActualizarPartida(PartidaLocal p)
        {
            Con(db =>
            {
                p.Modificado = DateTime.Now;
                db.Update(p);
            });
        }

        public static PartidaLocal PartidaPorIdLocal(int idLocal)
        {
            return Con(db => db.Table<PartidaLocal>().FirstOrDefault(x => x.IdLocal == idLocal), null);
        }

        public static PartidaLocal PartidaPorServerId(int serverId)
        {
            return Con(db => db.Table<PartidaLocal>().FirstOrDefault(x => x.ServerId == serverId), null);
        }

        public static List<PartidaLocal> ListarPartidas(string tipo, string usuario)
        {
            return Con(db =>
            {
                if (string.IsNullOrWhiteSpace(usuario)) return new List<PartidaLocal>();
                var q = db.Table<PartidaLocal>()
                          .Where(p => p.JugadorBlancas == usuario || p.JugadorNegras == usuario);
                if (!string.IsNullOrWhiteSpace(tipo))
                    q = q.Where(p => p.TipoJuego == tipo);
                return q.ToList()
                        .OrderByDescending(p => p.FechaInicio ?? DateTime.MinValue)
                        .ToList();
            }, new List<PartidaLocal>());
        }

        public static PartidaLocal BuscarPartidaEnCurso(string tipo, string usuario)
        {
            return Con(db =>
            {
                if (string.IsNullOrWhiteSpace(usuario)) return null;
                var q = db.Table<PartidaLocal>()
                          .Where(p => p.Estado == "en_curso"
                                   && (p.JugadorBlancas == usuario || p.JugadorNegras == usuario));
                if (!string.IsNullOrWhiteSpace(tipo))
                    q = q.Where(p => p.TipoJuego == tipo);
                return q.ToList()
                        .OrderByDescending(p => p.FechaInicio ?? DateTime.MinValue)
                        .FirstOrDefault();
            }, null);
        }

        public static List<PartidaLocal> PartidasPendientes()
        {
            return Con(db => db.Table<PartidaLocal>()
                               .Where(p => p.SyncState == EstadoSync.Pendiente)
                               .ToList(),
                       new List<PartidaLocal>());
        }

        public static void MarcarPartidaSincronizada(int idLocal, int serverId)
        {
            Con(db =>
            {
                var p = db.Table<PartidaLocal>().FirstOrDefault(x => x.IdLocal == idLocal);
                if (p == null) return;
                p.ServerId = serverId;
                p.SyncState = EstadoSync.Sincronizada;
                p.Modificado = DateTime.Now;
                db.Update(p);
            });
        }

        // ── MOVIMIENTOS ──

        public static MovimientoLocal MovimientoPorPartida(int idPartidaLocal)
        {
            return Con(db => db.Table<MovimientoLocal>()
                               .FirstOrDefault(m => m.IdPartidaLocal == idPartidaLocal), null);
        }

        public static MovimientoLocal MovimientoPorServerId(int serverId)
        {
            return Con(db => db.Table<MovimientoLocal>().FirstOrDefault(m => m.ServerId == serverId), null);
        }

        /// <summary>Inserta o reemplaza la posición guardada de una partida local.</summary>
        public static void GuardarMovimiento(int idPartidaLocal, int numMovimientos, string turnoFinal,
                                             string movimientoFinal, string tableroJson)
        {
            Con(db =>
            {
                var m = db.Table<MovimientoLocal>().FirstOrDefault(x => x.IdPartidaLocal == idPartidaLocal);
                bool nuevo = m == null;
                if (nuevo) m = new MovimientoLocal { IdPartidaLocal = idPartidaLocal };

                m.NumMovimientos = numMovimientos;
                m.TurnoFinal = turnoFinal ?? "";
                m.MovimientoFinal = movimientoFinal ?? "";
                if (!string.IsNullOrWhiteSpace(tableroJson)) m.Tablero = tableroJson;
                m.SyncState = EstadoSync.Pendiente;
                m.Modificado = DateTime.Now;

                if (nuevo) db.Insert(m); else db.Update(m);
            });
        }

        public static List<MovimientoLocal> MovimientosPendientes()
        {
            return Con(db => db.Table<MovimientoLocal>()
                               .Where(m => m.SyncState == EstadoSync.Pendiente)
                               .ToList(),
                       new List<MovimientoLocal>());
        }

        public static void MarcarMovimientoSincronizado(int idLocal, int serverId)
        {
            Con(db =>
            {
                var m = db.Table<MovimientoLocal>().FirstOrDefault(x => x.IdLocal == idLocal);
                if (m == null) return;
                m.ServerId = serverId;
                m.SyncState = EstadoSync.Sincronizada;
                m.Modificado = DateTime.Now;
                db.Update(m);
            });
        }

        // ── USUARIOS / ELO ──

        public static UsuarioLocal ObtenerUsuario(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return null;
            return Con(db => db.Table<UsuarioLocal>().FirstOrDefault(u => u.NombreUsuario == nombre), null);
        }

        public static UsuarioLocal UsuarioPorServerId(int serverId)
        {
            return Con(db => db.Table<UsuarioLocal>().FirstOrDefault(u => u.ServerId == serverId), null);
        }

        public static void GuardarUsuario(UsuarioLocal u)
        {
            Con(db =>
            {
                var existente = db.Table<UsuarioLocal>().FirstOrDefault(x => x.NombreUsuario == u.NombreUsuario);
                u.Modificado = DateTime.Now;
                if (existente != null)
                {
                    u.IdLocal = existente.IdLocal;
                    db.Update(u);
                }
                else
                {
                    u.IdLocal = 0;
                    db.Insert(u);
                }
            });
        }

        public static List<UsuarioLocal> UsuariosPendientes()
        {
            return Con(db => db.Table<UsuarioLocal>()
                               .Where(u => u.SyncState == EstadoSync.Pendiente)
                               .ToList(),
                       new List<UsuarioLocal>());
        }

        public static void MarcarUsuarioSincronizado(int idLocal, int serverId)
        {
            Con(db =>
            {
                var u = db.Table<UsuarioLocal>().FirstOrDefault(x => x.IdLocal == idLocal);
                if (u == null) return;
                if (serverId > 0) u.ServerId = serverId;
                u.SyncState = EstadoSync.Sincronizada;
                u.Modificado = DateTime.Now;
                db.Update(u);
            });
        }

        // ── DATOS QUE LLEGAN DEL SERVIDOR (PULL) ──

        public static void InsertarPartidaDesdeServidor(PartidaRepository.Partida sp)
        {
            if (sp == null) return;
            Con(db => db.Insert(new PartidaLocal
            {
                ServerId = sp.id_partida,
                JugadorBlancas = sp.jugador_blancas,
                JugadorNegras = sp.jugador_negras,
                Ganador = sp.ganador,
                Estado = sp.estado,
                Modo = sp.modo,
                Soplar = sp.soplar,
                TipoJuego = sp.tipo_juego,
                IdUsuario = sp.id_usuario,
                FechaInicio = sp.fecha_inicio,
                FechaFin = sp.fecha_fin,
                SyncState = EstadoSync.Sincronizada,
                Modificado = DateTime.Now
            }));
        }

        public static void ActualizarPartidaDesdeServidor(PartidaLocal local, PartidaRepository.Partida sp)
        {
            if (local == null || sp == null) return;
            Con(db =>
            {
                local.JugadorBlancas = sp.jugador_blancas;
                local.JugadorNegras = sp.jugador_negras;
                local.Ganador = sp.ganador;
                local.Estado = sp.estado;
                local.Modo = sp.modo;
                local.Soplar = sp.soplar;
                local.TipoJuego = sp.tipo_juego;
                local.IdUsuario = sp.id_usuario;
                local.FechaInicio = sp.fecha_inicio;
                local.FechaFin = sp.fecha_fin;
                local.SyncState = EstadoSync.Sincronizada;
                local.Modificado = DateTime.Now;
                db.Update(local);
            });
        }

        public static void InsertarMovimientoDesdeServidor(int idPartidaLocal, PartidaRepository.Movimiento sm)
        {
            if (sm == null) return;
            Con(db => db.Insert(new MovimientoLocal
            {
                ServerId = sm.id_movimiento,
                IdPartidaLocal = idPartidaLocal,
                NumMovimientos = sm.num_movimientos,
                TurnoFinal = sm.turno_final,
                MovimientoFinal = sm.movimiento_final,
                Tablero = sm.tablero != null ? sm.tablero.ToString(Newtonsoft.Json.Formatting.None) : null,
                SyncState = EstadoSync.Sincronizada,
                Modificado = DateTime.Now
            }));
        }

        public static void ActualizarMovimientoDesdeServidor(MovimientoLocal local, PartidaRepository.Movimiento sm)
        {
            if (local == null || sm == null) return;
            Con(db =>
            {
                local.NumMovimientos = sm.num_movimientos;
                local.TurnoFinal = sm.turno_final;
                local.MovimientoFinal = sm.movimiento_final;
                local.Tablero = sm.tablero != null ? sm.tablero.ToString(Newtonsoft.Json.Formatting.None) : null;
                local.SyncState = EstadoSync.Sincronizada;
                local.Modificado = DateTime.Now;
                db.Update(local);
            });
        }

        public static void InsertarUsuarioDesdeServidor(UsuarioNube su)
        {
            if (su == null) return;
            Con(db => db.Insert(new UsuarioLocal
            {
                ServerId = su.id_usuario,
                NombreUsuario = su.nombre_usuario,
                Elo = su.elo,
                PartidasJugadas = su.partidas_jugadas,
                PartidasGanadas = su.partidas_ganadas,
                PartidasEmpatadas = su.partidas_empatadas,
                PartidasPerdidas = su.partidas_perdidas,
                SyncState = EstadoSync.Sincronizada,
                Modificado = DateTime.Now
            }));
        }

        public static void ActualizarUsuarioDesdeServidor(UsuarioLocal local, UsuarioNube su)
        {
            if (local == null || su == null) return;
            Con(db =>
            {
                local.ServerId = su.id_usuario;
                local.Elo = su.elo;
                local.PartidasJugadas = su.partidas_jugadas;
                local.PartidasGanadas = su.partidas_ganadas;
                local.PartidasEmpatadas = su.partidas_empatadas;
                local.PartidasPerdidas = su.partidas_perdidas;
                local.SyncState = EstadoSync.Sincronizada;
                local.Modificado = DateTime.Now;
                db.Update(local);
            });
        }

        // ── CREDENCIALES (login sin conexión) ──

        public static void GuardarCredencial(string nombre, string hash)
        {
            if (string.IsNullOrWhiteSpace(nombre) || hash == null) return;
            Con(db =>
            {
                var fila = new CredencialLocal
                {
                    NombreUsuario = nombre,
                    HashContrasena = hash,
                    Modificado = DateTime.Now
                };
                db.InsertOrReplace(fila);
            });
        }

        public static bool ValidarCredencial(string nombre, string hash)
        {
            if (string.IsNullOrWhiteSpace(nombre) || hash == null) return false;
            var fila = Con(db => db.Table<CredencialLocal>()
                                   .FirstOrDefault(c => c.NombreUsuario == nombre), null);
            if (fila == null) return false;
            return string.Equals(fila.HashContrasena, hash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Hash SHA-256 de una contraseña (no se guarda texto plano).</summary>
        public static string HashContrasena(string contrasena)
        {
            if (contrasena == null) contrasena = "";
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                var sb = new StringBuilder();
                foreach (byte b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
