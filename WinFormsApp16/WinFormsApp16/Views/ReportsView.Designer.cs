using CertDesk.Common;

namespace CertDesk.Views;

public partial class ReportsView
{
    private System.ComponentModel.IContainer? components;
    private ComboBox reportTypeComboBox = null!;
    private DateTimePicker fromDatePicker = null!, toDatePicker = null!;
    private Button csvButton = null!, xlsxButton = null!, openFolderButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 110 };
        reportTypeComboBox = UiTheme.ComboBox(330); fromDatePicker = new DateTimePicker { Width = 140, Format = DateTimePickerFormat.Short, Margin = new Padding(4) }; toDatePicker = new DateTimePicker { Width = 140, Format = DateTimePickerFormat.Short, Margin = new Padding(4) }; csvButton = UiTheme.Button("Сформировать CSV", 150); xlsxButton = UiTheme.Button("Сформировать XLSX", 160); openFolderButton = UiTheme.Button("Открыть папку");
        SuspendLayout(); BackColor = UiTheme.White; reportTypeComboBox.Items.AddRange(["Реестр сотрудников", "Реестр сертификатов", "Сертификаты, истекающие в течение 30 дней", "Реестр МЧД", "МЧД, истекающие в течение 30 дней", "Реестр токенов", "Журнал операций с токенами", "Сводка по подразделениям"]); reportTypeComboBox.SelectedIndex = 1; fromDatePicker.Value = DateTime.Today.AddMonths(-1); toDatePicker.Value = DateTime.Today;
        top.Controls.AddRange([UiTheme.Label("Тип отчета"), reportTypeComboBox, UiTheme.Label("Период с"), fromDatePicker, UiTheme.Label("по"), toDatePicker, csvButton, xlsxButton, openFolderButton]); Controls.Add(top);
        var info = UiTheme.Label("Отчеты сохраняются в папку ReportsOutput. CSV формируется в UTF-8 BOM с разделителем ;, XLSX — через ClosedXML.", 12); info.Dock = DockStyle.Top; Controls.Add(info);
        csvButton.Click += CsvButton_Click; xlsxButton.Click += XlsxButton_Click; openFolderButton.Click += OpenFolderButton_Click; ResumeLayout(false);
    }
}
