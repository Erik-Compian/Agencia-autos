CREATE DATABASE agencia_autos;

USE agencia_autos;

CREATE TABLE Administrador (
    Id_Admin    INT          NOT NULL AUTO_INCREMENT,
    Usuario     VARCHAR(50)  NOT NULL UNIQUE,
    Password    VARCHAR(255) NOT NULL,
    Nombre      VARCHAR(100) NOT NULL,
    Email       VARCHAR(100) NOT NULL UNIQUE,
    CONSTRAINT PK_Administrador PRIMARY KEY (Id_Admin)
);

CREATE TABLE Marca (
    Id_Marca    INT         NOT NULL AUTO_INCREMENT,
    Nombre      VARCHAR(50) NOT NULL UNIQUE,
    CONSTRAINT PK_Marca PRIMARY KEY (Id_Marca)
);

CREATE TABLE Modelo (
    Id_Modelo   INT         NOT NULL AUTO_INCREMENT,
    Id_Marca    INT         NOT NULL,
    Nombre      VARCHAR(50) NOT NULL,
    Anio        YEAR        NOT NULL,
    CONSTRAINT PK_Modelo    PRIMARY KEY (Id_Modelo),
    CONSTRAINT FK_Mod_Marca FOREIGN KEY (Id_Marca) REFERENCES Marca(Id_Marca)
        ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE Cliente (
    Id_Cliente  INT          NOT NULL AUTO_INCREMENT,
    Nombre      VARCHAR(100) NOT NULL,
    Apellido    VARCHAR(100) NOT NULL,
    Telefono    VARCHAR(15)  NOT NULL,
    Email       VARCHAR(100),
    CONSTRAINT PK_Cliente PRIMARY KEY (Id_Cliente)
);

CREATE TABLE Vehiculo (
    Id_Vehiculo INT         NOT NULL AUTO_INCREMENT,
    Id_Cliente  INT         NOT NULL,
    Id_Modelo   INT         NOT NULL,
    Num_Serie   VARCHAR(50) NOT NULL UNIQUE,
    Color       VARCHAR(30),
    Placa       VARCHAR(20) NOT NULL UNIQUE,
    Km_Actual   INT         NOT NULL DEFAULT 0,
    CONSTRAINT PK_Vehiculo    PRIMARY KEY (Id_Vehiculo),
    CONSTRAINT FK_Veh_Cliente FOREIGN KEY (Id_Cliente) REFERENCES Cliente(Id_Cliente)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Veh_Modelo  FOREIGN KEY (Id_Modelo)  REFERENCES Modelo(Id_Modelo)
        ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE Tipo_Servicio (
    Id_Tipo_Serv    INT         NOT NULL AUTO_INCREMENT,
    Nombre          VARCHAR(50) NOT NULL UNIQUE,
    Descripcion     VARCHAR(255),
    CONSTRAINT PK_TipoServicio PRIMARY KEY (Id_Tipo_Serv)
);

CREATE TABLE Servicio (
    Folio           INT          NOT NULL AUTO_INCREMENT,
    Id_Vehiculo     INT          NOT NULL,
    Id_Admin        INT          NOT NULL,
    Id_Tipo_Serv    INT          NOT NULL,
    Quien_Entrego   VARCHAR(100) NOT NULL,
    Fecha_Ingreso   DATE         NOT NULL,
    Fecha_Salida    DATE,
    Estatus         ENUM('En espera','En proceso','Finalizado') NOT NULL DEFAULT 'En espera',
    Descripcion     TEXT,
    CONSTRAINT PK_Servicio     PRIMARY KEY (Folio),
    CONSTRAINT FK_Ser_Vehiculo FOREIGN KEY (Id_Vehiculo)  REFERENCES Vehiculo(Id_Vehiculo)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Ser_Admin    FOREIGN KEY (Id_Admin)     REFERENCES Administrador(Id_Admin)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Ser_TipoServ FOREIGN KEY (Id_Tipo_Serv) REFERENCES Tipo_Servicio(Id_Tipo_Serv)
        ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE Proximo_Servicio (
    Id_Prox_Serv    INT  NOT NULL AUTO_INCREMENT,
    Folio           INT  NOT NULL UNIQUE,
    Fecha_Prog      DATE NOT NULL,
    Km_Proximo      INT,
    Notas           VARCHAR(255),
    CONSTRAINT PK_ProxServicio  PRIMARY KEY (Id_Prox_Serv),
    CONSTRAINT FK_Prox_Servicio FOREIGN KEY (Folio) REFERENCES Servicio(Folio)
        ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE Refaccion (
    Id_Refaccion    INT           NOT NULL AUTO_INCREMENT,
    Nombre          VARCHAR(100)  NOT NULL,
    Codigo          VARCHAR(50)   NOT NULL UNIQUE,
    Precio          DECIMAL(10,2) NOT NULL,
    Stock           INT           NOT NULL DEFAULT 0,
    CONSTRAINT PK_Refaccion PRIMARY KEY (Id_Refaccion)
);

CREATE TABLE Servicio_Refaccion (
    Folio           INT           NOT NULL,
    Id_Refaccion    INT           NOT NULL,
    Cantidad        INT           NOT NULL DEFAULT 1,
    Precio_Aplicado DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_ServRef      PRIMARY KEY (Folio, Id_Refaccion),
    CONSTRAINT FK_SR_Servicio  FOREIGN KEY (Folio)        REFERENCES Servicio(Folio)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT FK_SR_Refaccion FOREIGN KEY (Id_Refaccion) REFERENCES Refaccion(Id_Refaccion)
        ON UPDATE CASCADE ON DELETE RESTRICT
);

INSERT INTO Tipo_Servicio (Nombre, Descripcion) VALUES
    ('Preventivo', 'Mantenimiento programado para prevenir fallas'),
    ('Correctivo', 'Reparación de fallas existentes en el vehículo');
 
INSERT INTO Administrador (Usuario, Password, Nombre, Email) VALUES
    ('admin', '$2a$10$placeholder_hash_cambiar', 'Administrador Principal', 'admin@agencia.com');
 
INSERT INTO Marca (Nombre) VALUES
    ('Toyota'), ('Nissan'), ('Honda'), ('Volkswagen'), ('Chevrolet');
