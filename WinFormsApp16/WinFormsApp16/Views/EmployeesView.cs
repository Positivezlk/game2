using CertDesk.Common;
using CertDesk.Forms;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Views;

public partial class EmployeesView : UserControl
{
    private readonly CurrentUser user;
    private readonly EmployeeService service;

    public EmployeesView(CurrentUser user)
    {
        this.user = user;
        service = new EmployeeService(user);
        InitializeComponent();
        ConfigurePermissions();
        LoadData();
    }

    private int SelectedId => grid.CurrentRow == null ? 0 : Convert.ToInt32(grid.CurrentRow.Cells["id"].Value);
    private void ConfigurePermissions() { addButton.Visible = editButton.Visible = archiveButton.Visible = RoleGuard.CanEdit(user); }
    private void SearchChanged(object? sender, EventArgs e) => LoadData();
    private void RefreshButton_Click(object? sender, EventArgs e) => LoadData();
    private void AddButton_Click(object? sender, EventArgs e) => OpenEditor(0);
    private void EditButton_Click(object? sender, EventArgs e) => OpenEditor(SelectedId);
    private void ArchiveButton_Click(object? sender, EventArgs e)
    {
        if (!RoleGuard.EnsureEdit(user) || SelectedId == 0) return;
        service.Archive(SelectedId);
        LoadData();
    }

    private void OpenEditor(int id)
    {
        if (!RoleGuard.EnsureEdit(user)) return;
        using var form = new EmployeeEditForm(service.Get(id) ?? new Employee(), service);
        if (form.ShowDialog(this) == DialogResult.OK) LoadData();
    }

    private void LoadData()
    {
        grid.DataSource = service.Search(searchTextBox.Text, stateComboBox.Text);
        if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false;
    }
}
