using CertDesk.Models; using CertDesk.Services;
namespace CertDesk.Forms;
public sealed class AuditView:UserControl
{ readonly AuditService service; readonly DataGridView grid=Ui.Grid(); readonly DateTimePicker from=new(){Width=130}, to=new(){Width=130}; readonly TextBox userBox=Ui.TextBox(), action=Ui.TextBox(), entity=Ui.TextBox();
 public AuditView(CurrentUser user){service=new(user); if(!AuthService.CanAudit(user)){MessageBox.Show("Недостаточно прав для выполнения операции."); return;} var top=new FlowLayoutPanel{Dock=DockStyle.Top,Height=86}; userBox.Width=130; action.Width=150; entity.Width=120; from.Value=DateTime.Today.AddMonths(-2); to.Value=DateTime.Today; var refresh=Ui.Button("Обновить"); top.Controls.AddRange([Ui.Label("Дата с"),from,Ui.Label("по"),to,Ui.Label("Пользователь"),userBox,Ui.Label("Действие"),action,Ui.Label("Объект"),entity,refresh]); Controls.Add(grid); Controls.Add(top); refresh.Click+=(_,_)=>LoadData(); LoadData();}
 void LoadData(){grid.DataSource=service.Search(from.Value,to.Value,userBox.Text,action.Text,entity.Text);}
}
