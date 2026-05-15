using CertDesk.Services;
namespace CertDesk.Forms;
public sealed class TokenIssueForm:Form
{ public TokenIssueForm(int tokenId,TokenService service){Text="Выдача токена"; StartPosition=FormStartPosition.CenterParent; Width=390; Height=320; var p=new FlowLayoutPanel{Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,Padding=new Padding(16)}; ComboBox emp=Ui.Combo(); emp.DataSource=LookupService.Employees(); emp.ValueMember="id"; emp.DisplayMember="name"; TextBox act=Ui.TextBox(), comment=Ui.TextBox(); foreach(Control c in [Ui.Label("Сотрудник"),emp,Ui.Label("Номер акта"),act,Ui.Label("Комментарий"),comment]) p.Controls.Add(c); var save=Ui.Button("Выдать"); p.Controls.Add(save); Controls.Add(p); save.Click+=(_,_)=>{try{service.Issue(tokenId,Convert.ToInt32(emp.SelectedValue),act.Text,comment.Text);DialogResult=DialogResult.OK;Close();}catch(Exception ex){Ui.Error(ex);}};}
}
