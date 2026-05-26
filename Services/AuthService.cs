using InventarioVentasCS.Data;
using InventarioVentasCS.Models;
using Microsoft.Data.SqlClient;

namespace InventarioVentasCS.Services;

public class AuthService
{
    public Usuario? Login(string nombreUsuario, string clave)
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();

        string sql = "SELECT IdUsuario, Usuario, Rol FROM Usuarios WHERE Usuario=@usuario AND Clave=@clave";
        using SqlCommand cmd = new(sql, cn);
        cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
        cmd.Parameters.AddWithValue("@clave", clave);

        using SqlDataReader dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;

        string rol = dr["Rol"].ToString() ?? "Cajero";
        Usuario usuario = rol switch
        {
            "Administrador" => new Administrador(),
            "Trabajador" => new Trabajador(),
            _ => new Cajero()
        };
        usuario.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
        usuario.NombreUsuario = dr["Usuario"].ToString() ?? "";
        usuario.Rol = rol;
        return usuario;
    }
}
