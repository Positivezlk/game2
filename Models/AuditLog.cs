namespace CertDesk.Models;
public sealed class AuditLog { public int Id { get; set; } public int? UserId { get; set; } public string? Login { get; set; } public string Action { get; set; } = ""; public string? EntityType { get; set; } public int? EntityId { get; set; } public string? Description { get; set; } public DateTime CreatedAt { get; set; } }
