using CertDesk.Common;

namespace CertDesk.Views;

public partial class DashboardView
{
    private System.ComponentModel.IContainer? components;
    private FlowLayoutPanel cardsPanel = null!;
    private DataGridView attentionGrid = null!;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1 };
        cardsPanel = new FlowLayoutPanel { Dock = DockStyle.Fill };
        attentionGrid = UiTheme.Grid();
        SuspendLayout(); BackColor = UiTheme.White;
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(UiTheme.Label("Главная панель", 18, true)); root.Controls.Add(cardsPanel); root.Controls.Add(attentionGrid); Controls.Add(root); ResumeLayout(false);
    }
}
