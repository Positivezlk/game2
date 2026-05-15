using CertDesk.Common;

namespace CertDesk.Forms;

public partial class EmployeeEditForm
{
    private System.ComponentModel.IContainer? components;
    private TextBox fullNameTextBox = null!, positionTextBox = null!, departmentTextBox = null!, emailTextBox = null!, phoneTextBox = null!, snilsTextBox = null!, innTextBox = null!;
    private CheckBox activeCheckBox = null!;
    private Button saveButton = null!;

    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(16), AutoScroll = true };
        fullNameTextBox = UiTheme.TextBox(); positionTextBox = UiTheme.TextBox(); departmentTextBox = UiTheme.TextBox(); emailTextBox = UiTheme.TextBox(); phoneTextBox = UiTheme.TextBox(); snilsTextBox = UiTheme.TextBox(); innTextBox = UiTheme.TextBox();
        activeCheckBox = new CheckBox { Text = "Активен", Width = 260, Margin = new Padding(4, 8, 4, 4) };
        saveButton = UiTheme.Button("Сохранить");
        SuspendLayout();
        Text = "Сотрудник"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(390, 520); BackColor = UiTheme.Background;
        AddField(panel, "ФИО", fullNameTextBox); AddField(panel, "Должность", positionTextBox); AddField(panel, "Подразделение", departmentTextBox); AddField(panel, "Email", emailTextBox); AddField(panel, "Телефон", phoneTextBox); AddField(panel, "СНИЛС", snilsTextBox); AddField(panel, "ИНН", innTextBox);
        panel.Controls.Add(activeCheckBox); panel.Controls.Add(saveButton); Controls.Add(panel);
        saveButton.Click += SaveButton_Click;
        ResumeLayout(false);
    }

    private static void AddField(Control parent, string label, Control editor) { parent.Controls.Add(UiTheme.Label(label)); parent.Controls.Add(editor); }
}
