using CertDesk.Common;

namespace CertDesk.Views;

public partial class EmployeesView
{
    private System.ComponentModel.IContainer? components;
    private DataGridView grid = null!;
    private TextBox searchTextBox = null!;
    private ComboBox stateComboBox = null!;
    private Button addButton = null!, editButton = null!, archiveButton = null!, refreshButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48 };
        grid = UiTheme.Grid(); searchTextBox = UiTheme.TextBox(); stateComboBox = UiTheme.ComboBox(); addButton = UiTheme.Button("Добавить"); editButton = UiTheme.Button("Изменить"); archiveButton = UiTheme.Button("Архивировать"); refreshButton = UiTheme.Button("Обновить");
        SuspendLayout(); BackColor = UiTheme.White; stateComboBox.Items.AddRange(["Все", "Активные", "Архивные"]); stateComboBox.SelectedIndex = 1;
        top.Controls.AddRange([UiTheme.Label("Поиск"), searchTextBox, stateComboBox, addButton, editButton, archiveButton, refreshButton]); Controls.Add(grid); Controls.Add(top);
        searchTextBox.TextChanged += SearchChanged; stateComboBox.SelectedIndexChanged += SearchChanged; refreshButton.Click += RefreshButton_Click; addButton.Click += AddButton_Click; editButton.Click += EditButton_Click; archiveButton.Click += ArchiveButton_Click; ResumeLayout(false);
    }
}
