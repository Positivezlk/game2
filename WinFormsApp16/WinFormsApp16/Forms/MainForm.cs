using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;
using CertDesk.Views;

namespace CertDesk.Forms;

public partial class MainForm : Form
{
    public static bool RestartLogin { get; private set; }
    private readonly CurrentUser user;

    public MainForm(CurrentUser user)
    {
        this.user = user;
        RestartLogin = false;
        InitializeComponent();
        userLabel.Text = $"{user.Login} — {AuthService.RoleTitle(user.Role)}";
        statusLabel.Text = $"Готово | {DateTime.Now:dd.MM.yyyy} | роль: {AuthService.RoleTitle(user.Role)}";
        BuildMenu();
        ShowDashboard();
    }

    private void BuildMenu()
    {
        AddMenuButton("Главная", ShowDashboard);
        AddMenuButton("Сертификаты", () => ShowView(new CertificatesView(user)));
        AddMenuButton("МЧД", () => ShowView(new MchdView(user)));
        AddMenuButton("Токены", () => ShowView(new TokensView(user)));
        AddMenuButton("Сотрудники", () => ShowView(new EmployeesView(user)));
        if (RoleGuard.CanExport(user)) AddMenuButton("Отчеты", () => ShowView(new ReportsView(user)));
        if (RoleGuard.CanOpenAudit(user)) AddMenuButton("Аудит", () => ShowView(new AuditView(user)));
        if (RoleGuard.CanBackup(user)) AddMenuButton("Резервная копия", CreateBackup);
        AddMenuButton("Выход", Logout);
    }

    private void AddMenuButton(string text, Action action)
    {
        var button = UiTheme.ApplyButton(new Button
        {
            Text = text,
            Width = 220,
            Height = 42,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(18, 0, 0, 0),
            Margin = Padding.Empty
        });
        button.MouseEnter += (_, _) => button.BackColor = UiTheme.DarkBlue;
        button.MouseLeave += (_, _) => button.BackColor = UiTheme.Blue;
        button.Click += (_, _) => action();
        sidebarPanel.Controls.Add(button);
    }

    private void ShowDashboard() => ShowView(new DashboardView());

    private void ShowView(UserControl view)
    {
        contentPanel.Controls.Clear();
        view.Dock = DockStyle.Fill;
        contentPanel.Controls.Add(view);
    }

    private void CreateBackup()
    {
        try
        {
            var path = new BackupService(user).CreateBackup();
            MessageHelper.Info("Резервная копия создана:\n" + path);
        }
        catch (Exception ex)
        {
            MessageHelper.Error(ex);
        }
    }

    private void Logout()
    {
        RestartLogin = true;
        Close();
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        new AuditService(user).Log("выход", "user", user.Id, "Выход из системы");
    }
}
