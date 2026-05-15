using CertDesk.Data;
using CertDesk.Models;
using System.Data;

namespace CertDesk.Services;
public sealed class AuditService(CurrentUser? user)
{
    public void Log(string action, string? entityType = null, int? entityId = null, string? description = null)
    {
        using var c = Db.OpenConnection();
        using var cmd = Db.Command(c, "INSERT INTO audit_log(user_id,login,action,entity_type,entity_id,description) VALUES(@u,@l,@a,@e,@id,@d)", ("@u", user?.Id), ("@l", user?.Login), ("@a", action), ("@e", entityType), ("@id", entityId), ("@d", description));
        cmd.ExecuteNonQuery();
    }
    public DataTable Search(DateTime from, DateTime to, string userText, string action, string entity)
    {
        using var c = Db.OpenConnection(); using var cmd = Db.Command(c, @"SELECT created_at AS 'Дата и время', COALESCE(login,'') AS 'Пользователь', action AS 'Действие', COALESCE(entity_type,'') AS 'Объект', COALESCE(entity_id,'') AS 'ID объекта', COALESCE(description,'') AS 'Описание' FROM audit_log WHERE date(created_at) BETWEEN date(@f) AND date(@t) AND login LIKE @u AND action LIKE @a AND COALESCE(entity_type,'') LIKE @e ORDER BY created_at DESC", ("@f", from.ToString("yyyy-MM-dd")), ("@t", to.ToString("yyyy-MM-dd")), ("@u", $"%{userText}%"), ("@a", $"%{action}%"), ("@e", $"%{entity}%"));
        var dt = new DataTable(); dt.Load(cmd.ExecuteReader()); return dt;
    }
}
