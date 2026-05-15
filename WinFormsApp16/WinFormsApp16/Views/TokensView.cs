using CertDesk.Common;
using CertDesk.Forms;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Views;

public partial class TokensView : UserControl
{
    private readonly CurrentUser user;
    private readonly TokenService service;
    public TokensView(CurrentUser user) { this.user = user; service = new TokenService(user); InitializeComponent(); ConfigurePermissions(); LoadData(); }
    private int SelectedId => grid.CurrentRow == null ? 0 : Convert.ToInt32(grid.CurrentRow.Cells["id"].Value);
    private void ConfigurePermissions() { addButton.Visible = editButton.Visible = issueButton.Visible = returnButton.Visible = damageButton.Visible = writeOffButton.Visible = RoleGuard.CanEdit(user); }
    private bool EnsureSelection() => SelectedId > 0;
    private void FilterChanged(object? sender, EventArgs e) => LoadData();
    private void AddButton_Click(object? sender, EventArgs e) => OpenEditor(0);
    private void EditButton_Click(object? sender, EventArgs e) => OpenEditor(SelectedId);
    private void IssueButton_Click(object? sender, EventArgs e) { if (!RoleGuard.EnsureEdit(user) || !EnsureSelection()) return; using var form = new TokenIssueForm(SelectedId, service); if (form.ShowDialog(this) == DialogResult.OK) LoadData(); }
    private void ReturnButton_Click(object? sender, EventArgs e) => ExecuteOperation(() => service.Return(SelectedId, "Возврат пользователем"));
    private void DamageButton_Click(object? sender, EventArgs e) => ExecuteOperation(() => service.Damage(SelectedId, "Отмечен как поврежден"));
    private void WriteOffButton_Click(object? sender, EventArgs e) => ExecuteOperation(() => service.WriteOff(SelectedId, "Списание токена"));
    private void RefreshButton_Click(object? sender, EventArgs e) => LoadData();
    private void OpenEditor(int id) { if (!RoleGuard.EnsureEdit(user)) return; using var form = new TokenEditForm(service.Get(id) ?? new Token { ReceivedAt = DateTime.Today }, service); if (form.ShowDialog(this) == DialogResult.OK) LoadData(); }
    private void ExecuteOperation(Action action) { if (!RoleGuard.EnsureEdit(user) || !EnsureSelection()) return; try { action(); LoadData(); } catch (Exception ex) { MessageHelper.Error(ex); } }
    private void LoadData() { grid.DataSource = service.Search(searchTextBox.Text, statusComboBox.Text); if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false; }
}
