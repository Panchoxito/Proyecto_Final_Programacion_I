namespace InventarioVentasCS.Models;

public class Venta
{
    public int IdVenta { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int IdUsuario { get; set; }
    public List<DetalleVenta> Detalles { get; set; } = new();

    public decimal CalcularSubtotal()
    {
        decimal subtotal = 0;
        foreach (DetalleVenta detalle in Detalles)
            subtotal += detalle.Subtotal;
        return subtotal;
    }

    public decimal CalcularDescuento()
    {
        decimal subtotal = CalcularSubtotal();
        return subtotal >= 100 ? subtotal * 0.05m : 0;
    }

    public decimal CalcularTotal() => CalcularSubtotal() - CalcularDescuento();
}
