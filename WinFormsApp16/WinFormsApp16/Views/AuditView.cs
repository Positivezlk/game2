using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Views;

public partial class AuditView : UserControl
{
    private readonly AuditService service;
    private readonly CurrentUser user;
    public AuditView(CurrentUser user)
    {
        this.user = user;
        service = new AuditService(user);
        InitializeComponent();
        if (!RoleGuard.CanOpenAudit(user))
        {
            MessageHelper.Warning("Недостаточно прав для выполнения операции.");
            Enabled = false;
            return;
        }
        LoadData();
    }
    private void RefreshButton_Click(object? sender, EventArgs e) => LoadData();
    private void LoadData() => grid.DataSource = service.Search(fromDatePicker.Value, toDatePicker.Value, userTextBox.Text, actionTextBox.Text, entityTextBox.Text);
}
