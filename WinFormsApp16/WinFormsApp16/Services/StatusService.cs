namespace CertDesk.Services;
public static class StatusService
{
    public static string Calculate(DateTime validTo, string current = "") => current is "revoked" or "archived" ? current : validTo.Date < DateTime.Today ? "expired" : (validTo.Date - DateTime.Today).TotalDays <= 30 ? "warning" : "active";
    public static string Text(string status) => status switch { "active" => "действует", "warning" => "истекает", "expired" => "истек", "revoked" => "отозван", "archived" => "архивный", "storage" => "на хранении", "issued" => "выдан", "damaged" => "поврежден", "written_off" => "списан", _ => status };
}
