namespace CertDesk.Common;

public static class MessageHelper
{
    public static void Info(string text) => MessageBox.Show(text, "CertDesk", MessageBoxButtons.OK, MessageBoxIcon.Information);
    public static void Warning(string text) => MessageBox.Show(text, "CertDesk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    public static void Error(Exception ex) => MessageBox.Show(ex.Message, "CertDesk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
