using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp16;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;
    private Label titleLabel = null!;
    private TabControl tabControl = null!;
    private TabPage tabDashboard = null!;
    private TabPage tabEmployees = null!;
    private TabPage tabCertificates = null!;
    private TabPage tabMchd = null!;
    private TabPage tabTokens = null!;
    private TabPage tabReports = null!;
    private TabPage tabAudit = null!;
    private ComboBox roleComboBox = null!;
    private Label roleLabel = null!;
    private Label lblEmployeesCount = null!;
    private Label lblCertificatesCount = null!;
    private Label lblCertificateWarnings = null!;
    private Label lblMchdCount = null!;
    private Label lblTokensCount = null!;
    private Label lblIssuedTokens = null!;
    private DataGridView gridAttention = null!;
    private TextBox txtEmployeeSearch = null!;
    private Button btnEmployeeAdd = null!;
    private Button btnEmployeeEdit = null!;
    private Button btnEmployeeArchive = null!;
    private Button btnEmployeeRefresh = null!;
    private DataGridView gridEmployees = null!;
    private TextBox txtCertificateSearch = null!;
    private ComboBox cmbCertificateStatus = null!;
    private Button btnCertificateAdd = null!;
    private Button btnCertificateEdit = null!;
    private Button btnCertificateRevoke = null!;
    private Button btnCertificateArchive = null!;
    private Button btnCertificateRefresh = null!;
    private DataGridView gridCertificates = null!;
    private TextBox txtMchdSearch = null!;
    private ComboBox cmbMchdStatus = null!;
    private Button btnMchdAdd = null!;
    private Button btnMchdEdit = null!;
    private Button btnMchdRevoke = null!;
    private Button btnMchdArchive = null!;
    private Button btnMchdRefresh = null!;
    private DataGridView gridMchd = null!;
    private TextBox txtTokenSearch = null!;
    private ComboBox cmbTokenStatus = null!;
    private Button btnTokenAdd = null!;
    private Button btnTokenEdit = null!;
    private Button btnTokenIssue = null!;
    private Button btnTokenReturn = null!;
    private Button btnTokenWriteOff = null!;
    private Button btnTokenRefresh = null!;
    private DataGridView gridTokens = null!;
    private ComboBox cmbReportType = null!;
    private Button btnExportCsv = null!;
    private Button btnExportXlsx = null!;
    private Label lblReportPath = null!;
    private Button btnAuditRefresh = null!;
    private Button btnAuditClear = null!;
    private DataGridView gridAudit = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        titleLabel = new Label();
        tabControl = new TabControl();
        tabDashboard = new TabPage();
        tabEmployees = new TabPage();
        tabCertificates = new TabPage();
        tabMchd = new TabPage();
        tabTokens = new TabPage();
        tabReports = new TabPage();
        tabAudit = new TabPage();
        roleComboBox = new ComboBox(); roleLabel = new Label();
        lblEmployeesCount = MetricLabel(); lblCertificatesCount = MetricLabel(); lblCertificateWarnings = MetricLabel(); lblMchdCount = MetricLabel(); lblTokensCount = MetricLabel(); lblIssuedTokens = MetricLabel(); gridAttention = Grid();
        txtEmployeeSearch = new TextBox(); btnEmployeeAdd = Button("Добавить"); btnEmployeeEdit = Button("Изменить"); btnEmployeeArchive = Button("Архивировать"); btnEmployeeRefresh = Button("Обновить"); gridEmployees = Grid();
        txtCertificateSearch = new TextBox(); cmbCertificateStatus = Combo(); btnCertificateAdd = Button("Добавить"); btnCertificateEdit = Button("Изменить"); btnCertificateRevoke = Button("Отозвать"); btnCertificateArchive = Button("Архивировать"); btnCertificateRefresh = Button("Обновить"); gridCertificates = Grid();
        txtMchdSearch = new TextBox(); cmbMchdStatus = Combo(); btnMchdAdd = Button("Добавить"); btnMchdEdit = Button("Изменить"); btnMchdRevoke = Button("Отозвать"); btnMchdArchive = Button("Архивировать"); btnMchdRefresh = Button("Обновить"); gridMchd = Grid();
        txtTokenSearch = new TextBox(); cmbTokenStatus = Combo(); btnTokenAdd = Button("Добавить"); btnTokenEdit = Button("Изменить"); btnTokenIssue = Button("Выдать"); btnTokenReturn = Button("Вернуть"); btnTokenWriteOff = Button("Списать"); btnTokenRefresh = Button("Обновить"); gridTokens = Grid();
        cmbReportType = Combo(); btnExportCsv = Button("Экспорт CSV"); btnExportXlsx = Button("Экспорт XLSX"); lblReportPath = new Label(); btnAuditRefresh = Button("Обновить"); btnAuditClear = Button("Очистить аудит"); gridAudit = Grid();
        SuspendLayout();
        Text = "CertDesk — учет сертификатов ЭП и МЧД"; StartPosition = FormStartPosition.CenterScreen; ClientSize = new Size(1200, 750); BackColor = Color.FromArgb(242, 244, 247);
        titleLabel.Text = "CertDesk — учет сертификатов ЭП и МЧД"; titleLabel.Dock = DockStyle.Top; titleLabel.Height = 54; titleLabel.TextAlign = ContentAlignment.MiddleLeft; titleLabel.Padding = new Padding(18, 0, 0, 0); titleLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold); titleLabel.ForeColor = Color.White; titleLabel.BackColor = Color.FromArgb(47, 95, 159);
        tabControl.Dock = DockStyle.Fill; tabControl.Font = new Font("Segoe UI", 10);
        tabDashboard.Text = "Главная"; tabEmployees.Text = "Сотрудники"; tabCertificates.Text = "Сертификаты"; tabMchd.Text = "МЧД"; tabTokens.Text = "Токены"; tabReports.Text = "Отчеты"; tabAudit.Text = "Аудит";
        tabControl.TabPages.AddRange(new[] { tabDashboard, tabEmployees, tabCertificates, tabMchd, tabTokens, tabReports, tabAudit });
        BuildDashboardTab(); BuildEmployeesTab(); BuildCertificatesTab(); BuildMchdTab(); BuildTokensTab(); BuildReportsTab(); BuildAuditTab();
        Controls.Add(tabControl); Controls.Add(titleLabel);
        ResumeLayout(false);
    }

    private void BuildDashboardTab()
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), RowCount = 4, ColumnCount = 1 };
        var rolePanel = new FlowLayoutPanel { Dock = DockStyle.Fill, Height = 42 };
        roleLabel.Text = "Роль пользователя:"; roleLabel.AutoSize = true; roleLabel.Margin = new Padding(4, 8, 8, 4);
        roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList; roleComboBox.Width = 180; roleComboBox.Items.AddRange(new object[] { "Администратор", "Специалист", "Просмотр" }); roleComboBox.SelectedIndex = 0;
        rolePanel.Controls.Add(roleLabel); rolePanel.Controls.Add(roleComboBox);
        var metrics = new FlowLayoutPanel { Dock = DockStyle.Fill, Height = 110 };
        metrics.Controls.AddRange(new Control[] { lblEmployeesCount, lblCertificatesCount, lblCertificateWarnings, lblMchdCount, lblTokensCount, lblIssuedTokens });
        var attentionLabel = new Label { Text = "Требуют внимания", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12, FontStyle.Bold), Height = 32 };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 46)); panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 115)); panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36)); panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.Controls.Add(rolePanel); panel.Controls.Add(metrics); panel.Controls.Add(attentionLabel); panel.Controls.Add(gridAttention); tabDashboard.Controls.Add(panel);
    }

    private void BuildEmployeesTab() => BuildRegistryTab(tabEmployees, txtEmployeeSearch, new[] { btnEmployeeAdd, btnEmployeeEdit, btnEmployeeArchive, btnEmployeeRefresh }, gridEmployees);
    private void BuildCertificatesTab() => BuildRegistryTab(tabCertificates, txtCertificateSearch, new[] { cmbCertificateStatus, btnCertificateAdd, btnCertificateEdit, btnCertificateRevoke, btnCertificateArchive, btnCertificateRefresh }, gridCertificates);
    private void BuildMchdTab() => BuildRegistryTab(tabMchd, txtMchdSearch, new Control[] { cmbMchdStatus, btnMchdAdd, btnMchdEdit, btnMchdRevoke, btnMchdArchive, btnMchdRefresh }, gridMchd);
    private void BuildTokensTab() => BuildRegistryTab(tabTokens, txtTokenSearch, new Control[] { cmbTokenStatus, btnTokenAdd, btnTokenEdit, btnTokenIssue, btnTokenReturn, btnTokenWriteOff, btnTokenRefresh }, gridTokens);

    private void BuildReportsTab()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 70, Padding = new Padding(12) };
        cmbReportType.Width = 240; cmbReportType.Items.AddRange(new object[] { "Сотрудники", "Сертификаты", "МЧД", "Токены", "Операции токенов" }); cmbReportType.SelectedIndex = 0;
        lblReportPath.AutoSize = true; lblReportPath.Margin = new Padding(12, 10, 4, 4);
        panel.Controls.Add(new Label { Text = "Отчет:", AutoSize = true, Margin = new Padding(4, 10, 4, 4) }); panel.Controls.Add(cmbReportType); panel.Controls.Add(btnExportCsv); panel.Controls.Add(btnExportXlsx); panel.Controls.Add(lblReportPath);
        tabReports.Controls.Add(panel);
    }

    private void BuildAuditTab()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(12) };
        panel.Controls.Add(btnAuditRefresh); panel.Controls.Add(btnAuditClear);
        tabAudit.Controls.Add(gridAudit); tabAudit.Controls.Add(panel);
    }

    private void BuildRegistryTab(TabPage page, TextBox search, Control[] actions, DataGridView grid)
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 56, Padding = new Padding(12) };
        search.Width = 240; search.PlaceholderText = "Поиск";
        panel.Controls.Add(search);
        foreach (var action in actions) panel.Controls.Add(action);
        page.Controls.Add(grid); page.Controls.Add(panel);
    }

    private static Button Button(string text) => new() { Text = text, Width = 120, Height = 32, Margin = new Padding(4) };
    private static ComboBox Combo() => new() { Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(4) };
    private static DataGridView Grid() => new() { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, RowHeadersVisible = false };
    private static Label MetricLabel() => new() { Width = 180, Height = 85, Margin = new Padding(6), Padding = new Padding(8), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
}
