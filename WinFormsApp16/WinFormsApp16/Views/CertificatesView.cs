using CertDesk.Common;
using CertDesk.Forms;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Views;

public partial class CertificatesView : UserControl
{
    private readonly CurrentUser user;
    private readonly CertificateService service;
    public CertificatesView(CurrentUser user)
    {
        this.user = user; service = new CertificateService(user); InitializeComponent(); ConfigurePermissions(); LoadData();
    }
    private int SelectedId => grid.CurrentRow == null ? 0 : Convert.ToInt32(grid.CurrentRow.Cells["id"].Value);
    private void ConfigurePermissions() { addButton.Visible = editButton.Visible = revokeButton.Visible = archiveButton.Visible = RoleGuard.CanEdit(user); exportButton.Visible = RoleGuard.CanExport(user); }
    private void FilterChanged(object? sender, EventArgs e) => LoadData();
    private void AddButton_Click(object? sender, EventArgs e) => OpenEditor(0);
    private void EditButton_Click(object? sender, EventArgs e) => OpenEditor(SelectedId);
    private void RevokeButton_Click(object? sender, EventArgs e) => SetStatus("revoked", "отзыв сертификата");
    private void ArchiveButton_Click(object? sender, EventArgs e) => SetStatus("archived", "архивирование сертификата");
    private void RefreshButton_Click(object? sender, EventArgs e) => LoadData();
    private void ExportButton_Click(object? sender, EventArgs e) => MessageHelper.Info("Используйте раздел Отчеты для CSV/XLSX экспорта.");
    private void OpenEditor(int id) { if (!RoleGuard.EnsureEdit(user)) return; using var form = new CertificateEditForm(service.Get(id) ?? new Certificate { IssuedAt = DateTime.Today, ValidFrom = DateTime.Today, ValidTo = DateTime.Today.AddYears(1) }, service); if (form.ShowDialog(this) == DialogResult.OK) LoadData(); }
    private void SetStatus(string status, string auditAction) { if (!RoleGuard.EnsureEdit(user) || SelectedId == 0) return; if (MessageBox.Show("Подтвердите операцию", "CertDesk", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) { service.SetStatus(SelectedId, status, auditAction); LoadData(); } }
    private void LoadData() { grid.DataSource = service.Search(searchTextBox.Text, statusComboBox.Text, typeComboBox.Text); if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false; }
}
