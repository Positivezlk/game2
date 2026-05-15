using ClosedXML.Excel; using System.Data;
namespace CertDesk.Reports;
public static class XlsxExporter
{
 public static void Save(DataTable table,string title,string path){using var wb=new XLWorkbook(); var ws=wb.Worksheets.Add("Отчет"); ws.Cell(1,1).Value=title; ws.Cell(2,1).Value="Дата формирования: "+DateTime.Now.ToString("dd.MM.yyyy HH:mm"); for(int c=0;c<table.Columns.Count;c++) ws.Cell(4,c+1).Value=table.Columns[c].ColumnName; for(int r=0;r<table.Rows.Count;r++) for(int c=0;c<table.Columns.Count;c++) ws.Cell(r+5,c+1).Value=Convert.ToString(table.Rows[r][c]); ws.Range(4,1,Math.Max(4,table.Rows.Count+4),table.Columns.Count).Style.Border.OutsideBorder=XLBorderStyleValues.Thin; ws.Columns().AdjustToContents(); wb.SaveAs(path);}
}
