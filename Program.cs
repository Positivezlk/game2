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
        while (true)
        {
            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK || login.CurrentUser == null) break;
            Application.Run(new MainForm(login.CurrentUser));
            if (!MainForm.RestartLogin) break;
        }
    }
}
