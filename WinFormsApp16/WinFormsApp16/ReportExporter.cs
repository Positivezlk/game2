using ClosedXML.Excel;
using System.Data;
using System.Text;

namespace WinFormsApp16;

public static class ReportExporter
{
    private static string ReportsDirectory
    {
        get
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            Directory.CreateDirectory(path);
            return path;
        }
    }

    public static string ExportCsv(string reportName, DataTable table)
    {
        var path = Path.Combine(ReportsDirectory, $"{SafeFileName(reportName)}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        using var writer = new StreamWriter(path, false, new UTF8Encoding(true));
        writer.WriteLine(string.Join(';', table.Columns.Cast<DataColumn>().Select(c => Escape(c.ColumnName))));
        foreach (DataRow row in table.Rows)
            writer.WriteLine(string.Join(';', table.Columns.Cast<DataColumn>().Select(c => Escape(Convert.ToString(row[c]) ?? string.Empty))));
        return path;
    }

    public static string ExportXlsx(string reportName, DataTable table)
    {
        var path = Path.Combine(ReportsDirectory, $"{SafeFileName(reportName)}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Отчет");
        worksheet.Cell(1, 1).Value = reportName;
        worksheet.Cell(2, 1).Value = "Дата формирования: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        for (int column = 0; column < table.Columns.Count; column++)
            worksheet.Cell(4, column + 1).Value = table.Columns[column].ColumnName;
        for (int row = 0; row < table.Rows.Count; row++)
            for (int column = 0; column < table.Columns.Count; column++)
                worksheet.Cell(row + 5, column + 1).Value = Convert.ToString(table.Rows[row][column]);
        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(path);
        return path;
    }

    public static DataTable BuildReport(string reportName) => reportName switch
    {
        "Сотрудники" => Database.Query("SELECT id AS 'ID', full_name AS 'ФИО', position AS 'Должность', department AS 'Подразделение', email AS 'Email', phone AS 'Телефон', CASE is_active WHEN 1 THEN 'активен' ELSE 'архивный' END AS 'Статус' FROM employees ORDER BY full_name"),
        "Сертификаты" => Database.Query("SELECT c.id AS 'ID', c.serial_number AS 'Серийный номер', e.full_name AS 'Владелец', c.signature_type AS 'Тип подписи', c.authority AS 'УЦ', c.valid_from AS 'Действует с', c.valid_to AS 'Действует до', c.status AS 'Статус', c.purpose AS 'Назначение' FROM certificates c LEFT JOIN employees e ON e.id=c.employee_id ORDER BY c.valid_to"),
        "МЧД" => Database.Query("SELECT m.id AS 'ID', m.number AS 'Номер МЧД', m.principal AS 'Доверитель', e.full_name AS 'Представитель', m.powers AS 'Полномочия', m.valid_from AS 'Действует с', m.valid_to AS 'Действует до', m.status AS 'Статус' FROM mchd m LEFT JOIN employees e ON e.id=m.representative_id ORDER BY m.valid_to"),
        "Токены" => Database.Query("SELECT t.id AS 'ID', t.inventory_number AS 'Инвентарный номер', t.token_type AS 'Тип', t.model AS 'Модель', t.serial_number AS 'Серийный номер', t.status AS 'Статус', e.full_name AS 'Держатель' FROM tokens t LEFT JOIN employees e ON e.id=t.holder_id ORDER BY t.inventory_number"),
        "Операции токенов" => Database.Query("SELECT o.id AS 'ID', o.created_at AS 'Дата', t.inventory_number AS 'Токен', e.full_name AS 'Сотрудник', o.operation AS 'Операция', o.act_number AS 'Акт', o.comment AS 'Комментарий' FROM token_operations o LEFT JOIN tokens t ON t.id=o.token_id LEFT JOIN employees e ON e.id=o.employee_id ORDER BY o.created_at DESC"),
        _ => new DataTable()
    };

    private static string Escape(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
    private static string SafeFileName(string value) => string.Concat(value.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '_' : ch)).Replace(' ', '_');
}
