using System.Text;
using InventarioVentasCS.Services;

namespace InventarioVentasCS.Forms;

public class FrmReportes : Form
{
    private readonly ReporteService service = new();
    private readonly DataGridView dgv = new();
    private readonly DateTimePicker dtpFecha = new();
    private string tituloReporte = "Reporte";

    public FrmReportes()
    {
        Text = "Reportes";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1040, 680);
        MinimumSize = new Size(1040, 680);
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        TableLayoutPanel principal = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(24, 24, 24, 24),
            BackColor = Color.FromArgb(245, 247, 250)
        };
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Panel panelTitulo = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(10, 35, 66),
            Padding = new Padding(28, 0, 28, 0),
            Margin = new Padding(0, 0, 0, 16)
        };
        Label lblTitulo = new()
        {
            Text = "Reportes",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill
        };
        panelTitulo.Controls.Add(lblTitulo);

        TableLayoutPanel panelFiltros = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 6,
            RowCount = 2,
            Padding = new Padding(18, 12, 18, 12),
            Margin = new Padding(0, 0, 0, 18)
        };
        panelFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        panelFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        panelFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        panelFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        panelFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panelFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        panelFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        panelFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Label lblFecha = CrearEtiqueta("Fecha del reporte");
        dtpFecha.Dock = DockStyle.Fill;
        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.Margin = new Padding(0, 2, 14, 8);

        Button btnVentasDia = CrearBoton("Ventas del día", Color.FromArgb(0, 102, 204));
        Button btnMasVendidos = CrearBoton("Más vendidos", Color.FromArgb(0, 102, 204));
        Button btnExportar = CrearBoton("Descargar", Color.FromArgb(0, 102, 204));
        Button btnVolver = CrearBoton("Volver", Color.FromArgb(52, 58, 64));

        btnVolver.Click += (s, e) => Close();
        btnVentasDia.Click += (s, e) =>
        {
            tituloReporte = $"Ventas del día {dtpFecha.Value:dd/MM/yyyy}";
            dgv.DataSource = service.VentasPorDia(dtpFecha.Value);
        };
        btnMasVendidos.Click += (s, e) =>
        {
            tituloReporte = "Productos más vendidos";
            dgv.DataSource = service.ProductosMasVendidos();
        };
        btnExportar.Click += BtnExportar_Click;

        panelFiltros.Controls.Add(lblFecha, 0, 0);
        panelFiltros.Controls.Add(dtpFecha, 0, 1);
        panelFiltros.Controls.Add(btnVentasDia, 1, 1);
        panelFiltros.Controls.Add(btnMasVendidos, 2, 1);
        panelFiltros.Controls.Add(btnExportar, 3, 1);
        panelFiltros.Controls.Add(btnVolver, 5, 1);

        dgv.Dock = DockStyle.Fill;
        dgv.Margin = new Padding(0);
        dgv.ReadOnly = true;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.None;
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(10, 35, 66);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        dgv.ColumnHeadersHeight = 36;
        dgv.RowHeadersVisible = false;

        principal.Controls.Add(panelTitulo, 0, 0);
        principal.Controls.Add(panelFiltros, 0, 1);
        principal.Controls.Add(dgv, 0, 2);
        Controls.Add(principal);
    }

    private static Label CrearEtiqueta(string texto)
    {
        return new Label
        {
            Text = texto,
            Dock = DockStyle.Fill,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.FromArgb(33, 37, 41),
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
    }

    private static Button CrearBoton(string texto, Color color)
    {
        Button boton = new()
        {
            Text = texto,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 2, 14, 8),
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            MinimumSize = new Size(110, 38)
        };
        boton.FlatAppearance.BorderSize = 0;
        return boton;
    }

    private void BtnExportar_Click(object? sender, EventArgs e)
    {
        if (dgv.Rows.Count == 0) { MessageBox.Show("No hay datos para exportar."); return; }

        using SaveFileDialog guardar = new()
        {
            Title = "Guardar reporte",
            Filter = "Archivo HTML (*.html)|*.html|Archivo CSV (*.csv)|*.csv",
            FileName = $"{tituloReporte.Replace(" ", "_").Replace("/", "-")}_{DateTime.Now:yyyyMMdd_HHmmss}.html",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        };

        if (guardar.ShowDialog() != DialogResult.OK) return;

        if (Path.GetExtension(guardar.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            GuardarCsv(guardar.FileName);
        else
            GuardarHtml(guardar.FileName);

        MessageBox.Show("Reporte descargado correctamente en:\n" + guardar.FileName, "Reporte", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void GuardarCsv(string ruta)
    {
        using StreamWriter sw = new(ruta, false, Encoding.UTF8);
        sw.WriteLine(string.Join(",", dgv.Columns.Cast<DataGridViewColumn>().Select(c => EscaparCsv(c.HeaderText))));
        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (row.IsNewRow) continue;
            sw.WriteLine(string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(c => EscaparCsv(c.Value?.ToString() ?? ""))));
        }
    }

    private void GuardarHtml(string ruta)
    {
        StringBuilder html = new();
        html.AppendLine("<!DOCTYPE html><html lang='es'><head><meta charset='UTF-8'>");
        html.AppendLine("<title>Reporte</title>");
        html.AppendLine("<style>body{font-family:Segoe UI,Arial;background:#f5f7fa;margin:35px;color:#212529}.card{background:white;border-radius:10px;padding:25px;box-shadow:0 6px 20px rgba(0,0,0,.12)}h1{background:#0a2342;color:white;padding:18px;border-radius:8px;text-align:center}table{width:100%;border-collapse:collapse;margin-top:20px}th{background:#0066cc;color:white;padding:12px;text-align:left}td{padding:10px;border-bottom:1px solid #d9e2ec}tr:nth-child(even){background:#f0f5fb}.fecha{text-align:right;color:#555}</style>");
        html.AppendLine("</head><body><div class='card'>");
        html.AppendLine($"<h1>{System.Net.WebUtility.HtmlEncode(tituloReporte)}</h1>");
        html.AppendLine($"<p class='fecha'>Generado: {DateTime.Now:dd/MM/yyyy HH:mm}</p>");
        html.AppendLine("<table><thead><tr>");
        foreach (DataGridViewColumn col in dgv.Columns)
            html.AppendLine($"<th>{System.Net.WebUtility.HtmlEncode(col.HeaderText)}</th>");
        html.AppendLine("</tr></thead><tbody>");
        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (row.IsNewRow) continue;
            html.AppendLine("<tr>");
            foreach (DataGridViewCell cell in row.Cells)
                html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(cell.Value?.ToString() ?? "")}</td>");
            html.AppendLine("</tr>");
        }
        html.AppendLine("</tbody></table></div></body></html>");
        File.WriteAllText(ruta, html.ToString(), Encoding.UTF8);
    }

    private static string EscaparCsv(string valor)
    {
        return "\"" + valor.Replace("\"", "\"\"") + "\"";
    }
}
