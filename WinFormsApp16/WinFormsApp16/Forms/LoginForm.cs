using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Forms;

public partial class LoginForm : Form
{
    private CurrentUser? currentUser;

    public LoginForm()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object? sender, EventArgs e) => SignIn();

    private void ExitButton_Click(object? sender, EventArgs e) => Close();

    private void SignIn()
    {
        var user = new AuthService().Login(loginTextBox.Text.Trim(), passwordTextBox.Text);
        if (user == null)
        {
            MessageHelper.Warning("Неверный логин или пароль");
            return;
        }

        currentUser = user;
        Hide();
        using var mainForm = new MainForm(currentUser);
        mainForm.ShowDialog(this);

        if (MainForm.RestartLogin)
        {
            passwordTextBox.Clear();
            Show();
        }
        else
        {
            Close();
        }
    }
}
