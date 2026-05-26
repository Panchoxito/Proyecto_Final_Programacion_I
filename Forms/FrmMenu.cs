using InventarioVentasCS.Models;

namespace InventarioVentasCS.Forms;

public class FrmMenu : Form
{
    private readonly Usuario usuarioActual;

    public FrmMenu(Usuario usuario)
    {
        usuarioActual = usuario;
        Text = "Menú principal";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(760, 500);
        MinimumSize = new Size(760, 500);
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        Panel panelTitulo = new()
        {
            Dock = DockStyle.Top,
            Height = 145,
            BackColor = Color.FromArgb(10, 35, 66),
            Padding = new Padding(24, 18, 24, 18)
        };

        TableLayoutPanel tituloLayout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        tituloLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
        tituloLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 38));

        Label lblTitulo = new()
        {
            Text = "Inventario y Ventas",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.BottomCenter,
            Dock = DockStyle.Fill
        };

        Label lblBienvenida = new()
        {
            Text = usuario.MostrarPermisos(),
            ForeColor = Color.FromArgb(202, 240, 248),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.TopCenter,
            Dock = DockStyle.Fill
        };

        tituloLayout.Controls.Add(lblTitulo, 0, 0);
        tituloLayout.Controls.Add(lblBienvenida, 0, 1);
        panelTitulo.Controls.Add(tituloLayout);

        TableLayoutPanel contenedor = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            Padding = new Padding(34, 35, 34, 38),
            BackColor = Color.Transparent
        };
        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34));
        contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        contenedor.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        contenedor.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        contenedor.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        Button btnProductos = CrearBoton("Productos");
        Button btnVentas = CrearBoton("Ventas");
        Button btnReportes = CrearBoton("Reportes");
        Button btnSalir = CrearBoton("Salir");

        btnProductos.Click += (s, e) => new FrmProductos(usuarioActual).ShowDialog();
        btnVentas.Click += (s, e) => new FrmVentas(usuarioActual).ShowDialog();
        btnReportes.Click += (s, e) => new FrmReportes().ShowDialog();
        btnSalir.Click += (s, e) => Close();

        contenedor.Controls.Add(btnProductos, 0, 0);
        contenedor.Controls.Add(btnVentas, 2, 0);
        contenedor.Controls.Add(btnReportes, 0, 2);
        contenedor.Controls.Add(btnSalir, 2, 2);

        Controls.Add(contenedor);
        Controls.Add(panelTitulo);
    }

    private static Button CrearBoton(string texto)
    {
        Button boton = new()
        {
            Text = texto,
            Dock = DockStyle.Fill,
            Margin = new Padding(28, 14, 28, 14),
            BackColor = Color.FromArgb(0, 102, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Cursor = Cursors.Hand,
            MinimumSize = new Size(210, 64)
        };
        boton.FlatAppearance.BorderSize = 0;
        return boton;
    }
}
