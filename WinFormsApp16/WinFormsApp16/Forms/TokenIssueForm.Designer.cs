using CertDesk.Common;

namespace CertDesk.Forms;

public partial class TokenIssueForm
{
    private System.ComponentModel.IContainer? components;
    private ComboBox employeeComboBox = null!;
    private TextBox actTextBox = null!, commentTextBox = null!;
    private Button issueButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(16) };
        employeeComboBox = UiTheme.ComboBox(); actTextBox = UiTheme.TextBox(); commentTextBox = UiTheme.TextBox(); issueButton = UiTheme.Button("Выдать");
        SuspendLayout(); Text = "Выдача токена"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(390, 320); BackColor = UiTheme.Background;
        AddField(panel, "Сотрудник", employeeComboBox); AddField(panel, "Номер акта", actTextBox); AddField(panel, "Комментарий", commentTextBox); panel.Controls.Add(issueButton); Controls.Add(panel); issueButton.Click += IssueButton_Click; ResumeLayout(false);
    }
    private static void AddField(Control parent, string label, Control editor) { parent.Controls.Add(UiTheme.Label(label)); parent.Controls.Add(editor); }
}
