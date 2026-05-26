CREATE DATABASE InventarioVentas;
GO
USE InventarioVentas;
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Usuario VARCHAR(50) NOT NULL UNIQUE,
    Clave VARCHAR(50) NOT NULL,
    Rol VARCHAR(20) NOT NULL
);

CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(30) NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    Categoria VARCHAR(50) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    StockMinimo INT NOT NULL DEFAULT 5,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE Ventas (
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    IdUsuario INT NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    Descuento DECIMAL(10,2) NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE DetalleVenta (
    IdDetalle INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (IdVenta) REFERENCES Ventas(IdVenta),
    FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
);

CREATE TABLE MovimientosInventario (
    IdMovimiento INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto INT NOT NULL,
    TipoMovimiento VARCHAR(20) NOT NULL,
    Cantidad INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Descripcion VARCHAR(200),
    FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto)
);

INSERT INTO Usuarios (Usuario, Clave, Rol)
VALUES ('admin', '1234', 'Administrador'), ('cajero', '1234', 'Cajero'), ('trabajador', '1234', 'Trabajador');

INSERT INTO Productos (Codigo, Nombre, Categoria, Precio, Stock, StockMinimo)
VALUES
('P001', 'Cuaderno Universitario', 'Librería', 18.50, 20, 5),
('P002', 'Lapicero Azul', 'Librería', 2.50, 50, 10),
('P003', 'Borrador', 'Librería', 3.00, 8, 5);
