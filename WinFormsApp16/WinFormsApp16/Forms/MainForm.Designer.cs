using CertDesk.Common;

namespace CertDesk.Forms;

public partial class MainForm
{
    private System.ComponentModel.IContainer? components;
    private FlowLayoutPanel sidebarPanel = null!;
    private Panel headerPanel = null!;
    private Panel contentPanel = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel statusLabel = null!;
    private Label userLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        sidebarPanel = new FlowLayoutPanel();
        headerPanel = new Panel();
        contentPanel = new Panel();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        userLabel = UiTheme.Label(string.Empty, 10, true);
        var titleLabel = UiTheme.Label("CertDesk", 20, true);
        var subtitleLabel = UiTheme.Label("Контроль сертификатов ЭП и МЧД");

        SuspendLayout();
        Text = "CertDesk";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1200, 750);
        MinimumSize = new Size(1000, 650);
        BackColor = UiTheme.Background;

        sidebarPanel.Dock = DockStyle.Left;
        sidebarPanel.Width = 220;
        sidebarPanel.BackColor = UiTheme.Blue;
        sidebarPanel.Padding = new Padding(0, 12, 0, 0);
        sidebarPanel.FlowDirection = FlowDirection.TopDown;
        sidebarPanel.WrapContents = false;

        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 72;
        headerPanel.BackColor = UiTheme.White;
        headerPanel.Padding = new Padding(18, 8, 18, 8);
        userLabel.Dock = DockStyle.Right;
        headerPanel.Controls.Add(userLabel);
        headerPanel.Controls.Add(subtitleLabel);
        headerPanel.Controls.Add(titleLabel);

        contentPanel.Dock = DockStyle.Fill;
        contentPanel.BackColor = UiTheme.White;
        contentPanel.Padding = new Padding(18);

        statusStrip.Items.Add(statusLabel);
        statusStrip.Dock = DockStyle.Bottom;

        Controls.Add(contentPanel);
        Controls.Add(sidebarPanel);
        Controls.Add(headerPanel);
        Controls.Add(statusStrip);
        FormClosing += MainForm_FormClosing;
        ResumeLayout(false);
        PerformLayout();
    }
}
