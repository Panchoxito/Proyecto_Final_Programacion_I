using System.Text;
using InventarioVentasCS.Models;

namespace InventarioVentasCS.Services;

public class TicketService
{
    public string GuardarTicket(int idVenta, Venta venta, string usuario, string? rutaPersonalizada = null)
    {
        string ruta;
        if (string.IsNullOrWhiteSpace(rutaPersonalizada))
        {
            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tickets");
            Directory.CreateDirectory(carpeta);
            ruta = Path.Combine(carpeta, $"Ticket_Venta_{idVenta}.html");
        }
        else
        {
            string? carpeta = Path.GetDirectoryName(rutaPersonalizada);
            if (!string.IsNullOrWhiteSpace(carpeta))
                Directory.CreateDirectory(carpeta);

            ruta = rutaPersonalizada;
        }

        StringBuilder html = new();
        html.AppendLine("<!DOCTYPE html><html lang='es'><head><meta charset='UTF-8'>");
        html.AppendLine("<title>Ticket de venta</title>");
        html.AppendLine("<style>");
        html.AppendLine("body{font-family:Segoe UI,Arial;background:#f5f7fa;margin:35px;color:#212529}");
        html.AppendLine(".ticket{max-width:780px;margin:auto;background:white;border-radius:12px;padding:26px;box-shadow:0 6px 20px rgba(0,0,0,.12)}");
        html.AppendLine(".encabezado{background:#0a2342;color:white;padding:20px;border-radius:9px;text-align:center}");
        html.AppendLine(".encabezado h1{margin:0;font-size:28px}.encabezado p{margin:6px 0 0;font-size:14px}");
        html.AppendLine(".datos{display:grid;grid-template-columns:1fr 1fr;gap:10px;margin:22px 0;background:#f0f5fb;border-radius:9px;padding:16px}");
        html.AppendLine(".dato strong{color:#0a2342}table{width:100%;border-collapse:collapse;margin-top:14px}");
        html.AppendLine("th{background:#0066cc;color:white;padding:12px;text-align:left}td{padding:10px;border-bottom:1px solid #d9e2ec}");
        html.AppendLine("tr:nth-child(even){background:#f8fbff}.totales{margin-top:18px;margin-left:auto;width:310px;background:#f8fbff;border-radius:9px;padding:16px}");
        html.AppendLine(".linea{display:flex;justify-content:space-between;margin:8px 0}.total{font-size:22px;font-weight:bold;color:#0a2342;border-top:2px solid #d9e2ec;padding-top:10px}");
        html.AppendLine(".pie{text-align:center;margin-top:24px;color:#555;font-size:13px}");
        html.AppendLine("</style></head><body><div class='ticket'>");
        html.AppendLine("<div class='encabezado'><h1>Ticket de venta</h1><p>Inventario y Ventas</p></div>");
        html.AppendLine("<div class='datos'>");
        html.AppendLine($"<div class='dato'><strong>Venta No.:</strong> {idVenta}</div>");
        html.AppendLine($"<div class='dato'><strong>Fecha:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</div>");
        html.AppendLine($"<div class='dato'><strong>Usuario:</strong> {System.Net.WebUtility.HtmlEncode(usuario)}</div>");
        html.AppendLine($"<div class='dato'><strong>Productos:</strong> {venta.Detalles.Count}</div>");
        html.AppendLine("</div>");
        html.AppendLine("<table><thead><tr><th>Código</th><th>Producto</th><th>Cantidad</th><th>Precio</th><th>Subtotal</th></tr></thead><tbody>");

        foreach (var d in venta.Detalles)
        {
            html.AppendLine("<tr>");
            html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(d.Codigo)}</td>");
            html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(d.Nombre)}</td>");
            html.AppendLine($"<td>{d.Cantidad}</td>");
            html.AppendLine($"<td>Q{d.PrecioUnitario:0.00}</td>");
            html.AppendLine($"<td>Q{d.Subtotal:0.00}</td>");
            html.AppendLine("</tr>");
        }

        html.AppendLine("</tbody></table>");
        html.AppendLine("<div class='totales'>");
        html.AppendLine($"<div class='linea'><span>Subtotal:</span><strong>Q{venta.CalcularSubtotal():0.00}</strong></div>");
        html.AppendLine($"<div class='linea'><span>Descuento:</span><strong>Q{venta.CalcularDescuento():0.00}</strong></div>");
        html.AppendLine($"<div class='linea total'><span>Total:</span><span>Q{venta.CalcularTotal():0.00}</span></div>");
        html.AppendLine("</div>");
        html.AppendLine("<div class='pie'>Gracias por su compra</div>");
        html.AppendLine("</div></body></html>");

        File.WriteAllText(ruta, html.ToString(), Encoding.UTF8);
        return ruta;
    }
}
