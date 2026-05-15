using CertDesk.Common;

namespace CertDesk.Forms;

public partial class CertificateEditForm
{
    private System.ComponentModel.IContainer? components;
    private ComboBox employeeComboBox = null!, authorityComboBox = null!, tokenComboBox = null!, signatureComboBox = null!;
    private TextBox serialTextBox = null!, purposeTextBox = null!;
    private DateTimePicker issuedDatePicker = null!, validFromPicker = null!, validToPicker = null!;
    private Button saveButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(16), AutoScroll = true };
        employeeComboBox = UiTheme.ComboBox(); authorityComboBox = UiTheme.ComboBox(); tokenComboBox = UiTheme.ComboBox(); signatureComboBox = UiTheme.ComboBox(); serialTextBox = UiTheme.TextBox(); purposeTextBox = UiTheme.TextBox(); issuedDatePicker = Picker(); validFromPicker = Picker(); validToPicker = Picker(); saveButton = UiTheme.Button("Сохранить");
        SuspendLayout(); Text = "Сертификат"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(430, 620); BackColor = UiTheme.Background;
        AddField(panel, "Владелец", employeeComboBox); AddField(panel, "Удостоверяющий центр", authorityComboBox); AddField(panel, "Токен", tokenComboBox); AddField(panel, "Серийный номер", serialTextBox); AddField(panel, "Тип подписи", signatureComboBox); AddField(panel, "Дата выдачи", issuedDatePicker); AddField(panel, "Действует с", validFromPicker); AddField(panel, "Действует до", validToPicker); AddField(panel, "Назначение", purposeTextBox);
        panel.Controls.Add(saveButton); Controls.Add(panel); saveButton.Click += SaveButton_Click; ResumeLayout(false);
    }
    private static DateTimePicker Picker() => new() { Width = 260, Format = DateTimePickerFormat.Short, Margin = new Padding(4) };
    private static void AddField(Control parent, string label, Control editor) { parent.Controls.Add(UiTheme.Label(label)); parent.Controls.Add(editor); }
}
