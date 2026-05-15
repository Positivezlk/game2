using CertDesk.Common;

namespace CertDesk.Forms;

public partial class TokenEditForm
{
    private System.ComponentModel.IContainer? components;
    private TextBox inventoryTextBox = null!, typeTextBox = null!, modelTextBox = null!, serialTextBox = null!, notesTextBox = null!;
    private ComboBox statusComboBox = null!, holderComboBox = null!;
    private DateTimePicker receivedDatePicker = null!;
    private Button saveButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(16), AutoScroll = true };
        inventoryTextBox = UiTheme.TextBox(); typeTextBox = UiTheme.TextBox(); modelTextBox = UiTheme.TextBox(); serialTextBox = UiTheme.TextBox(); notesTextBox = UiTheme.TextBox(); statusComboBox = UiTheme.ComboBox(); holderComboBox = UiTheme.ComboBox(); receivedDatePicker = new DateTimePicker { Width = 260, Format = DateTimePickerFormat.Short, Margin = new Padding(4) }; saveButton = UiTheme.Button("Сохранить");
        SuspendLayout(); Text = "Токен"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(420, 620); BackColor = UiTheme.Background;
        AddField(panel, "Инвентарный номер", inventoryTextBox); AddField(panel, "Тип токена", typeTextBox); AddField(panel, "Модель", modelTextBox); AddField(panel, "Серийный номер", serialTextBox); AddField(panel, "Дата поступления", receivedDatePicker); AddField(panel, "Статус", statusComboBox); AddField(panel, "Текущий держатель", holderComboBox); AddField(panel, "Примечание", notesTextBox);
        panel.Controls.Add(saveButton); Controls.Add(panel); saveButton.Click += SaveButton_Click; ResumeLayout(false);
    }
    private static void AddField(Control parent, string label, Control editor) { parent.Controls.Add(UiTheme.Label(label)); parent.Controls.Add(editor); }
}
