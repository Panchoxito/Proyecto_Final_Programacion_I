namespace InventarioVentasCS.Models;

public class Producto
{
    private int idProducto;
    private string codigo = string.Empty;
    private string nombre = string.Empty;
    private string categoria = string.Empty;
    private decimal precio;
    private int stock;
    private int stockMinimo;

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Codigo { get => codigo; set => codigo = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Categoria { get => categoria; set => categoria = value; }
    public decimal Precio { get => precio; set => precio = value; }
    public int Stock { get => stock; set => stock = value; }
    public int StockMinimo { get => stockMinimo; set => stockMinimo = value; }

    public bool TieneStockBajo() => Stock <= StockMinimo;

    public override string ToString()
    {
        return $"{Codigo} - {Nombre} | Q{Precio:0.00} | Stock: {Stock}";
    }
}
