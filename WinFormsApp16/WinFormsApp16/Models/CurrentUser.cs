namespace CertDesk.Models;
public sealed class CurrentUser { public int Id { get; set; } public string Login { get; set; } = ""; public string Role { get; set; } = "viewer"; public int? EmployeeId { get; set; } public bool IsActive { get; set; } }
