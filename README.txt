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

CÓMO ABRIR:
1. Descomprime la carpeta.
2. Abre InventarioVentasCS.csproj con Visual Studio.
3. Restaura los paquetes NuGet si Visual Studio lo solicita.
4. Ejecuta el proyecto.

FORMULARIOS:
- FrmLogin: inicio de sesión y validación de rol.
- FrmMenu: menú principal.
- FrmProductos: registro, edición, eliminación, búsqueda y movimientos.
- FrmVentas: venta con varios productos, descuento, stock y ticket TXT.
- FrmReportes: ventas por día, productos más vendidos y exportación TXT.

NOTA:
Si tu instancia no es localhost\SQLEXPRESS, cambia la cadena en Data/Conexion.cs.


Actualización:
- Usuario trabajador agregado: trabajador / 1234.
- El rol Trabajador puede consultar y vender, pero no puede editar ni eliminar productos.
- Los reportes ahora se pueden descargar como HTML o CSV desde el formulario Reportes.

Si ya tenías la base creada, ejecuta en SSMS:
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Usuario='trabajador')
    INSERT INTO Usuarios (Usuario, Clave, Rol) VALUES ('trabajador', '1234', 'Trabajador');
