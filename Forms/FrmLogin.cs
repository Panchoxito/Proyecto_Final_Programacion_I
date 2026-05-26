using InventarioVentasCS.Models;
using InventarioVentasCS.Services;

namespace InventarioVentasCS.Forms;

public class FrmLogin : Form
{
    private readonly TextBox txtUsuario = new();
    private readonly TextBox txtClave = new();
    private readonly Button btnIngresar = new();

    public FrmLogin()
    {
        Text = "Inicio de sesión";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(520, 430);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10);

        TableLayoutPanel principal = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.FromArgb(245, 247, 250)
        };
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        Panel panelTitulo = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(10, 35, 66),
            Padding = new Padding(28, 0, 28, 0)
        };
        Label lblTitulo = new()
        {
            Text = "Sistema de Inventario y Ventas",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 17, FontStyle.Bold),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        panelTitulo.Controls.Add(lblTitulo);

        TableLayoutPanel centro = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            Padding = new Padding(0, 26, 0, 26)
        };
        centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        centro.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380));
        centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        centro.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        centro.RowStyles.Add(new RowStyle(SizeType.Absolute, 235));
        centro.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        TableLayoutPanel card = new()
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            ColumnCount = 1,
            BackColor = Color.White,
            Padding = new Padding(28, 22, 28, 22)
        };
        card.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        card.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        card.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        card.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        card.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        txtUsuario.Dock = DockStyle.Fill;
        txtUsuario.Margin = new Padding(0, 2, 0, 12);
        txtClave.Dock = DockStyle.Fill;
        txtClave.Margin = new Padding(0, 2, 0, 12);
        txtClave.PasswordChar = '*';

        btnIngresar.Text = "Ingresar";
        btnIngresar.Dock = DockStyle.Bottom;
        btnIngresar.Height = 44;
        btnIngresar.BackColor = Color.FromArgb(0, 102, 204);
        btnIngresar.ForeColor = Color.White;
        btnIngresar.FlatStyle = FlatStyle.Flat;
        btnIngresar.FlatAppearance.BorderSize = 0;
        btnIngresar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btnIngresar.Cursor = Cursors.Hand;
        btnIngresar.Click += BtnIngresar_Click;

        card.Controls.Add(CrearEtiqueta("Usuario"), 0, 0);
        card.Controls.Add(txtUsuario, 0, 1);
        card.Controls.Add(CrearEtiqueta("Contraseña"), 0, 2);
        card.Controls.Add(txtClave, 0, 3);
        card.Controls.Add(btnIngresar, 0, 4);
        centro.Controls.Add(card, 1, 1);

        principal.Controls.Add(panelTitulo, 0, 0);
        principal.Controls.Add(centro, 0, 1);
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
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };
    }

    private void BtnIngresar_Click(object? sender, EventArgs e)
    {
        try
        {
            Usuario? usuario = new AuthService().Login(txtUsuario.Text.Trim(), txtClave.Text.Trim());
            if (usuario == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Hide();
            new FrmMenu(usuario).ShowDialog();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
        }
    }
}
