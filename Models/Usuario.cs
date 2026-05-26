namespace InventarioVentasCS.Models;

public abstract class Usuario
{
    private int idUsuario;
    private string nombreUsuario = string.Empty;
    private string rol = string.Empty;

    public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
    public string Rol { get => rol; set => rol = value; }

    public abstract bool PuedeAdministrarProductos();
    public virtual string MostrarPermisos() => $"Usuario: {NombreUsuario} | Rol: {Rol}";
}

public class Administrador : Usuario
{
    public override bool PuedeAdministrarProductos() => true;
    public override string MostrarPermisos() => $"Administrador: {NombreUsuario} | Permisos completos";
}

public class Cajero : Usuario
{
    public override bool PuedeAdministrarProductos() => false;
    public override string MostrarPermisos() => $"Cajero: {NombreUsuario} | Ventas y reportes";
}

public class Trabajador : Usuario
{
    public override bool PuedeAdministrarProductos() => false;
    public override string MostrarPermisos() => $"Trabajador: {NombreUsuario} | Sin permisos para editar o eliminar";
}
