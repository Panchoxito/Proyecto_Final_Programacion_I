using InventarioVentasCS.Models;
using InventarioVentasCS.Services;

namespace InventarioVentasCS.Forms;

public class FrmVentas : Form
{
    private readonly Usuario usuario;
    private readonly ProductoService productoService = new();
    private readonly VentaService ventaService = new();
    private readonly TicketService ticketService = new();
    private readonly List<DetalleVenta> carrito = new();

    private readonly TextBox txtCodigo = new();
    private readonly NumericUpDown numCantidad = new();
    private readonly DataGridView dgv = new();
    private readonly Label lblSubtotal = new();
    private readonly Label lblDescuento = new();
    private readonly Label lblTotal = new();

    public FrmVentas(Usuario usuarioActual)
    {
        usuario = usuarioActual;
        Text = "Registro de ventas";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1040, 700);
        MinimumSize = new Size(1040, 700);
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        TableLayoutPanel principal = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(24, 24, 24, 24),
            BackColor = Color.FromArgb(245, 247, 250)
        };
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 98));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));

        Panel panelTitulo = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(10, 35, 66),
            Padding = new Padding(28, 0, 28, 0),
            Margin = new Padding(0, 0, 0, 16)
        };
        Label lblTitulo = new()
        {
            Text = "Registro de ventas",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill
        };
        panelTitulo.Controls.Add(lblTitulo);

        TableLayoutPanel panelEntrada = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 5,
            RowCount = 2,
            Padding = new Padding(18, 12, 18, 12),
            Margin = new Padding(0, 0, 0, 18)
        };
        panelEntrada.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 330));
        panelEntrada.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        panelEntrada.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        panelEntrada.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panelEntrada.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        panelEntrada.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        panelEntrada.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Button btnAgregar = CrearBoton("Agregar", Color.FromArgb(0, 102, 204));
        Button btnVolver = CrearBoton("Volver", Color.FromArgb(52, 58, 64));
        btnVolver.Click += (s, e) => Close();
        btnAgregar.Click += BtnAgregar_Click;

        txtCodigo.Dock = DockStyle.Fill;
        txtCodigo.Margin = new Padding(0, 2, 14, 8);
        numCantidad.Dock = DockStyle.Fill;
        numCantidad.Minimum = 1;
        numCantidad.Maximum = 999;
        numCantidad.Value = 1;
        numCantidad.Margin = new Padding(0, 2, 14, 8);

        panelEntrada.Controls.Add(CrearEtiqueta("Código o nombre del producto"), 0, 0);
        panelEntrada.Controls.Add(CrearEtiqueta("Cantidad"), 1, 0);
        panelEntrada.Controls.Add(txtCodigo, 0, 1);
        panelEntrada.Controls.Add(numCantidad, 1, 1);
        panelEntrada.Controls.Add(btnAgregar, 2, 1);
        panelEntrada.Controls.Add(btnVolver, 4, 1);

        dgv.Dock = DockStyle.Fill;
        dgv.Margin = new Padding(0, 0, 0, 18);
        dgv.ReadOnly = true;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.None;
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(10, 35, 66);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        dgv.ColumnHeadersHeight = 36;
        dgv.RowHeadersVisible = false;

        TableLayoutPanel panelInferior = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            BackColor = Color.FromArgb(245, 247, 250)
        };
        panelInferior.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 470));
        panelInferior.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panelInferior.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320));

        TableLayoutPanel panelBotones = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            BackColor = Color.Transparent
        };
        panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
        panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280));
        panelBotones.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        panelBotones.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        Button btnQuitar = CrearBoton("Quitar", Color.FromArgb(52, 58, 64));
        Button btnLimpiar = CrearBoton("Limpiar", Color.FromArgb(52, 58, 64));
        Button btnVender = CrearBoton("Registrar venta y ticket", Color.FromArgb(0, 102, 204));
        btnQuitar.Click += BtnQuitar_Click;
        btnVender.Click += BtnVender_Click;
        btnLimpiar.Click += (s, e) => { carrito.Clear(); RefrescarCarrito(); };

        panelBotones.Controls.Add(btnQuitar, 0, 0);
        panelBotones.Controls.Add(btnLimpiar, 0, 1);
        panelBotones.Controls.Add(btnVender, 1, 0);
        panelBotones.SetRowSpan(btnVender, 2);

        Panel panelTotales = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(24, 18, 24, 18)
        };
        lblSubtotal.Dock = DockStyle.Top;
        lblSubtotal.Height = 28;
        lblDescuento.Dock = DockStyle.Top;
        lblDescuento.Height = 28;
        lblTotal.Dock = DockStyle.Top;
        lblTotal.Height = 38;
        lblTotal.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTotal.ForeColor = Color.FromArgb(10, 35, 66);
        panelTotales.Controls.AddRange(new Control[] { lblTotal, lblDescuento, lblSubtotal });

        panelInferior.Controls.Add(panelBotones, 0, 0);
        panelInferior.Controls.Add(panelTotales, 2, 0);

        principal.Controls.Add(panelTitulo, 0, 0);
        principal.Controls.Add(panelEntrada, 0, 1);
        principal.Controls.Add(dgv, 0, 2);
        principal.Controls.Add(panelInferior, 0, 3);
        Controls.Add(principal);
        RefrescarCarrito();
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
            Margin = new Padding(0, 4, 14, 8),
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Cursor = Cursors.Hand,
            MinimumSize = new Size(120, 38)
        };
        boton.FlatAppearance.BorderSize = 0;
        return boton;
    }

    private void BtnAgregar_Click(object? sender, EventArgs e)
    {
        Producto? p = productoService.BuscarPorCodigoONombre(txtCodigo.Text.Trim());
        if (p == null) { MessageBox.Show("Producto no encontrado. Puede buscarlo por código o por nombre."); return; }

        int cantidad = Convert.ToInt32(numCantidad.Value);
        if (p.Stock < cantidad)
        {
            MessageBox.Show($"No hay suficiente stock. Existencia actual: {p.Stock}");
            return;
        }

        if (p.TieneStockBajo())
            MessageBox.Show($"Alerta: el producto {p.Nombre} tiene existencias bajas.");

        DetalleVenta? existente = carrito.FirstOrDefault(x => x.IdProducto == p.IdProducto);
        if (existente != null)
            existente.Cantidad += cantidad;
        else
            carrito.Add(new DetalleVenta { IdProducto = p.IdProducto, Codigo = p.Codigo, Nombre = p.Nombre, Cantidad = cantidad, PrecioUnitario = p.Precio });

        txtCodigo.Clear();
        numCantidad.Value = 1;
        RefrescarCarrito();
    }

    private void BtnQuitar_Click(object? sender, EventArgs e)
    {
        if (dgv.CurrentRow == null) return;
        string codigo = dgv.CurrentRow.Cells["Codigo"].Value.ToString() ?? "";
        DetalleVenta? item = carrito.FirstOrDefault(x => x.Codigo == codigo);
        if (item != null) carrito.Remove(item);
        RefrescarCarrito();
    }

    private void BtnVender_Click(object? sender, EventArgs e)
    {
        if (carrito.Count == 0) { MessageBox.Show("Debe agregar productos a la venta."); return; }

        Venta venta = new() { IdUsuario = usuario.IdUsuario, Detalles = carrito.ToList() };
        try
        {
            int idVenta = ventaService.RegistrarVenta(venta);

            using SaveFileDialog guardar = new()
            {
                Title = "Guardar ticket",
                Filter = "Archivo HTML (*.html)|*.html",
                FileName = $"Ticket_Venta_{idVenta}.html",
                AddExtension = true,
                DefaultExt = "html"
            };

            if (guardar.ShowDialog() == DialogResult.OK)
            {
                string ruta = ticketService.GuardarTicket(idVenta, venta, usuario.NombreUsuario, guardar.FileName);
                MessageBox.Show($"Venta registrada correctamente. Ticket guardado en:\n{ruta}");
            }
            else
            {
                MessageBox.Show("Venta registrada correctamente. No se guardó el ticket porque se canceló la selección de ubicación.");
            }

            carrito.Clear();
            RefrescarCarrito();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al registrar venta: " + ex.Message);
        }
    }

    private void RefrescarCarrito()
    {
        dgv.DataSource = null;
        dgv.DataSource = carrito.Select(x => new { x.Codigo, x.Nombre, x.Cantidad, x.PrecioUnitario, x.Subtotal }).ToList();

        Venta ventaTemp = new() { Detalles = carrito };
        lblSubtotal.Text = $"Subtotal: Q{ventaTemp.CalcularSubtotal():0.00}";
        lblDescuento.Text = $"Descuento: Q{ventaTemp.CalcularDescuento():0.00}";
        lblTotal.Text = $"Total: Q{ventaTemp.CalcularTotal():0.00}";
    }
}
