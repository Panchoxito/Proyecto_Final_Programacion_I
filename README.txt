PROYECTO: Sistema de Inventario y Ventas
Tecnologías: C# Windows Forms, SQL Server Express, SSMS

REQUISITOS:
1. Tener instalado SQL Server Express.
2. Tener instalada la base de datos InventarioVentas.
3. Tener Visual Studio 2022 con carga de trabajo .NET Desktop Development.

CONEXIÓN:
Server=localhost\SQLEXPRESS;Database=InventarioVentas;Trusted_Connection=True;TrustServerCertificate=True;

USUARIOS DE PRUEBA:
Administrador:
Usuario: admin
Clave: 1234

Cajero:
Usuario: cajero
Clave: 1234


FORMULARIOS:
- FrmLogin: inicio de sesión y validación de rol.
- FrmMenu: menú principal.
- FrmProductos: registro, edición, eliminación, búsqueda y movimientos.
- FrmVentas: venta con varios productos, descuento, stock y ticket TXT.
- FrmReportes: ventas por día, productos más vendidos y exportación TXT.

