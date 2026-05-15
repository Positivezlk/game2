using CertDesk.Models; using CertDesk.Services;
namespace CertDesk.Forms;
public sealed class LoginForm:Form
{
 readonly TextBox login=Ui.TextBox(); readonly TextBox password=Ui.TextBox(){UseSystemPasswordChar=true}; private CurrentUser? currentUser;
 public LoginForm(){Text="CertDesk — вход"; StartPosition=FormStartPosition.CenterScreen; ClientSize=new Size(420,300); BackColor=Ui.Bg; FormBorderStyle=FormBorderStyle.FixedDialog; MaximizeBox=false; var p=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(34),RowCount=7}; p.Controls.Add(Ui.Label("CertDesk",20,true)); p.Controls.Add(Ui.Label("Контроль сертификатов ЭП и МЧД",10)); p.Controls.Add(Ui.Label("Логин")); p.Controls.Add(login); p.Controls.Add(Ui.Label("Пароль")); p.Controls.Add(password); var buttons=new FlowLayoutPanel{Dock=DockStyle.Fill}; var enter=Ui.Button("Войти"); var exit=Ui.Button("Выход"); buttons.Controls.AddRange([enter,exit]); p.Controls.Add(buttons); Controls.Add(p); enter.Click+=(_,_)=>DoLogin(); exit.Click+=(_,_)=>Close(); AcceptButton=enter; CancelButton=exit; }
 void DoLogin(){var user=new AuthService().Login(login.Text.Trim(),password.Text); if(user==null){MessageBox.Show("Неверный логин или пароль","CertDesk",MessageBoxButtons.OK,MessageBoxIcon.Warning);return;} currentUser=user; Hide(); using var main = new MainForm(currentUser); main.ShowDialog(this); if (MainForm.RestartLogin) { password.Clear(); Show(); } else { Close(); }}
}
