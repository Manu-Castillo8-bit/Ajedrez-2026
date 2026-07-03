create database ajedrez;
use ajedrez;


-- ============================================
-- 1. TABLA DE USUARIOS
-- ============================================
CREATE TABLE usuarios (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    nombre_usuario VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    contrasena VARCHAR(255) NOT NULL,
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP,
    elo INT DEFAULT 1200,
    partidas_jugadas INT DEFAULT 0,
    partidas_ganadas INT DEFAULT 0,
    partidas_empatadas INT DEFAULT 0,
    partidas_perdidas INT DEFAULT 0,
    activo BOOLEAN DEFAULT TRUE
);

-- ============================================
-- 2. TABLA DE PARTIDAS
-- ============================================
CREATE TABLE partidas (
    id_partida INT PRIMARY KEY AUTO_INCREMENT,
    jugador_blancas INT NOT NULL,
    jugador_negras INT NOT NULL,
    ganador INT NULL,
    estado ENUM('en_curso', 'finalizada', 'abandonada') DEFAULT 'en_curso',
    modo ENUM('local', 'ia') DEFAULT 'local',
    fecha_inicio DATETIME DEFAULT CURRENT_TIMESTAMP,
    fecha_fin DATETIME NULL,
    FOREIGN KEY (jugador_blancas) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (jugador_negras) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (ganador) REFERENCES usuarios(id_usuario)
);

-- ============================================
-- 3. TABLA DE MOVIMIENTOS
-- ============================================
CREATE TABLE movimientos (
    id_movimiento INT PRIMARY KEY AUTO_INCREMENT,
    id_partida INT NOT NULL,
    numero_movimiento INT NOT NULL,
    movimiento VARCHAR(10) NOT NULL,
    turno ENUM('blancas', 'negras') NOT NULL,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_partida) REFERENCES partidas(id_partida) ON DELETE CASCADE
);

-- ============================================
-- 4. TABLA DE PARTIDAS GUARDADAS
-- ============================================
CREATE TABLE partidas_guardadas (
    id_guardado INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT NOT NULL,
    id_partida INT NOT NULL,
    nombre_guardado VARCHAR(50) DEFAULT 'Partida sin nombre',
    fecha_guardado DATETIME DEFAULT CURRENT_TIMESTAMP,
    posicion_fen TEXT NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_partida) REFERENCES partidas(id_partida)
);

-- ============================================
-- 5. TABLA DE PREFERENCIAS
-- ============================================
CREATE TABLE preferencias (
    id_preferencia INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT NOT NULL UNIQUE,
    tema VARCHAR(20) DEFAULT 'claro',
    sonido BOOLEAN DEFAULT TRUE,
    mostrar_coordenadas BOOLEAN DEFAULT TRUE,
    idioma VARCHAR(10) DEFAULT 'es',
    FOREIGN KEY (id_usuario)
        REFERENCES usuarios (id_usuario)
        ON DELETE CASCADE
);




/*
-- ============================================
-- ÍNDICES PARA RENDIMIENTO (OPCIONAL)
-- ============================================
CREATE INDEX idx_usuarios_email ON usuarios(email);
CREATE INDEX idx_usuarios_nombre ON usuarios(nombre_usuario);
CREATE INDEX idx_partidas_estado ON partidas(estado);
CREATE INDEX idx_partidas_fecha ON partidas(fecha_inicio);
CREATE INDEX idx_partidas_jugadores ON partidas(jugador_blancas, jugador_negras);
CREATE INDEX idx_movimientos_partida ON movimientos(id_partida);
CREATE INDEX idx_guardadas_usuario ON partidas_guardadas(id_usuario);
*/
