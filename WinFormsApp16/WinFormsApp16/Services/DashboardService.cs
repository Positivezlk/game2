using CertDesk.Data;
using System.Data;

namespace CertDesk.Services;

public sealed class DashboardService
{
    public IReadOnlyList<(string Title, string Value, string Status)> GetMetrics()
    {
        using var connection = Db.OpenConnection();
        int Count(string sql) { using var command = Db.Command(connection, sql); return Convert.ToInt32(command.ExecuteScalar()); }
        return [
            ("действующие сертификаты", Count("SELECT COUNT(*) FROM certificates WHERE status='active'").ToString(), "active"),
            ("сертификаты истекают", Count("SELECT COUNT(*) FROM certificates WHERE status='warning'").ToString(), "warning"),
            ("действующие МЧД", Count("SELECT COUNT(*) FROM mchd WHERE status='active'").ToString(), "active"),
            ("МЧД истекают", Count("SELECT COUNT(*) FROM mchd WHERE status='warning'").ToString(), "warning"),
            ("всего токенов", Count("SELECT COUNT(*) FROM tokens").ToString(), "info"),
            ("выданные токены", Count("SELECT COUNT(*) FROM tokens WHERE status='issued'").ToString(), "issued"),
            ("активные сотрудники", Count("SELECT COUNT(*) FROM employees WHERE is_active=1").ToString(), "info")
        ];
    }

    public DataTable GetAttentionTable()
    {
        using var connection = Db.OpenConnection();
        using var command = Db.Command(connection, @"SELECT 'Сертификат' AS 'Тип записи', c.serial_number AS 'Номер', e.full_name AS 'Сотрудник', c.valid_to AS 'Действует до', CAST(julianday(c.valid_to)-julianday('now','start of day') AS INT) AS 'Осталось дней', c.status AS 'Статус' FROM certificates c JOIN employees e ON e.id=c.employee_id WHERE c.status IN ('warning','expired') UNION ALL SELECT 'МЧД', m.number, e.full_name, m.valid_to, CAST(julianday(m.valid_to)-julianday('now','start of day') AS INT), m.status FROM mchd m JOIN employees e ON e.id=m.representative_employee_id WHERE m.status IN ('warning','expired') ORDER BY 'Действует до'");
        var table = new DataTable();
        table.Load(command.ExecuteReader());
        return table;
    }
}
