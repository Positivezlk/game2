using CertDesk.Common;

namespace CertDesk.Views;

public partial class MchdView
{
    private System.ComponentModel.IContainer? components;
    private DataGridView grid = null!; private TextBox searchTextBox = null!; private ComboBox statusComboBox = null!, registrationComboBox = null!; private Button addButton = null!, editButton = null!, revokeButton = null!, archiveButton = null!, refreshButton = null!, exportButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 88 };
        grid = UiTheme.Grid(); searchTextBox = UiTheme.TextBox(); statusComboBox = UiTheme.ComboBox(); registrationComboBox = UiTheme.ComboBox(); addButton = UiTheme.Button("Добавить"); editButton = UiTheme.Button("Изменить"); revokeButton = UiTheme.Button("Отозвать"); archiveButton = UiTheme.Button("Архивировать"); refreshButton = UiTheme.Button("Обновить"); exportButton = UiTheme.Button("Экспорт");
        SuspendLayout(); BackColor = UiTheme.White; statusComboBox.Items.AddRange(["Все", "active", "warning", "expired", "revoked", "archived"]); statusComboBox.SelectedIndex = 0; registrationComboBox.Items.AddRange(["Все", "Зарегистрирована", "Не зарегистрирована"]); registrationComboBox.SelectedIndex = 0;
        top.Controls.AddRange([UiTheme.Label("Поиск"), searchTextBox, UiTheme.Label("Статус"), statusComboBox, UiTheme.Label("Регистрация"), registrationComboBox, addButton, editButton, revokeButton, archiveButton, refreshButton, exportButton]); Controls.Add(grid); Controls.Add(top);
        searchTextBox.TextChanged += FilterChanged; statusComboBox.SelectedIndexChanged += FilterChanged; registrationComboBox.SelectedIndexChanged += FilterChanged; addButton.Click += AddButton_Click; editButton.Click += EditButton_Click; revokeButton.Click += RevokeButton_Click; archiveButton.Click += ArchiveButton_Click; refreshButton.Click += RefreshButton_Click; exportButton.Click += ExportButton_Click; ResumeLayout(false);
    }
}
