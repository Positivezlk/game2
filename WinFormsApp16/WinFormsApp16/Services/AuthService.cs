using CertDesk.Data;
using CertDesk.Models;

namespace CertDesk.Services;
public sealed class AuthService
{
    public CurrentUser? Login(string login, string password)
    {
        using var c = Db.OpenConnection();
        using var cmd = Db.Command(c, "SELECT id,login,password_hash,role,employee_id,is_active FROM users WHERE login=@l AND is_active=1", ("@l", login));
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, r.GetString(2))) return null;
        var user = new CurrentUser { Id = r.GetInt32(0), Login = r.GetString(1), Role = r.GetString(3), EmployeeId = r.IsDBNull(4) ? null : r.GetInt32(4), IsActive = r.GetInt32(5) == 1 };
        new AuditService(user).Log("вход", "user", user.Id, "Успешный вход в систему");
        return user;
    }
    public static string RoleTitle(string role) => role switch { "administrator" => "Администратор", "specialist" => "Специалист", _ => "Просмотр" };
}
