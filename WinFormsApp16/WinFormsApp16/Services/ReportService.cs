using CertDesk.Data; using CertDesk.Models; using CertDesk.Reports; using System.Data; using System.Diagnostics;
namespace CertDesk.Services;
public sealed class ReportService(CurrentUser user)
{
 public string ReportsDir { get; } = Path.Combine(AppContext.BaseDirectory,"ReportsOutput");
 public DataTable Build(string type,DateTime from,DateTime to){var sql=type switch{
  "Реестр сотрудников"=>"SELECT full_name AS 'ФИО',position AS 'Должность',department AS 'Подразделение',email AS 'Email',phone AS 'Телефон' FROM employees",
  "Сертификаты, истекающие в течение 30 дней"=>"SELECT serial_number AS 'Серийный номер',valid_to AS 'Действует до',status AS 'Статус' FROM certificates WHERE status='warning'",
  "Реестр МЧД"=>"SELECT number AS 'Номер МЧД',powers AS 'Полномочия',valid_to AS 'Действует до',status AS 'Статус' FROM mchd",
  "МЧД, истекающие в течение 30 дней"=>"SELECT number AS 'Номер МЧД',valid_to AS 'Действует до',status AS 'Статус' FROM mchd WHERE status='warning'",
  "Реестр токенов"=>"SELECT inventory_number AS 'Инвентарный номер',token_type AS 'Тип',model AS 'Модель',status AS 'Статус' FROM tokens",
  "Журнал операций с токенами"=>"SELECT created_at AS 'Дата',token_id AS 'Токен',operation AS 'Операция',act_number AS 'Акт',comment AS 'Комментарий' FROM token_operations",
  "Сводка по подразделениям"=>"SELECT department AS 'Подразделение',COUNT(*) AS 'Сотрудников' FROM employees GROUP BY department",
  _=>"SELECT c.serial_number AS 'Серийный номер',e.full_name AS 'Владелец',c.signature_type AS 'Тип',c.valid_to AS 'Действует до',c.status AS 'Статус' FROM certificates c JOIN employees e ON e.id=c.employee_id"}; using var c=Db.OpenConnection(); using var cmd=Db.Command(c,sql); var dt=new DataTable(type); dt.Load(cmd.ExecuteReader()); return dt;}
 public string Export(string type,bool xlsx,DateTime from,DateTime to){Directory.CreateDirectory(ReportsDir); var table=Build(type,from,to); var path=Path.Combine(ReportsDir,$"{Safe(type)}_{DateTime.Now:yyyyMMdd_HHmmss}."+(xlsx?"xlsx":"csv")); if(xlsx) XlsxExporter.Save(table,type,path); else CsvExporter.Save(table,path); new AuditService(user).Log("экспорт отчета","report",null,type); return path;}
 public void OpenFolder(){Directory.CreateDirectory(ReportsDir); Process.Start(new ProcessStartInfo{FileName=ReportsDir,UseShellExecute=true});}
 private static string Safe(string s)=>string.Concat(s.Select(ch=>Path.GetInvalidFileNameChars().Contains(ch)?'_':ch)).Replace(' ','_');
}
