using CertDesk.Common;

namespace CertDesk.Forms;

public partial class LoginForm
{
    private System.ComponentModel.IContainer? components;
    private TableLayoutPanel layoutPanel = null!;
    private TextBox loginTextBox = null!;
    private TextBox passwordTextBox = null!;
    private Button loginButton = null!;
    private Button exitButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        layoutPanel = new TableLayoutPanel();
        loginTextBox = UiTheme.TextBox();
        passwordTextBox = UiTheme.TextBox();
        loginButton = UiTheme.Button("Войти");
        exitButton = UiTheme.Button("Выход");
        var buttonsPanel = new FlowLayoutPanel();

        SuspendLayout();
        Text = "CertDesk — вход";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(420, 300);
        BackColor = UiTheme.Background;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        AcceptButton = loginButton;
        CancelButton = exitButton;

        layoutPanel.Dock = DockStyle.Fill;
        layoutPanel.Padding = new Padding(34);
        layoutPanel.RowCount = 7;
        layoutPanel.ColumnCount = 1;
        layoutPanel.Controls.Add(UiTheme.Label("CertDesk", 20, true));
        layoutPanel.Controls.Add(UiTheme.Label("Контроль сертификатов ЭП и МЧД"));
        layoutPanel.Controls.Add(UiTheme.Label("Логин"));
        layoutPanel.Controls.Add(loginTextBox);
        layoutPanel.Controls.Add(UiTheme.Label("Пароль"));
        passwordTextBox.UseSystemPasswordChar = true;
        layoutPanel.Controls.Add(passwordTextBox);

        buttonsPanel.Dock = DockStyle.Fill;
        buttonsPanel.Controls.Add(loginButton);
        buttonsPanel.Controls.Add(exitButton);
        layoutPanel.Controls.Add(buttonsPanel);
        Controls.Add(layoutPanel);

        loginButton.Click += LoginButton_Click;
        exitButton.Click += ExitButton_Click;
        ResumeLayout(false);
    }
}
