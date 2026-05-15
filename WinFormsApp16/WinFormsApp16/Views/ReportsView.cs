using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Views;

public partial class ReportsView : UserControl
{
    private readonly ReportService service;
    public ReportsView(CurrentUser user)
    {
        service = new ReportService(user);
        InitializeComponent();
    }
    private void CsvButton_Click(object? sender, EventArgs e) => Export(false);
    private void XlsxButton_Click(object? sender, EventArgs e) => Export(true);
    private void OpenFolderButton_Click(object? sender, EventArgs e) => service.OpenFolder();
    private void Export(bool xlsx)
    {
        try
        {
            var path = service.Export(reportTypeComboBox.Text, xlsx, fromDatePicker.Value, toDatePicker.Value);
            MessageHelper.Info("Отчет сформирован:\n" + path);
        }
        catch (Exception ex) { MessageHelper.Error(ex); }
    }
}
