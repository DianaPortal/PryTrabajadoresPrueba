CREATE DATABASE TrabajadoresPrueba;
GO
USE TrabajadoresPrueba;
GO

-- Create Table
CREATE TABLE Trabajadores(
	IdTrb VARCHAR(5) primary key,
	Nombres NVARCHAR(100) not null,
    Apellidos NVARCHAR(100) not null,
    TipoDocumento NVARCHAR(20) not null, -- DNI, Pasaporte, etc.
    NumeroDocumento NVARCHAR(20) not null unique,
    Sexo CHAR(1) not null, -- 'M' o 'F'
    FechaNacimiento DATE not null,
    FotoUrl NVARCHAR(255) not null, -- Guardaremos la ruta, no el binario
    Direccion NVARCHAR(200) not null,
	Activo BIT DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

--Secuencia
CREATE SEQUENCE SeqTrabajadores
    START WITH 1
    INCREMENT BY 1;
GO
--Store Procedure

--Listado
CREATE OR ALTER PROCEDURE sp_ListarTrabajadores
    @NombreBusqueda NVARCHAR(100) = NULL,
    @SexoBusqueda CHAR(1) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdTrb, Nombres, Apellidos, TipoDocumento, NumeroDocumento, 
        Sexo, FechaNacimiento, FotoUrl, Direccion, Activo, FechaRegistro
    FROM Trabajadores
    WHERE Activo = 1
    AND (@NombreBusqueda IS NULL OR 
         Nombres LIKE '%' + @NombreBusqueda + '%' OR 
         Apellidos LIKE '%' + @NombreBusqueda + '%')
    AND (@SexoBusqueda IS NULL OR Sexo = @SexoBusqueda)
    ORDER BY FechaRegistro DESC; 
END;
GO

-- Registro
CREATE OR ALTER PROCEDURE sp_RegistrarTrabajador
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @TipoDocumento NVARCHAR(20),
    @NumeroDocumento NVARCHAR(20),
    @Sexo CHAR(1),
    @FechaNacimiento DATE,
    @FotoUrl NVARCHAR(255),
    @Direccion NVARCHAR(200)
AS
BEGIN
    DECLARE @Codigo VARCHAR(5);

    SET @Codigo = 'T' + RIGHT('0000' + CAST(NEXT VALUE FOR SeqTrabajadores AS VARCHAR(4)), 4);

    INSERT INTO Trabajadores (IdTrb, Nombres, Apellidos, TipoDocumento, NumeroDocumento, Sexo, FechaNacimiento, FotoUrl, Direccion)
    VALUES (@Codigo, @Nombres, @Apellidos, @TipoDocumento, @NumeroDocumento, @Sexo, @FechaNacimiento, @FotoUrl, @Direccion);
END;
GO

CREATE OR ALTER PROCEDURE sp_EditarTrabajador
    @IdTrb VARCHAR(5),
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @TipoDocumento NVARCHAR(20),
    @NumeroDocumento NVARCHAR(20),
    @Sexo CHAR(1),
    @FechaNacimiento DATE,
    @FotoUrl NVARCHAR(255),
    @Direccion NVARCHAR(200)
AS
BEGIN
    UPDATE Trabajadores
    SET Nombres = @Nombres,
        Apellidos = @Apellidos,
        TipoDocumento = @TipoDocumento,
        NumeroDocumento = @NumeroDocumento,
        Sexo = @Sexo,
        FechaNacimiento = @FechaNacimiento,
        FotoUrl = @FotoUrl,
        Direccion = @Direccion
    WHERE IdTrb = @IdTrb;
END;
GO

--Eliminar
CREATE OR ALTER PROCEDURE sp_EliminarTrabajador
    @IdTrb VARCHAR(5)
AS
BEGIN
    UPDATE Trabajadores
    SET Activo = 0
    WHERE IdTrb = @IdTrb;
END;
GO


select * from Trabajadores;