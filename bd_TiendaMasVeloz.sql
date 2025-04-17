-- Crear la base de datos
DROP DATABASE IF EXISTS TiendaMasVeloz;
CREATE DATABASE TiendaMasVeloz;
USE TiendaMasVeloz;

-- Tabla Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    NombreUsuario VARCHAR(50) NOT NULL,
	Correo VARCHAR(100) NOT NULL,
    Contraseña VARCHAR(100) NOT NULL,
    Rol VARCHAR(20) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion DATETIME NULL
);

-- Tabla Categorias
CREATE TABLE Categorias (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200),
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla Proveedores
CREATE TABLE Proveedores (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Nombre VARCHAR(100) NOT NULL,
    Contacto VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Direccion VARCHAR(200) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla Productos
CREATE TABLE Productos (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Codigo VARCHAR(20) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500),
    PrecioCosto DECIMAL(18,2) NOT NULL,
    PrecioVenta DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    CategoriaId INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);

-- Tabla Empleados
CREATE TABLE Empleados (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Cedula VARCHAR(20) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Telefono VARCHAR(20),
    Direccion VARCHAR(200),
    Cargo VARCHAR(50) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CedulaNit VARCHAR(20) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100),
    Documento VARCHAR(20),
    Email VARCHAR(100),
    Telefono VARCHAR(20),
    Direccion VARCHAR(200),
    Incentivos INT NOT NULL DEFAULT 0,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla Facturas
CREATE TABLE Facturas (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Numero VARCHAR(20) NOT NULL,
    IdEmpleado INT NOT NULL,
    IdCliente INT NOT NULL,
    Fecha DATETIME NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    IVA DECIMAL(18,2) NOT NULL,
    Total DECIMAL(18,2) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaModificacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(Id),
    FOREIGN KEY (IdCliente) REFERENCES Clientes(Id)
);

-- Tabla DetalleFacturas
CREATE TABLE DetalleFacturas (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    FacturaId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (FacturaId) REFERENCES Facturas(Id),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
);

-- Insertar datos de prueba  
INSERT INTO Usuarios (NombreUsuario, Contraseña, Rol) VALUES
('admin', 'admin123', 'Administrador'),
('vendedor', 'vendedor123', 'Vendedor');

INSERT INTO Categorias (Nombre, Descripcion) VALUES
('Bicicletas', 'Todo tipo de bicicletas'),
('Repuestos', 'Repuestos para bicicletas'),
('Accesorios', 'Accesorios para ciclistas');

INSERT INTO Proveedores (Nombre, Contacto, Telefono, Direccion, Email) VALUES
('Shimano', 'Juan Pérez', '3001234567', 'Calle 1 #1-1', 'shimano@email.com'),
('GW', 'María López', '3009876543', 'Calle 2 #2-2', 'gw@email.com');

INSERT INTO Productos (Codigo, Nombre, Descripcion, PrecioCosto, PrecioVenta, Stock, CategoriaId) VALUES
('BIC001', 'Bicicleta Mountain Bike', 'Bicicleta todo terreno', 500000, 750000, 10, 1),
('BIC002', 'Bicicleta Ruta', 'Bicicleta de ruta profesional', 800000, 1200000, 5, 1),
('REP001', 'Cadena Shimano', 'Cadena de alta resistencia', 50000, 75000, 20, 2),
('ACC001', 'Casco Protector', 'Casco de seguridad', 30000, 45000, 15, 3);

INSERT INTO Empleados (Cedula, Nombre, Apellido, Email, Telefono, Direccion, Cargo) VALUES
('1234567890', 'Juan', 'Pérez', 'juan@email.com', '3001234567', 'Calle 1 #1-1', 'Vendedor'),
('0987654321', 'María', 'López', 'maria@email.com', '3009876543', 'Calle 2 #2-2', 'Administrador');

INSERT INTO Clientes (CedulaNit, Nombre, Apellido, Email, Telefono, Direccion) VALUES
('1111111111', 'Pedro', 'Gómez', 'pedro@email.com', '3002223333', 'Calle 3 #3-3'),
('2222222222', 'Ana', 'Martínez', 'ana@email.com', '3004445555', 'Calle 4 #4-4');