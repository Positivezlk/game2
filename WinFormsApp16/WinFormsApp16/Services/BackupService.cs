using CertDesk.Data; using CertDesk.Models;
namespace CertDesk.Services;
public sealed class BackupService(CurrentUser user)
{
 public string CreateBackup(){var dir=Path.Combine(AppContext.BaseDirectory,"Backups"); Directory.CreateDirectory(dir); var path=Path.Combine(dir,$"certdesk_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db"); File.Copy(Db.DatabasePath,path,true); new AuditService(user).Log("создание резервной копии","backup",null,Path.GetFileName(path)); return path;}
}
