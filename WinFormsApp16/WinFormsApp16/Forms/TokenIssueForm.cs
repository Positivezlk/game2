using CertDesk.Common;
using CertDesk.Services;

namespace CertDesk.Forms;

public partial class TokenIssueForm : Form
{
    private readonly int tokenId;
    private readonly TokenService service;

    public TokenIssueForm(int tokenId, TokenService service)
    {
        this.tokenId = tokenId;
        this.service = service;
        InitializeComponent();
        employeeComboBox.DataSource = LookupService.Employees();
        employeeComboBox.ValueMember = "id";
        employeeComboBox.DisplayMember = "name";
    }

    private void IssueButton_Click(object? sender, EventArgs e)
    {
        try
        {
            service.Issue(tokenId, Convert.ToInt32(employeeComboBox.SelectedValue), actTextBox.Text, commentTextBox.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { MessageHelper.Error(ex); }
    }
}
