using CertDesk.Common;

namespace CertDesk.Forms;

public partial class MchdEditForm
{
    private System.ComponentModel.IContainer? components;
    private TextBox numberTextBox = null!, powersTextBox = null!, codesTextBox = null!, notesTextBox = null!;
    private ComboBox principalComboBox = null!, representativeComboBox = null!, certificateComboBox = null!;
    private DateTimePicker validFromPicker = null!, validToPicker = null!;
    private CheckBox registeredCheckBox = null!;
    private Button saveButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(16), AutoScroll = true };
        numberTextBox = UiTheme.TextBox(); powersTextBox = UiTheme.TextBox(); powersTextBox.Multiline = true; powersTextBox.Height = 70; codesTextBox = UiTheme.TextBox(); notesTextBox = UiTheme.TextBox(); principalComboBox = UiTheme.ComboBox(); representativeComboBox = UiTheme.ComboBox(); certificateComboBox = UiTheme.ComboBox(); validFromPicker = Picker(); validToPicker = Picker(); registeredCheckBox = new CheckBox { Text = "Зарегистрирована", Width = 260, Margin = new Padding(4, 8, 4, 4) }; saveButton = UiTheme.Button("Сохранить");
        SuspendLayout(); Text = "Машиночитаемая доверенность"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(460, 720); BackColor = UiTheme.Background;
        AddField(panel, "Номер МЧД", numberTextBox); AddField(panel, "Доверитель", principalComboBox); AddField(panel, "Представитель", representativeComboBox); AddField(panel, "Сертификат", certificateComboBox); AddField(panel, "Полномочия", powersTextBox); AddField(panel, "Коды полномочий", codesTextBox); AddField(panel, "Действует с", validFromPicker); AddField(panel, "Действует до", validToPicker); panel.Controls.Add(registeredCheckBox); AddField(panel, "Примечание", notesTextBox);
        panel.Controls.Add(saveButton); Controls.Add(panel); saveButton.Click += SaveButton_Click; ResumeLayout(false);
    }
    private static DateTimePicker Picker() => new() { Width = 260, Format = DateTimePickerFormat.Short, Margin = new Padding(4) };
    private static void AddField(Control parent, string label, Control editor) { parent.Controls.Add(UiTheme.Label(label)); parent.Controls.Add(editor); }
}
