USE BD_SIS_DEPORTE;
GO

-----------------------------------------------------
-- ACEPTAR INVITACIÓN DE EQUIPO
-----------------------------------------------------
CREATE PROCEDURE sp_AceptarInvitacionEquipo
(
    @IDInvitacion INT,
    @IDUsuarioReceptor INT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IDEquipo INT;

    -- Validar invitación pendiente
    SELECT @IDEquipo = IDEquipo
    FROM tbInvitacionEquipo
    WHERE IDInvitacion = @IDInvitacion
      AND IDUsuarioReceptor = @IDUsuarioReceptor
      AND EstadoInvitacion = 'PENDIENTE';

    IF @IDEquipo IS NULL
    BEGIN
        RAISERROR('La invitación no existe, no pertenece al usuario, o ya fue respondida.', 16, 1);
        RETURN;
    END;

    -- Aceptar invitación
    UPDATE tbInvitacionEquipo
    SET EstadoInvitacion = 'ACEPTADA',
        FechaRespuesta = GETDATE()
    WHERE IDInvitacion = @IDInvitacion;

    -- Insertar en equipo si aún no es miembro
    IF NOT EXISTS (
        SELECT 1 
        FROM tbEquipoMiembros
        WHERE IDEquipo = @IDEquipo
          AND IDJugador = @IDUsuarioReceptor
    )
    BEGIN
        INSERT INTO tbEquipoMiembros (IDEquipo, IDJugador, IDRol)
        VALUES (@IDEquipo, @IDUsuarioReceptor, 1); -- 1 = Jugador
    END
END;
GO

-----------------------------------------------------
-- RECHAZAR INVITACIÓN DE EQUIPO
-----------------------------------------------------
CREATE PROCEDURE sp_RechazarInvitacionEquipo
(
    @IDInvitacion INT,
    @IDUsuarioReceptor INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar que exista la invitación y que esté pendiente
    IF NOT EXISTS (
        SELECT 1
        FROM tbInvitacionEquipo
        WHERE IDInvitacion = @IDInvitacion
          AND IDUsuarioReceptor = @IDUsuarioReceptor
          AND EstadoInvitacion = 'PENDIENTE'
    )
    BEGIN
        RAISERROR('La invitación no existe, no pertenece al usuario, o ya fue respondida.', 16, 1);
        RETURN;
    END;

    -- Rechazar invitación
    UPDATE tbInvitacionEquipo
    SET EstadoInvitacion = 'RECHAZADA',
        FechaRespuesta = GETDATE()
    WHERE IDInvitacion = @IDInvitacion
      AND IDUsuarioReceptor = @IDUsuarioReceptor;
END;
GO

-----------------------------------------------------
-- ELIMINACIÓN LÓGICA DE EQUIPO
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_Equipo_EliminarLogico
(
    @IDEquipo INT
)
AS
BEGIN
    UPDATE tbEquipo
    SET Estado = 0,
        FechaModificacion = GETDATE()
    WHERE IDEquipo = @IDEquipo;
END
GO

-----------------------------------------------------
-- INSERTAR EQUIPO
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_Equipo_Insertar
(
    @IDCreador INT,
    @Nombre VARCHAR(150),
    @IDDeporte INT,
    @Descripcion VARCHAR(250) = NULL
)
AS
BEGIN
    DECLARE @NuevoIDEquipo INT;

    INSERT INTO tbEquipo(IDCreador, Nombre, IDDeporte, Descripcion)
    VALUES (@IDCreador, @Nombre, @IDDeporte, @Descripcion);

    SET @NuevoIDEquipo = SCOPE_IDENTITY();

    INSERT INTO tbEquipoMiembros(IDEquipo, IDJugador, IDRol)
    VALUES (@NuevoIDEquipo, @IDCreador, 2);
END
GO

-----------------------------------------------------
-- LISTAR EQUIPOS ACTIVOS POR USUARIO
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_Equipo_ListarActivosPorUsuario
(
    @IDUsuario INT
)
AS
BEGIN
    SELECT
        E.IDEquipo,
        E.Nombre,
        D.Deporte,
        E.Descripcion,
        E.FechaRegistro
    FROM tbEquipo E
    INNER JOIN tbDeporte D ON E.IDDeporte = D.IDDeporte
    WHERE E.Estado = 1
      AND E.IDCreador = @IDUsuario
    ORDER BY E.FechaRegistro DESC;
END
GO

-----------------------------------------------------
-- LISTAR INTEGRANTES DE EQUIPO
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_Equipo_ListarIntegrantes
    @IDEquipo INT
AS
BEGIN
    SELECT
        U.IDUsuario,
        U.NombreUsuario,
        R.Rol,
        EM.FechaUnion
    FROM tbEquipoMiembros EM
    INNER JOIN tbUsuario U ON EM.IDJugador = U.IDUsuario
    INNER JOIN tbRol R ON EM.IDRol = R.IDRol
    WHERE EM.IDEquipo = @IDEquipo
      AND EM.Estado = 1
    ORDER BY EM.FechaUnion ASC;
END
GO

-----------------------------------------------------
-- INSERTAR INVITACIÓN
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_InvitacionEquipo_Insertar
(
    @IDEquipo INT,
    @IDUsuarioEmisor INT,
    @IDUsuarioReceptor INT
)
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM tbInvitacionEquipo
        WHERE IDEquipo = @IDEquipo
          AND IDUsuarioReceptor = @IDUsuarioReceptor
          AND EstadoInvitacion = 'PENDIENTE'
    )
    BEGIN
        RAISERROR('Ya existe una invitación pendiente para este usuario en el equipo.', 16, 1);
        RETURN;
    END;

    INSERT INTO tbInvitacionEquipo(IDEquipo, IDUsuarioEmisor, IDUsuarioReceptor)
    VALUES (@IDEquipo, @IDUsuarioEmisor, @IDUsuarioReceptor);
END
GO

-----------------------------------------------------
-- LISTAR INVITACIONES
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_InvitacionEquipo_Listar
(
    @IDUsuario INT
)
AS
BEGIN
    SELECT
        I.IDInvitacion,
        E.Nombre AS NombreEquipo,
        D.Deporte,
        Ue.NombreUsuario AS UsuarioEmisor,
        I.FechaEnvio,
        I.EstadoInvitacion
    FROM tbInvitacionEquipo I
    INNER JOIN tbEquipo E ON I.IDEquipo = E.IDEquipo
    INNER JOIN tbDeporte D ON E.IDDeporte = D.IDDeporte
    INNER JOIN tbUsuario Ue ON I.IDUsuarioEmisor = Ue.IDUsuario
    WHERE 
        I.IDUsuarioReceptor = @IDUsuario
    ORDER BY I.FechaEnvio DESC;
END
GO

-----------------------------------------------------
-- LISTAR USUARIOS
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_ListarUsuarios
AS
BEGIN
    SELECT 
        u.IDUsuario AS ID,
        CONCAT(p.Nombres, ' ', p.ApellidoPaterno, ' ' , p.ApellidoMaterno) AS NombreCompleto,
        u.NombreUsuario AS Usuario,
        p.Correo
    FROM tbUsuario u
    INNER JOIN tbPersona p ON u.IDPersona = p.IDPersona;
END
GO

-----------------------------------------------------
-- LISTAR USUARIOS PARA INVITACIÓN
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_ListarUsuariosParaInvitacion
(
    @Busqueda NVARCHAR(100) = NULL
)
AS
BEGIN
    SELECT 
        u.IDUsuario AS ID,
        CONCAT(p.Nombres, ' ', p.ApellidoPaterno, ' ', p.ApellidoMaterno) AS NombreCompleto,
        u.NombreUsuario AS Usuario,
        p.Correo
    FROM tbUsuario u
    INNER JOIN tbPersona p ON u.IDPersona = p.IDPersona
    WHERE (@Busqueda IS NULL
           OR u.NombreUsuario LIKE '%' + @Busqueda + '%'
           OR p.Nombres LIKE '%' + @Busqueda + '%'
           OR p.ApellidoPaterno LIKE '%' + @Busqueda + '%'
           OR p.ApellidoMaterno LIKE '%' + @Busqueda + '%');
END
GO

-----------------------------------------------------
-- LOGIN USUARIO
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_LoginUsuario
    @Usuario VARCHAR(50) = NULL,
    @Correo VARCHAR(250) = NULL
AS
BEGIN
    SELECT 
        u.IDUsuario AS ID,
        p.Correo,
        u.NombreUsuario AS Usuario,
        u.Contraseña
    FROM tbUsuario u
    INNER JOIN tbPersona p ON u.IDPersona = p.IDPersona
    WHERE 
        (@Usuario IS NOT NULL AND u.NombreUsuario = @Usuario)
        OR
        (@Correo IS NOT NULL AND p.Correo = @Correo);
END
GO

-----------------------------------------------------
-- MODIFICAR EQUIPO
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_ModificarEquipo
    @IDEquipo INT,
    @NombreEquipo VARCHAR(150),
    @Descripcion VARCHAR(300)
AS
BEGIN
    UPDATE tbEquipo
    SET 
        Nombre = @NombreEquipo,
        Descripcion = @Descripcion
    WHERE IDEquipo = @IDEquipo;
END
GO

-----------------------------------------------------
-- REGISTRAR USUARIO
-----------------------------------------------------
CREATE PROCEDURE dbo.sp_RegistrarUsuario
    @Nombres            VARCHAR(250),
    @ApellidoPaterno    VARCHAR(250),
    @ApellidoMaterno    VARCHAR(250),
    @IDTipoDocumento    INT,
    @Documento          VARCHAR(15),
    @FechaNacimiento    DATE,
    @Telefono           VARCHAR(9),
    @Correo             VARCHAR(250),
    @Genero             CHAR(1),
    @NombreUsuario      VARCHAR(50),
    @ContraseñaHash     VARCHAR(255)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO tbPersona(
            Nombres, ApellidoPaterno, ApellidoMaterno,
            IDTipoDocumento, Documento, FechaNacimiento,
            Telefono, Correo, Genero
        )
        VALUES (
            @Nombres, @ApellidoPaterno, @ApellidoMaterno,
            @IDTipoDocumento, @Documento, @FechaNacimiento,
            @Telefono, @Correo, @Genero
        );

        DECLARE @IDPersona INT = SCOPE_IDENTITY();

        INSERT INTO tbUsuario(IDPersona, NombreUsuario, Contraseña)
        VALUES (@IDPersona, @NombreUsuario, @ContraseñaHash);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
GO

-----------------------------------------------------
-- CREAR TORNEO
-----------------------------------------------------
CREATE PROCEDURE sp_Torneo_Insertar
(
    @IDCreador INT,
    @Nombre VARCHAR(255),
    @Descripcion VARCHAR(250)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tbTorneos (IDCreador, Nombre, Descripcion)
    VALUES (@IDCreador, @Nombre, @Descripcion);
END;
GO

-----------------------------------------------------
-- LISTAR TORNEOS DEL USUARIO
-----------------------------------------------------

CREATE PROCEDURE sp_Torneos_ListarActivosPorUsuario
(
    @IDUsuario INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        T.IDTorneos,
        T.Nombre,
        T.Descripcion,
        T.FechaRegistro,
        U.NombreUsuario AS Creador
    FROM tbTorneos T
    INNER JOIN tbUsuario U ON T.IDCreador = U.IDUsuario
    WHERE 
        T.Estado = 1
        AND T.IDCreador = @IDUsuario
    ORDER BY T.FechaRegistro DESC;
END;
GO

-----------------------------------------------------
-- MODIFICAR TORNEOS DEL USUARIO
-----------------------------------------------------

CREATE PROCEDURE sp_ModificarTorneo
(
    @IDTorneos INT,
    @IDCreador INT,
    @Nombre VARCHAR(255),
    @Descripcion VARCHAR(250)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE tbTorneos
    SET 
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        FechaModificacion = GETDATE()
    WHERE 
        IDTorneos = @IDTorneos
        AND IDCreador = @IDCreador;
END
GO

-----------------------------------------------------
-- ELIMINAR TORNEOS DEL USUARIO
-----------------------------------------------------

CREATE PROCEDURE sp_Torneo_EliminarLogico
(
    @IDTorneo INT,
    @IDUsuario INT
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Solo el creador puede eliminar su torneo
    UPDATE tbTorneos
    SET Estado = 0,
        FechaModificacion = GETDATE()
    WHERE IDTorneos = @IDTorneo
      AND IDCreador = @IDUsuario;
END;
GO

-----------------------------------------------------
-- BUSCAR EQUIPOS PARA INVITAR
-----------------------------------------------------
CREATE PROCEDURE spBuscarEquiposPorNombre
    @Nombre NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        e.IDEquipo,
        e.Nombre AS NombreEquipo,
        u.NombreUsuario AS NombreCapitan,
        d.Deporte AS Deporte,
        CASE 
            WHEN e.Estado = 1 THEN 'Activo'
            ELSE 'Inactivo'
        END AS EstadoParticipacion
    FROM tbEquipo e
    INNER JOIN tbUsuario u ON e.IDCreador = u.IDUsuario
    INNER JOIN tbDeporte d ON e.IDDeporte = d.IDDeporte
    WHERE e.Nombre LIKE '%' + @Nombre + '%';
END
GO


-----------------------------------------------------
-- LISTAR EQUIPOS PARTICIPANTES
-----------------------------------------------------
CREATE PROCEDURE uspTorneo_ListarParticipantes
    @IDTorneo INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        tp.IDEquipo,
        e.Nombre AS NombreEquipo,
        e.Descripcion,
        u.NombreCompleto AS NombreCapitan,
        d.NombreDeporte AS Deporte,
        CASE tp.Estado
            WHEN 1 THEN 'Activo'
            WHEN 0 THEN 'Retirado'
        END AS EstadoParticipacion
    FROM tbTorneoParticipantes tp
        INNER JOIN tbEquipo e ON tp.IDEquipo = e.IDEquipo
        INNER JOIN tbUsuario u ON e.IDCreador = u.IDUsuario
        INNER JOIN tbDeporte d ON e.IDDeporte = d.IDDeporte
    WHERE tp.IDTorneo = @IDTorneo
    ORDER BY e.Nombre;
END;
GO

-----------------------------------------------------
-- INVITAR EQUIPOS AL TORNEO
-----------------------------------------------------
CREATE PROCEDURE spInvitarEquipoATorneo
(
    @IDTorneo INT,
    @IDEquipo INT,
    @IDUsuarioEmisor INT
)
AS
BEGIN
    -- Verificar si ya existe una invitación
    IF EXISTS (
        SELECT 1 
        FROM tbParticipacionTorneo 
        WHERE IDTorneo = @IDTorneo AND IDEquipo = @IDEquipo
    )
    BEGIN
        RAISERROR('El equipo ya tiene una invitación o ya participa en el torneo.', 16, 1);
        RETURN;
    END;

    -- Insertar invitación
    INSERT INTO tbParticipacionTorneo (IDTorneo, IDEquipo, IDUsuarioEmisor)
    VALUES (@IDTorneo, @IDEquipo, @IDUsuarioEmisor);
END;
GO


-----------------------------------------------------
-- LISTAR INVITAR EQUIPOS AL TORNEO
-----------------------------------------------------
CREATE PROCEDURE spListarInvitacionesDeEquipos
(
    @IDUsuario INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.IDParticipacion,
        t.Nombre AS Torneo,
        e.Nombre AS Equipo,
        ue.NombreUsuario AS InvitaUsuario,
        p.FechaInvitacion AS EnviadaEl,
        p.EstadoParticipacion
    FROM tbParticipacionTorneo p
    INNER JOIN tbEquipo e ON p.IDEquipo = e.IDEquipo
    INNER JOIN tbUsuario u ON e.IDCreador = u.IDUsuario
    INNER JOIN tbUsuario ue ON p.IDUsuarioEmisor = ue.IDUsuario
    INNER JOIN tbTorneos t ON p.IDTorneo = t.IDTorneos
    WHERE e.IDCreador = @IDUsuario -- recibe invitaciones
    ORDER BY p.FechaInvitacion DESC;
END
GO
