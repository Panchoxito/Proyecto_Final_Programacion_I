using InventarioVentasCS.Forms;

namespace InventarioVentasCS;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmLogin());
    }
}
