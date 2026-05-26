using System.Data;
using InventarioVentasCS.Data;
using Microsoft.Data.SqlClient;

namespace InventarioVentasCS.Services;

public class ReporteService
{
    public DataTable VentasPorDia(DateTime fecha)
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();
        string sql = @"SELECT V.IdVenta, V.Fecha, U.Usuario, V.Subtotal, V.Descuento, V.Total
                       FROM Ventas V INNER JOIN Usuarios U ON V.IdUsuario = U.IdUsuario
                       WHERE CONVERT(date, V.Fecha) = @fecha
                       ORDER BY V.Fecha DESC";
        using SqlCommand cmd = new(sql, cn);
        cmd.Parameters.AddWithValue("@fecha", fecha.Date);
        using SqlDataAdapter da = new(cmd);
        DataTable dt = new();
        da.Fill(dt);
        return dt;
    }

    public DataTable ProductosMasVendidos()
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();
        string sql = @"SELECT TOP 10 P.Codigo, P.Nombre, SUM(D.Cantidad) AS CantidadVendida, SUM(D.Subtotal) AS TotalVendido
                       FROM DetalleVenta D INNER JOIN Productos P ON D.IdProducto = P.IdProducto
                       GROUP BY P.Codigo, P.Nombre
                       ORDER BY CantidadVendida DESC";
        using SqlCommand cmd = new(sql, cn);
        using SqlDataAdapter da = new(cmd);
        DataTable dt = new();
        da.Fill(dt);
        return dt;
    }
}
