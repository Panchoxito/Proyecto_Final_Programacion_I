using InventarioVentasCS.Data;
using InventarioVentasCS.Models;
using Microsoft.Data.SqlClient;

namespace InventarioVentasCS.Services;

public class VentaService
{
    public int RegistrarVenta(Venta venta)
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();
        using SqlTransaction tx = cn.BeginTransaction();

        try
        {
            foreach (var d in venta.Detalles)
            {
                using SqlCommand validar = new("SELECT Stock FROM Productos WHERE IdProducto=@id", cn, tx);
                validar.Parameters.AddWithValue("@id", d.IdProducto);
                int stockActual = Convert.ToInt32(validar.ExecuteScalar());
                if (stockActual < d.Cantidad)
                    throw new Exception($"No hay suficiente stock para {d.Nombre}. Stock actual: {stockActual}");
            }

            string sqlVenta = @"INSERT INTO Ventas(IdUsuario, Subtotal, Descuento, Total)
                                OUTPUT INSERTED.IdVenta
                                VALUES(@idUsuario,@subtotal,@descuento,@total)";
            using SqlCommand cmdVenta = new(sqlVenta, cn, tx);
            cmdVenta.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);
            cmdVenta.Parameters.AddWithValue("@subtotal", venta.CalcularSubtotal());
            cmdVenta.Parameters.AddWithValue("@descuento", venta.CalcularDescuento());
            cmdVenta.Parameters.AddWithValue("@total", venta.CalcularTotal());
            int idVenta = Convert.ToInt32(cmdVenta.ExecuteScalar());

            foreach (var d in venta.Detalles)
            {
                using SqlCommand cmdDetalle = new(@"INSERT INTO DetalleVenta(IdVenta,IdProducto,Cantidad,PrecioUnitario,Subtotal)
                                                   VALUES(@idVenta,@idProducto,@cantidad,@precio,@subtotal)", cn, tx);
                cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);
                cmdDetalle.Parameters.AddWithValue("@idProducto", d.IdProducto);
                cmdDetalle.Parameters.AddWithValue("@cantidad", d.Cantidad);
                cmdDetalle.Parameters.AddWithValue("@precio", d.PrecioUnitario);
                cmdDetalle.Parameters.AddWithValue("@subtotal", d.Subtotal);
                cmdDetalle.ExecuteNonQuery();

                using SqlCommand cmdStock = new("UPDATE Productos SET Stock = Stock - @cantidad WHERE IdProducto=@idProducto", cn, tx);
                cmdStock.Parameters.AddWithValue("@cantidad", d.Cantidad);
                cmdStock.Parameters.AddWithValue("@idProducto", d.IdProducto);
                cmdStock.ExecuteNonQuery();

                using SqlCommand cmdMov = new(@"INSERT INTO MovimientosInventario(IdProducto,TipoMovimiento,Cantidad,Descripcion)
                                                VALUES(@idProducto,'Salida',@cantidad,@descripcion)", cn, tx);
                cmdMov.Parameters.AddWithValue("@idProducto", d.IdProducto);
                cmdMov.Parameters.AddWithValue("@cantidad", d.Cantidad);
                cmdMov.Parameters.AddWithValue("@descripcion", $"Venta No. {idVenta}");
                cmdMov.ExecuteNonQuery();
            }

            tx.Commit();
            return idVenta;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}
