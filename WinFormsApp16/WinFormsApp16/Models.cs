namespace WinFormsApp16;

public static class AppRoles
{
    public const string Admin = "Администратор";
    public const string Specialist = "Специалист";
    public const string Viewer = "Просмотр";

    public static bool CanEdit(string role) => role is Admin or Specialist;
    public static bool CanClearAudit(string role) => role == Admin;
}

public static class StatusText
{
    public static string CertificateStatus(DateTime validTo, string currentStatus)
    {
        if (currentStatus is "revoked" or "archived") return currentStatus;
        var daysLeft = (validTo.Date - DateTime.Today).Days;
        if (daysLeft < 0) return "expired";
        return daysLeft <= 30 ? "warning" : "active";
    }

    public static string ToRussian(string status) => status switch
    {
        "active" => "действует",
        "warning" => "истекает",
        "expired" => "истек",
        "revoked" => "отозван",
        "archived" => "архивный",
        "storage" => "на хранении",
        "issued" => "выдан",
        "damaged" => "поврежден",
        "written_off" => "списан",
        _ => status
    };
}
