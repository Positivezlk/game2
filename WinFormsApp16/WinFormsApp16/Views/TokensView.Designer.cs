using CertDesk.Common;

namespace CertDesk.Views;

public partial class TokensView
{
    private System.ComponentModel.IContainer? components;
    private DataGridView grid = null!; private TextBox searchTextBox = null!; private ComboBox statusComboBox = null!; private Button addButton = null!, editButton = null!, issueButton = null!, returnButton = null!, damageButton = null!, writeOffButton = null!, refreshButton = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container(); var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 88 };
        grid = UiTheme.Grid(); searchTextBox = UiTheme.TextBox(); statusComboBox = UiTheme.ComboBox(); addButton = UiTheme.Button("Добавить"); editButton = UiTheme.Button("Изменить"); issueButton = UiTheme.Button("Выдать"); returnButton = UiTheme.Button("Вернуть"); damageButton = UiTheme.Button("Поврежден"); writeOffButton = UiTheme.Button("Списать"); refreshButton = UiTheme.Button("Обновить");
        SuspendLayout(); BackColor = UiTheme.White; statusComboBox.Items.AddRange(["Все", "storage", "issued", "damaged", "written_off"]); statusComboBox.SelectedIndex = 0;
        top.Controls.AddRange([UiTheme.Label("Поиск"), searchTextBox, UiTheme.Label("Статус"), statusComboBox, addButton, editButton, issueButton, returnButton, damageButton, writeOffButton, refreshButton]); Controls.Add(grid); Controls.Add(top);
        searchTextBox.TextChanged += FilterChanged; statusComboBox.SelectedIndexChanged += FilterChanged; addButton.Click += AddButton_Click; editButton.Click += EditButton_Click; issueButton.Click += IssueButton_Click; returnButton.Click += ReturnButton_Click; damageButton.Click += DamageButton_Click; writeOffButton.Click += WriteOffButton_Click; refreshButton.Click += RefreshButton_Click; ResumeLayout(false);
    }
}
