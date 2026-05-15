using System.Data; using System.Text;
namespace CertDesk.Reports;
public static class CsvExporter
{
 public static void Save(DataTable table,string path){using var w=new StreamWriter(path,false,new UTF8Encoding(true)); w.WriteLine(string.Join(';',table.Columns.Cast<DataColumn>().Select(c=>Esc(c.ColumnName)))); foreach(DataRow r in table.Rows) w.WriteLine(string.Join(';',table.Columns.Cast<DataColumn>().Select(c=>Esc(Convert.ToString(r[c])??""))));}
 private static string Esc(string s)=>$"\"{s.Replace("\"","\"\"")}\"";
}
