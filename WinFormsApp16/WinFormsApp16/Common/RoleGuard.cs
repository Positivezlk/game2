using CertDesk.Models;

namespace CertDesk.Common;

public static class RoleGuard
{
    public static bool CanEdit(CurrentUser user) => user.Role is "administrator" or "specialist";
    public static bool CanOpenAudit(CurrentUser user) => user.Role == "administrator";
    public static bool CanExport(CurrentUser user) => user.Role is "administrator" or "specialist";
    public static bool CanBackup(CurrentUser user) => user.Role == "administrator";
    public static bool EnsureEdit(CurrentUser user)
    {
        if (CanEdit(user)) return true;
        MessageHelper.Warning("Недостаточно прав для выполнения операции.");
        return false;
    }
}
