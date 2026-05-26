using System.Data;
using InventarioVentasCS.Data;
using InventarioVentasCS.Models;
using Microsoft.Data.SqlClient;

namespace InventarioVentasCS.Services;

public class ProductoService
{
    public DataTable Listar(string filtro = "")
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();
        string sql = @"SELECT IdProducto, Codigo, Nombre, Categoria, Precio, Stock, StockMinimo
                       FROM Productos
                       WHERE Estado=1 AND (Codigo LIKE @filtro OR Nombre LIKE @filtro)
                       ORDER BY Nombre";
        using SqlCommand cmd = new(sql, cn);
        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
        using SqlDataAdapter da = new(cmd);
        DataTable dt = new();
        da.Fill(dt);
        return dt;
    }

    public Producto? BuscarPorCodigo(string codigo)
    {
        return BuscarPorCodigoONombre(codigo);
    }

    public Producto? BuscarPorCodigoONombre(string busqueda)
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();
        string sql = @"SELECT TOP 1 *
                       FROM Productos
                       WHERE Estado=1
                         AND (Codigo=@busqueda OR LOWER(Nombre)=LOWER(@busqueda) OR Nombre LIKE @filtro)
                       ORDER BY
                         CASE
                            WHEN Codigo=@busqueda THEN 1
                            WHEN LOWER(Nombre)=LOWER(@busqueda) THEN 2
                            ELSE 3
                         END, Nombre";
        using SqlCommand cmd = new(sql, cn);
        cmd.Parameters.AddWithValue("@busqueda", busqueda.Trim());
        cmd.Parameters.AddWithValue("@filtro", "%" + busqueda.Trim() + "%");
        using SqlDataReader dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;
        return new Producto
        {
            IdProducto = Convert.ToInt32(dr["IdProducto"]),
            Codigo = dr["Codigo"].ToString() ?? "",
            Nombre = dr["Nombre"].ToString() ?? "",
            Categoria = dr["Categoria"].ToString() ?? "",
            Precio = Convert.ToDecimal(dr["Precio"]),
            Stock = Convert.ToInt32(dr["Stock"]),
            StockMinimo = Convert.ToInt32(dr["StockMinimo"])
        };
    }

    public string ObtenerSiguienteCodigo()
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();

        int numero = 1;
        using SqlCommand cmd = new("SELECT Codigo FROM Productos WHERE Codigo LIKE 'P%'", cn);
        using SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            string codigo = dr["Codigo"].ToString() ?? "";
            if (codigo.Length > 1 && int.TryParse(codigo[1..], out int actual) && actual >= numero)
                numero = actual + 1;
        }
        dr.Close();

        string nuevoCodigo;
        do
        {
            nuevoCodigo = $"P{numero:0000}";
            using SqlCommand verificar = new("SELECT COUNT(*) FROM Productos WHERE Codigo=@codigo", cn);
            verificar.Parameters.AddWithValue("@codigo", nuevoCodigo);
            int existe = Convert.ToInt32(verificar.ExecuteScalar());
            if (existe == 0) return nuevoCodigo;
            numero++;
        } while (true);
    }

    public void Guardar(Producto p)
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();

        using SqlCommand verificar = new("SELECT COUNT(*) FROM Productos WHERE Estado=1 AND LOWER(Nombre)=LOWER(@nombre) AND IdProducto<>@id", cn);
        verificar.Parameters.AddWithValue("@nombre", p.Nombre.Trim());
        verificar.Parameters.AddWithValue("@id", p.IdProducto);
        int repetidos = Convert.ToInt32(verificar.ExecuteScalar());
        if (repetidos > 0)
            throw new InvalidOperationException("Ya existe un producto activo con ese mismo nombre. No se puede duplicar.");

        if (p.IdProducto == 0)
            p.Codigo = ObtenerSiguienteCodigo();

        using SqlCommand verificarCodigo = new("SELECT COUNT(*) FROM Productos WHERE Codigo=@codigo AND IdProducto<>@id", cn);
        verificarCodigo.Parameters.AddWithValue("@codigo", p.Codigo.Trim());
        verificarCodigo.Parameters.AddWithValue("@id", p.IdProducto);
        int codigosRepetidos = Convert.ToInt32(verificarCodigo.ExecuteScalar());
        if (codigosRepetidos > 0)
            throw new InvalidOperationException("Ya existe un producto con ese código. El sistema generará uno nuevo para evitar duplicados.");

        string sql = p.IdProducto == 0
            ? "INSERT INTO Productos(Codigo,Nombre,Categoria,Precio,Stock,StockMinimo) VALUES(@codigo,@nombre,@categoria,@precio,@stock,@minimo)"
            : "UPDATE Productos SET Codigo=@codigo, Nombre=@nombre, Categoria=@categoria, Precio=@precio, Stock=@stock, StockMinimo=@minimo WHERE IdProducto=@id";
        using SqlCommand cmd = new(sql, cn);
        cmd.Parameters.AddWithValue("@id", p.IdProducto);
        cmd.Parameters.AddWithValue("@codigo", p.Codigo);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@categoria", p.Categoria);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@stock", p.Stock);
        cmd.Parameters.AddWithValue("@minimo", p.StockMinimo);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int idProducto)
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();
        using SqlCommand cmd = new("UPDATE Productos SET Estado=0 WHERE IdProducto=@id", cn);
        cmd.Parameters.AddWithValue("@id", idProducto);
        cmd.ExecuteNonQuery();
    }

    public void RegistrarMovimiento(int idProducto, string tipo, int cantidad, string descripcion)
    {
        using SqlConnection cn = Conexion.ObtenerConexion();
        cn.Open();
        string sql = "INSERT INTO MovimientosInventario(IdProducto, TipoMovimiento, Cantidad, Descripcion) VALUES(@id,@tipo,@cantidad,@descripcion)";
        using SqlCommand cmd = new(sql, cn);
        cmd.Parameters.AddWithValue("@id", idProducto);
        cmd.Parameters.AddWithValue("@tipo", tipo);
        cmd.Parameters.AddWithValue("@cantidad", cantidad);
        cmd.Parameters.AddWithValue("@descripcion", descripcion);
        cmd.ExecuteNonQuery();
    }
}
