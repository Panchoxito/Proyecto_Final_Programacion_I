using InventarioVentasCS.Models;
using InventarioVentasCS.Services;

namespace InventarioVentasCS.Forms;

public class FrmProductos : Form
{
    private readonly Usuario usuario;
    private readonly ProductoService service = new();
    private readonly DataGridView dgv = new();
    private readonly TextBox txtBuscar = new();
    private readonly TextBox txtCodigo = new();
    private readonly TextBox txtNombre = new();
    private readonly TextBox txtCategoria = new();
    private readonly NumericUpDown numPrecio = new();
    private readonly NumericUpDown numStock = new();
    private readonly NumericUpDown numMinimo = new();
    private int idSeleccionado = 0;

    public FrmProductos(Usuario usuarioActual)
    {
        usuario = usuarioActual;
        Text = "Productos";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1080, 720);
        MinimumSize = new Size(1080, 720);
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
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 165));

        Panel panelTitulo = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(10, 35, 66),
            Padding = new Padding(28, 0, 28, 0),
            Margin = new Padding(0, 0, 0, 16)
        };
        Label lblTitulo = new()
        {
            Text = "Administración de productos",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill
        };
        panelTitulo.Controls.Add(lblTitulo);

        TableLayoutPanel panelBusqueda = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 4,
            RowCount = 2,
            Padding = new Padding(18, 12, 18, 12),
            Margin = new Padding(0, 0, 0, 18)
        };
        panelBusqueda.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));
        panelBusqueda.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        panelBusqueda.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panelBusqueda.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        panelBusqueda.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        panelBusqueda.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        txtBuscar.Dock = DockStyle.Fill;
        txtBuscar.Margin = new Padding(0, 2, 14, 8);
        Button btnBuscar = CrearBotonTabla("Buscar", Color.FromArgb(0, 102, 204));
        Button btnVolver = CrearBotonTabla("Volver", Color.FromArgb(52, 58, 64));
        btnVolver.Click += (s, e) => Close();
        btnBuscar.Click += (s, e) => CargarProductos();

        panelBusqueda.Controls.Add(CrearEtiquetaTabla("Buscar producto"), 0, 0);
        panelBusqueda.Controls.Add(txtBuscar, 0, 1);
        panelBusqueda.Controls.Add(btnBuscar, 1, 1);
        panelBusqueda.Controls.Add(btnVolver, 3, 1);

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
        dgv.CellClick += Dgv_CellClick;

        Panel panelFormulario = CrearFormularioPanel();

        principal.Controls.Add(panelTitulo, 0, 0);
        principal.Controls.Add(panelBusqueda, 0, 1);
        principal.Controls.Add(dgv, 0, 2);
        principal.Controls.Add(panelFormulario, 0, 3);
        Controls.Add(principal);

        CargarProductos();
        GenerarCodigoNuevo();
        ActivarPermisos();
    }

    private Panel CrearFormularioPanel()
    {
        Panel panel = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(18, 12, 18, 12)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 6,
            RowCount = 4
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        txtCodigo.Dock = DockStyle.Fill;
        txtCodigo.ReadOnly = true;
        txtCodigo.BackColor = Color.FromArgb(233, 236, 239);
        txtNombre.Dock = DockStyle.Fill;
        txtCategoria.Dock = DockStyle.Fill;
        numPrecio.Dock = DockStyle.Fill;
        numStock.Dock = DockStyle.Fill;
        numMinimo.Dock = DockStyle.Fill;

        txtCodigo.Margin = new Padding(0, 2, 14, 8);
        txtNombre.Margin = new Padding(0, 2, 14, 8);
        txtCategoria.Margin = new Padding(0, 2, 14, 8);
        numPrecio.Margin = new Padding(0, 2, 14, 8);
        numStock.Margin = new Padding(0, 2, 14, 8);
        numMinimo.Margin = new Padding(0, 2, 14, 8);

        numPrecio.DecimalPlaces = 2;
        numPrecio.Maximum = 99999;
        numStock.Maximum = 99999;
        numMinimo.Maximum = 99999;
        numMinimo.Value = 5;

        layout.Controls.Add(CrearEtiquetaTabla("Código"), 0, 0);
        layout.Controls.Add(CrearEtiquetaTabla("Nombre"), 1, 0);
        layout.Controls.Add(CrearEtiquetaTabla("Categoría"), 2, 0);
        layout.Controls.Add(CrearEtiquetaTabla("Precio"), 3, 0);
        layout.Controls.Add(CrearEtiquetaTabla("Stock"), 4, 0);

        layout.Controls.Add(txtCodigo, 0, 1);
        layout.Controls.Add(txtNombre, 1, 1);
        layout.Controls.Add(txtCategoria, 2, 1);
        layout.Controls.Add(numPrecio, 3, 1);
        layout.Controls.Add(numStock, 4, 1);

        layout.Controls.Add(CrearEtiquetaTabla("Stock mínimo"), 0, 2);
        layout.Controls.Add(numMinimo, 0, 3);

        TableLayoutPanel botones = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1,
            Margin = new Padding(0)
        };
        botones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
        botones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
        botones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
        botones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));

        Button btnNuevo = CrearBotonTabla("Nuevo", Color.FromArgb(52, 58, 64));
        Button btnGuardar = CrearBotonTabla("Guardar", Color.FromArgb(0, 102, 204));
        Button btnEliminar = CrearBotonTabla("Eliminar", Color.FromArgb(52, 58, 64));
        Button btnMovimiento = CrearBotonTabla("Entrada / ajuste", Color.FromArgb(0, 102, 204));

        btnNuevo.Click += (s, e) => Limpiar();
        btnGuardar.Click += BtnGuardar_Click;
        btnEliminar.Click += BtnEliminar_Click;
        btnMovimiento.Click += BtnMovimiento_Click;

        botones.Controls.Add(btnNuevo, 0, 0);
        botones.Controls.Add(btnGuardar, 1, 0);
        botones.Controls.Add(btnEliminar, 2, 0);
        botones.Controls.Add(btnMovimiento, 3, 0);

        layout.Controls.Add(botones, 1, 3);
        layout.SetColumnSpan(botones, 5);

        panel.Controls.Add(layout);
        return panel;
    }

    private static Label CrearEtiquetaTabla(string texto)
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

    private static Button CrearBotonTabla(string texto, Color color)
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
            MinimumSize = new Size(105, 38)
        };
        boton.FlatAppearance.BorderSize = 0;
        return boton;
    }

    private static Label CrearEtiqueta(string texto, int x, int y)
    {
        return new Label
        {
            Text = texto,
            Location = new Point(x, y),
            AutoSize = true,
            ForeColor = Color.FromArgb(33, 37, 41),
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
    }

    private static Button CrearBoton(string texto, int x, int y, int ancho)
    {
        Button boton = new() { Text = texto, Location = new Point(x, y), Size = new Size(ancho, 38), BackColor = Color.FromArgb(0, 102, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
        boton.FlatAppearance.BorderSize = 0;
        return boton;
    }

    private void ActivarPermisos()
    {
        if (!usuario.PuedeAdministrarProductos())
        {
            DeshabilitarBotonesAdministracion(this);
            MessageBox.Show("El rol Cajero solo puede consultar productos, no puede administrarlos.");
        }
    }

    private static void DeshabilitarBotonesAdministracion(Control contenedor)
    {
        foreach (Control c in contenedor.Controls)
        {
            if (c is Button b && b.Text != "Buscar" && b.Text != "Volver") b.Enabled = false;
            if (c.HasChildren) DeshabilitarBotonesAdministracion(c);
        }
    }

    private void CargarProductos()
    {
        dgv.DataSource = service.Listar(txtBuscar.Text.Trim());

        // El ID se mantiene internamente para seleccionar y editar,
        // pero no se muestra en la tabla para que el usuario vea solo el código.
        if (dgv.Columns.Contains("IdProducto"))
            dgv.Columns["IdProducto"].Visible = false;

        if (dgv.Columns.Contains("Codigo"))
            dgv.Columns["Codigo"].HeaderText = "Código";
        if (dgv.Columns.Contains("StockMinimo"))
            dgv.Columns["StockMinimo"].HeaderText = "Stock mínimo";
    }

    private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow row = dgv.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(row.Cells["IdProducto"].Value);
        txtCodigo.Text = row.Cells["Codigo"].Value.ToString();
        txtNombre.Text = row.Cells["Nombre"].Value.ToString();
        txtCategoria.Text = row.Cells["Categoria"].Value.ToString();
        numPrecio.Value = Convert.ToDecimal(row.Cells["Precio"].Value);
        numStock.Value = Convert.ToDecimal(row.Cells["Stock"].Value);
        numMinimo.Value = Convert.ToDecimal(row.Cells["StockMinimo"].Value);
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (txtCodigo.Text.Trim() == "" || txtNombre.Text.Trim() == "")
        {
            MessageBox.Show("Código y nombre son obligatorios.");
            return;
        }

        Producto p = new()
        {
            IdProducto = idSeleccionado,
            Codigo = txtCodigo.Text.Trim(),
            Nombre = txtNombre.Text.Trim(),
            Categoria = txtCategoria.Text.Trim(),
            Precio = numPrecio.Value,
            Stock = Convert.ToInt32(numStock.Value),
            StockMinimo = Convert.ToInt32(numMinimo.Value)
        };

        try
        {
            service.Guardar(p);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Producto duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        MessageBox.Show("Producto guardado correctamente.");
        Limpiar();
        CargarProductos();
    }

    private void BtnEliminar_Click(object? sender, EventArgs e)
    {
        if (idSeleccionado == 0) return;
        service.Eliminar(idSeleccionado);
        MessageBox.Show("Producto eliminado.");
        Limpiar();
        CargarProductos();
    }

    private void BtnMovimiento_Click(object? sender, EventArgs e)
    {
        if (idSeleccionado == 0) { MessageBox.Show("Seleccione un producto."); return; }
        service.RegistrarMovimiento(idSeleccionado, "Entrada", Convert.ToInt32(numStock.Value), "Entrada o ajuste registrado desde formulario de productos");
        MessageBox.Show("Movimiento registrado en bitácora de inventario.");
    }

    private void Limpiar()
    {
        idSeleccionado = 0;
        txtNombre.Clear(); txtCategoria.Clear();
        numPrecio.Value = 0; numStock.Value = 0; numMinimo.Value = 5;
        GenerarCodigoNuevo();
    }

    private void GenerarCodigoNuevo()
    {
        try
        {
            txtCodigo.Text = service.ObtenerSiguienteCodigo();
        }
        catch
        {
            txtCodigo.Text = "P0001";
        }
    }
}
