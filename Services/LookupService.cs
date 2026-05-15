using CertDesk.Data;
using System.Data;
namespace CertDesk.Services;
public static class LookupService
{
    public static DataTable Employees() => Query("SELECT id, full_name AS name FROM employees WHERE is_active=1 ORDER BY full_name");
    public static DataTable Authorities() => Query("SELECT id, name FROM authorities WHERE is_active=1 ORDER BY name");
    public static DataTable Tokens() => Query("SELECT id, inventory_number || ' — ' || COALESCE(model,'') AS name FROM tokens ORDER BY inventory_number");
    public static DataTable Certificates() => Query("SELECT id, serial_number AS name FROM certificates WHERE is_archived=0 ORDER BY serial_number");
    private static DataTable Query(string sql) { using var c = Db.OpenConnection(); using var cmd = Db.Command(c, sql); var dt = new DataTable(); dt.Load(cmd.ExecuteReader()); return dt; }
}
