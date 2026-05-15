using CertDesk.Common;

namespace CertDesk.Views;

public partial class AuditView
{
    private System.ComponentModel.IContainer? components;
    private DataGridView grid = null!;
    private DateTimePicker fromDatePicker = null!, toDatePicker = null!;
    private TextBox userTextBox = null!, actionTextBox = null!, entityTextBox = null!;
    private Button refreshButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 86 };
        grid = UiTheme.Grid(); fromDatePicker = new DateTimePicker { Width = 130, Format = DateTimePickerFormat.Short, Margin = new Padding(4) }; toDatePicker = new DateTimePicker { Width = 130, Format = DateTimePickerFormat.Short, Margin = new Padding(4) }; userTextBox = UiTheme.TextBox(130); actionTextBox = UiTheme.TextBox(150); entityTextBox = UiTheme.TextBox(120); refreshButton = UiTheme.Button("Обновить");
        SuspendLayout(); BackColor = UiTheme.White; fromDatePicker.Value = DateTime.Today.AddMonths(-2); toDatePicker.Value = DateTime.Today;
        top.Controls.AddRange([UiTheme.Label("Дата с"), fromDatePicker, UiTheme.Label("по"), toDatePicker, UiTheme.Label("Пользователь"), userTextBox, UiTheme.Label("Действие"), actionTextBox, UiTheme.Label("Объект"), entityTextBox, refreshButton]); Controls.Add(grid); Controls.Add(top);
        refreshButton.Click += RefreshButton_Click; ResumeLayout(false);
    }
}
