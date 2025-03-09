-------------------------------------------------------------------------------
-- 0) CREAR BASE DE DATOS Y USARLA
-------------------------------------------------------------------------------
CREATE DATABASE EcommerceDB;

USE EcommerceDB;


-------------------------------------------------------------------------------
-- 1) TABLA USUARIO (con EsAdmin para distinguir roles y nuevos campos)
-------------------------------------------------------------------------------
CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    PasswordSalt NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    EsAdmin BIT NOT NULL DEFAULT 0,
    Telefono NVARCHAR(20) NULL,
    Direccion NVARCHAR(255) NULL,
    ResetToken NVARCHAR(255) NULL,
    ResetTokenExpiration DATETIME NULL
);

-------------------------------------------------------------------------------
-- 2) INSERTAR USUARIOS DE EJEMPLO
-------------------------------------------------------------------------------
INSERT INTO Usuario (Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro, EsAdmin, Telefono, Direccion)
VALUES
    ('Admin Local', 'admin@correo.com', 'INSERT-HASH-BASE64', 'INSERT-SALT-BASE64', GETDATE(), 1, '666111222', 'Calle Falsa 123, Ciudad'),
    ('Usuario Normal', 'user@correo.com', 'INSERT-HASH-BASE64', 'INSERT-SALT-BASE64', GETDATE(), 0, '655222333', 'Avenida Siempre Viva 742, Springfield');

-------------------------------------------------------------------------------
-- 3) TABLA CATEGORIA
-------------------------------------------------------------------------------
CREATE TABLE Categoria (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
    Nombre NVARCHAR(100) NOT NULL,  
    Descripcion NVARCHAR(500),
    UrlImagen NVARCHAR(255) NOT NULL
);

-------------------------------------------------------------------------------
-- 4) TABLA PRODUCTO (relación con CATEGORIA)
-------------------------------------------------------------------------------
CREATE TABLE Producto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    CategoriaId INT NOT NULL, 
    UrlImagen NVARCHAR(255),
    Descripcion NVARCHAR(500),
    CONSTRAINT FK_Producto_Categoria
        FOREIGN KEY (CategoriaId) REFERENCES Categoria(Id)
);

-------------------------------------------------------------------------------
-- 5) INSERTAR CATEGORÍAS
-------------------------------------------------------------------------------
INSERT INTO Categoria (Nombre, Descripcion, UrlImagen)
VALUES
    ('Textil', 'Productos relacionados con textiles, como sábanas, toallas, cortinas, alfombras, etc.', 'https://www.happers.es/server/Portal_0010674/img/blogposts/guia-de-textiles-para-el-hogar-como-combinar-y-cuidar_6979.jpg'),
    ('Decoración vertical', 'Productos para decoración de paredes, como cuadros, espejos, vinilos, etc.', 'https://images.pexels.com/photos/15743386/pexels-photo-15743386/free-photo-of-lamparas-mesa-alfombra-salon.jpeg'),
    ('Accesorio decorativo', 'Productos que mejoran la estética de los espacios, como lámparas, jarrones, etc.', 'https://ixia.es/media/wysiwyg/2023-03/02-salon-straight-line-2023-05-11.jpg');

-------------------------------------------------------------------------------
-- 6) INSERTAR PRODUCTOS para la categoría 'Textil' (Id=1)
-------------------------------------------------------------------------------
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('ALFOMBRA MULTICOLOR LANA-ALGODÓN 160 X 230 CM', 199.99, 1, 'https://ixia.es/media/catalog/product/6/1/614920.jpg', 'Alfombra multicolor de lana y algodón, 160 x 230 cm.'),
    ('ALFOMBRA BLANCO NATURAL LANA-ALGODÓN 200 X 300 CM', 249.99, 1, 'https://ixia.es/media/catalog/product/6/1/614918.jpg', 'Alfombra de color blanco natural, lana y algodón, 200 x 300 cm.'),
    ('ALFOMBRA MARRÓN POLIPROPILENO 160 X 230 CM', 129.99, 1, 'https://ixia.es/media/catalog/product/6/1/612752.jpg', 'Alfombra marrón de polipropileno, 160 x 230 cm. Resistente y fácil de mantener.'),
    ('ALFOMBRA GRIS POLIÉSTER-ALGODÓN 80 X 150 CM', 59.99, 1, 'https://ixia.es/media/catalog/product/6/0/609529.jpg', 'Alfombra gris de poliéster y algodón, 80 x 150 cm.'),
    ('ALFOMBRA NATURAL YUTE 180 X 180 X 1 CM', 119.99, 1, 'https://ixia.es/media/catalog/product/1/0/106830.jpg', 'Alfombra natural de yute, 180 x 180 x 1 cm.'),
    ('ALFOMBRA CONEJO GRIS ALGODÓN INFANTIL 100 CM', 39.99, 1, 'https://ixia.es/media/catalog/product/6/0/608553.jpg', 'Alfombra infantil con diseño de conejo, diámetro de 100 cm.'),
    ('ALFOMBRA OSO MARRÓN ALGODÓN INFANTIL 100 CM', 39.99, 1, 'https://ixia.es/media/catalog/product/6/0/608552.jpg', 'Alfombra infantil de algodón con diseño de oso marrón, diámetro de 100 cm.'),
    ('COJÍN VERDE ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614934.jpg', 'Cojín verde de algodón y poliéster, 50 x 30 cm.'),
    ('COJÍN TIERRA ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614935.jpg', 'Cojín en tonos tierra, 50 x 30 cm.'),
    ('COJÍN BLANCO ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614933.jpg', 'Cojín blanco de algodón y poliéster, 50 x 30 cm.'),
    ('COJÍN CREMA JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615076.jpg', 'Cojín jacquard crema, 45 x 45 cm.'),
    ('COJÍN BLANCO/BEIGE JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615080.jpg', 'Cojín blanco/beige jacquard, 45 x 45 cm.'),
    ('COJÍN BLANCO-VERDE JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615072.jpg', 'Cojín blanco-verde jacquard, 45 x 45 cm.');

-------------------------------------------------------------------------------
-- 7) INSERTAR PRODUCTOS para la categoría 'Decoración vertical' (Id=2)
-------------------------------------------------------------------------------
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('CUADRO PINTURA ABSTRACTO LIENZO 60 X 150 CM', 159.99, 2, 'https://ixia.es/media/catalog/product/6/1/615166.jpg', 'Cuadro de pintura abstracto en lienzo, 60 x 150 cm.'),
    ('CUADRO PINTURA ABSTRACTO 44 X 50 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/1/615155.jpg', 'Cuadro de pintura abstracto, 44 x 50 cm.'),
    ('CUADRO PINTURA ABSTRACTO 60 X 80 CM', 129.99, 2, 'https://ixia.es/media/catalog/product/6/1/615159.jpg', 'Cuadro de pintura abstracto, 60 x 80 cm.'),
    ('CUADRO PINTURA ABSTRACTO 70 X 70 CM', 149.99, 2, 'https://ixia.es/media/catalog/product/6/1/615162.jpg', 'Cuadro de pintura abstracto, 70 x 70 cm.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 120 X 150 CM', 179.99, 2, 'https://ixia.es/media/catalog/product/6/1/615168.jpg', 'Pintura abstracta en lienzo, 120 x 150 cm.'),
    ('CUADRO PINTURA ABSTRACTO 52 X 62 CM', 99.99, 2, 'https://ixia.es/media/catalog/product/6/1/615144.jpg', 'Cuadro de pintura abstracto, 52 x 62 cm.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 120 X 3 X 60 CM', 189.99, 2, 'https://ixia.es/media/catalog/product/6/1/614404.jpg', 'Pintura abstracta en lienzo, 120 x 3 x 60 cm.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 50 X 120 CM', 139.99, 2, 'https://ixia.es/media/catalog/product/6/1/614411.jpg', 'Pintura abstracta en lienzo, 50 x 120 cm.'),
    ('CUADRO PINTURA ABSTRACTO 50 X 4 X 50 CM', 109.99, 2, 'https://ixia.es/media/catalog/product/6/1/612473.jpg', 'Cuadro de pintura abstracto, 50 x 4 x 50 cm.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 100 X 100 CM', 169.99, 2, 'https://ixia.es/media/catalog/product/6/1/614413.jpg', 'Pintura abstracta en lienzo, 100 x 100 cm.'),
    ('PINTURA MUJER LIENZO DECORACIÓN 80 X 3,50 X 80 CM', 199.99, 2, 'https://ixia.es/media/catalog/product/6/1/610822.jpg', 'Pintura de mujer en lienzo, 80 x 3,50 x 80 cm.'),
    ('PINTURA AFRICANA LIENZO 90 X 3,50 X 120 CM', 219.99, 2, 'https://ixia.es/media/catalog/product/6/1/613361.jpg', 'Pintura africana en lienzo, 90 x 3,50 x 120 cm.'),
    ('ESPEJO DM-CRISTAL DECORACIÓN 45 X 1 X 45 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/0/608790.jpg', 'Espejo decorativo en DM y cristal, 45 x 1 x 45 cm.'),
    ('ESPEJO NEGRO METAL-CRISTAL DECORACIÓN 82 X 7 X 113 CM', 349.99, 2, 'https://ixia.es/media/catalog/product/6/1/613600.jpg', 'Espejo de metal negro, 82 x 7 x 113 cm.'),
    ('ESPEJO MARCO DORADO MADERA DE PINO 65 X 65 CM', 149.99, 2, 'https://ixia.es/media/catalog/product/6/1/610510.jpg', 'Espejo con marco dorado de madera de pino, 65 x 65 cm.');

-------------------------------------------------------------------------------
-- 8) INSERTAR PRODUCTOS para la categoría 'Accesorio decorativo' (Id=3)
-------------------------------------------------------------------------------
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('FIGURA ABSTRACTA POLIRESINA 27,50 X 14,50 X 29 CM', 49.99, 3, 'https://ixia.es/media/catalog/product/6/1/613483.jpg', 'Figura abstracta en poliresina.'),
    ('FIGURA CÍRCULO BLANCO-ORO RESINA 45 X 10 X 57 CM', 79.99, 3, 'https://ixia.es/media/catalog/product/6/1/610484.jpg', 'Figura de círculo blanco y oro en resina.'),
    ('ESCULTURA ANILLO NATURAL-BLANCO 25,50 X 9,50 X 37 CM', 89.99, 3, 'https://ixia.es/media/catalog/product/6/0/609776.jpg', 'Escultura de anillo natural con detalles en blanco.'),
    ('FIGURA NATURAL-NEGRO MADERA-HIERRO 17 X 17 X 31 CM', 59.99, 3, 'https://ixia.es/media/catalog/product/6/0/606256.jpg', 'Figura natural en madera y hierro.'),
    ('FIGURA ABSTRACTA NATURAL MADERA DE MANGO 15 X 9 X 68,50 CM', 129.99, 3, 'https://ixia.es/media/catalog/product/6/0/609768.jpg', 'Figura abstracta en madera de mango.'),
    ('ESCULTURA NATURAL MADERA DE MANGO 38 X 8 X 52 CM', 149.99, 3, 'https://ixia.es/media/catalog/product/6/0/609762.jpg', 'Escultura de madera de mango.'),
    ('FIGURA AFRICANO NATURAL MADERA DE MANGO 14 X 14 X 88,50 CM', 199.99, 3, 'https://ixia.es/media/catalog/product/6/0/609765.jpg', 'Figura africana en madera de mango natural.'),
    ('CAJA DORADO METAL 31 X 21,50 X 35,50 CM', 69.99, 3, 'https://ixia.es/media/catalog/product/6/1/613122.jpg', 'Caja decorativa dorada de metal.'),
    ('FIGURA MUJER COBRE RESINA 18 X 16 X 63 CM', 79.99, 3, 'https://ixia.es/media/catalog/product/6/1/610476.jpg', 'Figura de mujer en resina color cobre.'),
    ('ESCULTURA PERSONAS COBRE RESINA 40 X 10,50 X 34 CM', 99.99, 3, 'https://ixia.es/media/catalog/product/6/1/610487.jpg', 'Escultura de personas en resina color cobre.'),
    ('FIGURA PAVO REAL ORO METAL 50 X 30 X 85 CM', 129.99, 3, 'https://ixia.es/media/catalog/product/6/1/610280.jpg', 'Figura de pavo real en metal dorado.'),
    ('FIGURA FLAMENCO ORO METAL 25 X 21 X 85 CM', 109.99, 3, 'https://ixia.es/media/catalog/product/6/1/610281.jpg', 'Figura de flamenco en metal dorado.'),
    ('FIGURA ÁRBOL ORO METAL 26 X 26 X 83 CM', 119.99, 3, 'https://ixia.es/media/catalog/product/6/1/613945.jpg', 'Figura de árbol en metal dorado.'),
    ('CORONA BLANCO MADERA PAULONIA 52 X 15 X 52 CM', 89.99, 3, 'https://ixia.es/media/catalog/product/6/1/610781.jpg', 'Corona decorativa en madera paulownia blanca.'),
    ('FIGURA ÁNGEL NATURAL MADERA DE MANGO 22,50 X 9 X 84,50 CM', 159.99, 3, 'https://ixia.es/media/catalog/product/6/0/609758.jpg', 'Figura de ángel en madera de mango natural.'),
    ('FIGURA GALLINA BLANCO ROZADO METAL 34 X 12 X 38 CM', 49.99, 3, 'https://ixia.es/media/catalog/product/6/1/613871.jpg', 'Figura de gallina en metal blanco rozado.'),
    ('FIGURA PEZ POLIRESINA 25,70 X 14 X 35,70 CM', 69.99, 3, 'https://ixia.es/media/catalog/product/6/1/613467.jpg', 'Figura de pez en poliresina.'),
    ('FIGURA PEZ MADERA-FIBRA NATURAL 70 X 12 X 53 CM', 89.99, 3, 'https://ixia.es/media/catalog/product/6/0/609795.jpg', 'Figura de pez en madera-fibra natural.'),
    ('FIGURA PECES MADERA-HIERRO 56 X 7 X 31 CM', 99.99, 3, 'https://ixia.es/media/catalog/product/6/0/609802.jpg', 'Figura de peces en madera-hierro.');


-------------------------------------------------------------------------------
-- 9) TABLA PEDIDO (relación con USUARIO)
-------------------------------------------------------------------------------
CREATE TABLE Pedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    FechaPedido DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_Pedido_Usuario
        FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-------------------------------------------------------------------------------
-- 10) TABLA DETALLEPEDIDO (relación con PEDIDO y PRODUCTO)
-------------------------------------------------------------------------------
CREATE TABLE DetallePedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_DetallePedido_Pedido
        FOREIGN KEY (PedidoId) REFERENCES Pedido(Id),
    CONSTRAINT FK_DetallePedido_Producto
        FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);

-------------------------------------------------------------------------------
-- 11) TABLA RESEÑA (relación con PRODUCTO y USUARIO)
-------------------------------------------------------------------------------
CREATE TABLE Reseña (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductoId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(1000),
    Calificacion INT CHECK (Calificacion BETWEEN 1 AND 5),
    CONSTRAINT FK_Reseña_Producto
        FOREIGN KEY (ProductoId) REFERENCES Producto(Id),
    CONSTRAINT FK_Reseña_Usuario
        FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-------------------------------------------------------------------------------
-- 12) EJEMPLOS DE INSERTS EN PEDIDO, DETALLEPEDIDO, RESEÑA (DESCOMENTA SI QUIERES PROBAR)
-------------------------------------------------------------------------------
/*
INSERT INTO Pedido (UsuarioId, Total) VALUES (1, 799.99);

INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario)
VALUES (1, 1, 1, 799.99);

INSERT INTO Reseña (ProductoId, UsuarioId, Comentario, Calificacion)
VALUES (1, 1, 'Muy bueno', 5);
*/

-------------------------------------------------------------------------------
-- CONSULTA DE EJEMPLO
-------------------------------------------------------------------------------
-- SELECT * FROM Usuario;
-- SELECT * FROM Categoria;
-- SELECT * FROM Producto;
-- SELECT * FROM Pedido;
-- SELECT * FROM DetallePedido;
-- SELECT * FROM Reseña;
