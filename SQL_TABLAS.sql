USE BD_SIS_DEPORTE;
GO

-----------------------------------------------------
-- 1. TABLA: tbTipoDocumento
-----------------------------------------------------
CREATE TABLE tbTipoDocumento (
    IDTipoDocumento INT IDENTITY(1,1) PRIMARY KEY,
    TipoDocumento VARCHAR(30) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);
GO

INSERT INTO tbTipoDocumento (TipoDocumento)
VALUES 
('DNI'),
('Carnet de Extranjería'),
('Pasaporte');
GO

-----------------------------------------------------
-- 2. TABLA: tbPersona
-----------------------------------------------------
CREATE TABLE tbPersona (
    IDPersona INT IDENTITY(1,1) PRIMARY KEY,
    Nombres VARCHAR(250) NOT NULL,
    ApellidoPaterno VARCHAR(250) NOT NULL,
    ApellidoMaterno VARCHAR(250) NOT NULL,
    IDTipoDocumento INT NOT NULL,
    Documento VARCHAR(15) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
    Telefono VARCHAR(9),
    Correo VARCHAR(250) NOT NULL UNIQUE,
    Genero CHAR(1) CHECK (Genero IN ('M','F')),
    CONSTRAINT FK_Persona_TipoDoc FOREIGN KEY (IDTipoDocumento)
        REFERENCES tbTipoDocumento(IDTipoDocumento)
);
GO

-----------------------------------------------------
-- 3. TABLA: tbUsuario
-----------------------------------------------------
CREATE TABLE tbUsuario (
    IDUsuario INT IDENTITY(1,1) PRIMARY KEY,
    IDPersona INT NOT NULL,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Contraseña VARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME,
    Estado BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Usuario_Persona FOREIGN KEY (IDPersona)
        REFERENCES tbPersona(IDPersona)
);
GO

-----------------------------------------------------
-- 4. TABLA: tbDeporte
-----------------------------------------------------
CREATE TABLE tbDeporte (
    IDDeporte INT IDENTITY(1,1) PRIMARY KEY,
    Deporte VARCHAR(250) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);
GO

INSERT INTO tbDeporte (Deporte)
VALUES 
('Futbol'),
('Voley'),
('Basquet');
GO

-----------------------------------------------------
-- 5. TABLA: tbEquipo
-----------------------------------------------------
CREATE TABLE tbEquipo (
    IDEquipo INT IDENTITY(1,1) PRIMARY KEY,
    IDCreador INT NOT NULL,
    Nombre VARCHAR(150) NOT NULL,
    IDDeporte INT NOT NULL,
    Descripcion VARCHAR(250),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME,
    Estado BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Equipo_Creador FOREIGN KEY (IDCreador)
        REFERENCES tbUsuario(IDUsuario),
    CONSTRAINT FK_Equipo_Deporte FOREIGN KEY (IDDeporte)
        REFERENCES tbDeporte(IDDeporte)
);
GO

-----------------------------------------------------
-- 6. TABLA: tbRol
-----------------------------------------------------
CREATE TABLE tbRol (
    IDRol INT IDENTITY(1,1) PRIMARY KEY,
    Rol VARCHAR(250) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);
GO

INSERT INTO tbRol (Rol)
VALUES 
('Jugador'),
('Capitán');
GO

-----------------------------------------------------
-- 7. TABLA: tbEquipoMiembros
-----------------------------------------------------
CREATE TABLE tbEquipoMiembros (
    IdMiembroEquipo INT IDENTITY(1,1) PRIMARY KEY,
    IDEquipo INT NOT NULL,
    IDJugador INT NOT NULL,
    IDRol INT NOT NULL DEFAULT 1,
    FechaUnion DATETIME NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_EquipoMiembro_Equipo FOREIGN KEY (IDEquipo)
        REFERENCES tbEquipo(IDEquipo),
    CONSTRAINT FK_EquipoMiembro_Jugador FOREIGN KEY (IDJugador)
        REFERENCES tbUsuario(IDUsuario),
    CONSTRAINT FK_EquipoMiembro_Rol FOREIGN KEY (IDRol)
        REFERENCES tbRol(IDRol),
    CONSTRAINT UQ_Equipo_Jugador UNIQUE (IDEquipo, IDJugador)
);
GO

-----------------------------------------------------
-- 8. TABLA: tbInvitacionEquipo
-----------------------------------------------------
CREATE TABLE tbInvitacionEquipo (
    IDInvitacion INT IDENTITY(1,1) PRIMARY KEY,
    IDEquipo INT NOT NULL,
    IDUsuarioEmisor INT NOT NULL,
    IDUsuarioReceptor INT NOT NULL,
    EstadoInvitacion VARCHAR(15) NOT NULL DEFAULT 'PENDIENTE'
        CHECK (EstadoInvitacion IN ('PENDIENTE','ACEPTADA','RECHAZADA')),
    FechaEnvio DATETIME NOT NULL DEFAULT GETDATE(),
    FechaRespuesta DATETIME,
    CONSTRAINT FK_Inv_Equipo FOREIGN KEY (IDEquipo)
        REFERENCES tbEquipo(IDEquipo),
    CONSTRAINT FK_Inv_Emisor FOREIGN KEY (IDUsuarioEmisor)
        REFERENCES tbUsuario(IDUsuario),
    CONSTRAINT FK_Inv_Receptor FOREIGN KEY (IDUsuarioReceptor)
        REFERENCES tbUsuario(IDUsuario),
    CONSTRAINT UQ_Invitacion UNIQUE (IDEquipo, IDUsuarioReceptor, EstadoInvitacion)
);
GO

-----------------------------------------------------
-- 9. TABLA: tbTorneos
-----------------------------------------------------
CREATE TABLE tbTorneos (
    IDTorneos INT IDENTITY(1,1) PRIMARY KEY,
    IDCreador INT NOT NULL,
    Nombre VARCHAR(255) NOT NULL,
    Descripcion VARCHAR(250),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME,
    Estado BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Torneo_Creador FOREIGN KEY (IDCreador)
        REFERENCES tbUsuario(IDUsuario)
);
GO

-----------------------------------------------------
-- 10. TABLA: tbParticipacionTorneo
-----------------------------------------------------
CREATE TABLE tbParticipacionTorneo (
    IDParticipacion INT IDENTITY(1,1) PRIMARY KEY,
    IDTorneo INT NOT NULL,
    IDEquipo INT NOT NULL,
    IDUsuarioEmisor INT NOT NULL,
    EstadoParticipacion VARCHAR(15) NOT NULL DEFAULT 'PENDIENTE'
        CHECK (EstadoParticipacion IN ('PENDIENTE','ACEPTADA','RECHAZADA')),
    FechaInvitacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaRespuesta DATETIME,
    CONSTRAINT FK_Part_Torneo FOREIGN KEY (IDTorneo)
        REFERENCES tbTorneos(IDTorneos),
    CONSTRAINT FK_Part_Equipo FOREIGN KEY (IDEquipo)
        REFERENCES tbEquipo(IDEquipo),
    CONSTRAINT FK_Part_Emisor FOREIGN KEY (IDUsuarioEmisor)
        REFERENCES tbUsuario(IDUsuario),
    CONSTRAINT UQ_Participacion UNIQUE (IDTorneo, IDEquipo)
);
GO

-----------------------------------------------------
-- 11. TABLA: tbEnfrentamientos
-----------------------------------------------------
CREATE TABLE tbEnfrentamientos (
    IDEnfrentamiento INT IDENTITY(1,1) PRIMARY KEY,
    IDTorneo INT NOT NULL,
    IDEquipoLocal INT NOT NULL,
    IDEquipoVisitante INT NOT NULL,
    FechaPartido DATETIME NOT NULL,
    GolesLocal INT DEFAULT 0,
    GolesVisitante INT DEFAULT 0,
    EstadoPartido VARCHAR(15) NOT NULL DEFAULT 'PENDIENTE'
        CHECK (EstadoPartido IN ('PENDIENTE','JUGADO','CANCELADO')),
    
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),

    -----------------------------------------
    -- RELACIONES
    -----------------------------------------
    CONSTRAINT FK_Enfrentamiento_Torneo FOREIGN KEY (IDTorneo)
        REFERENCES tbTorneos(IDTorneos),

    CONSTRAINT FK_Enfrentamiento_Local FOREIGN KEY (IDEquipoLocal)
        REFERENCES tbEquipo(IDEquipo),

    CONSTRAINT FK_Enfrentamiento_Visitante FOREIGN KEY (IDEquipoVisitante)
        REFERENCES tbEquipo(IDEquipo),

    -----------------------------------------
    -- VALIDACIONES IMPORTANTES
    -----------------------------------------

    -- No permitir que un equipo juegue contra sí mismo
    CONSTRAINT CK_Equipos_Distintos CHECK (IDEquipoLocal <> IDEquipoVisitante),

    -- Evitar duplicados: el mismo par de equipos no puede repetirse en el mismo torneo
    CONSTRAINT UQ_Partido UNIQUE (IDTorneo, IDEquipoLocal, IDEquipoVisitante, FechaPartido)
);
GO  

