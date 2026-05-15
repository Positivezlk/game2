using CertDesk.Data;
using CertDesk.Forms;

namespace CertDesk;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        DbInitializer.EnsureCreated();
        Application.Run(new LoginForm());
    }
}
