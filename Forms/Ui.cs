namespace CertDesk.Forms;
public static class Ui
{
 public static readonly Color Blue=ColorTranslator.FromHtml("#2F5F9F"), DarkBlue=ColorTranslator.FromHtml("#244A7C"), Bg=ColorTranslator.FromHtml("#F2F4F7"), Border=ColorTranslator.FromHtml("#D7DCE2"), Text=ColorTranslator.FromHtml("#1F2933"), Muted=ColorTranslator.FromHtml("#6B7280");
 public static Button Button(string text)=>new(){Text=text,Height=34,Width=120,BackColor=Blue,ForeColor=Color.White,FlatStyle=FlatStyle.Flat,Margin=new Padding(4)};
 public static Label Label(string text,int size=10,bool bold=false)=>new(){Text=text,AutoSize=true,ForeColor=Text,Font=new Font("Segoe UI",size,bold?FontStyle.Bold:FontStyle.Regular),Margin=new Padding(4,8,4,2)};
 public static TextBox TextBox()=>new(){Width=260,Margin=new Padding(4)};
 public static ComboBox Combo()=>new(){Width=260,DropDownStyle=ComboBoxStyle.DropDownList,Margin=new Padding(4)};
 public static DataGridView Grid()=>new(){Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,AllowUserToDeleteRows=false,SelectionMode=DataGridViewSelectionMode.FullRowSelect,MultiSelect=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,BackgroundColor=Color.White,BorderStyle=BorderStyle.FixedSingle,RowHeadersVisible=false};
 public static void Error(Exception ex)=>MessageBox.Show(ex.Message,"CertDesk",MessageBoxButtons.OK,MessageBoxIcon.Warning);
}
