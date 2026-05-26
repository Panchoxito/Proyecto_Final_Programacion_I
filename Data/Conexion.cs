using Microsoft.Data.SqlClient;

namespace InventarioVentasCS.Data;

public static class Conexion
{
    public static readonly string Cadena = @"Server=localhost\SQLEXPRESS;Database=InventarioVentas;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(Cadena);
    }
}
