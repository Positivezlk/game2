using CertDesk.Common;
using CertDesk.Services;

namespace CertDesk.Views;

public partial class DashboardView : UserControl
{
    private readonly DashboardService service = new();

    public DashboardView()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        cardsPanel.Controls.Clear();
        foreach (var metric in service.GetMetrics())
            cardsPanel.Controls.Add(CreateCard(metric.Title, metric.Value, UiTheme.StatusColor(metric.Status)));
        attentionGrid.DataSource = service.GetAttentionTable();
    }

    private static Control CreateCard(string title, string value, Color valueColor)
    {
        var panel = new Panel { Width = 180, Height = 92, BackColor = UiTheme.Background, Margin = new Padding(6), Padding = new Padding(12) };
        var valueLabel = UiTheme.Label(value, 22, true);
        valueLabel.ForeColor = valueColor;
        var titleLabel = UiTheme.Label(title, 9);
        titleLabel.ForeColor = UiTheme.Muted;
        titleLabel.Top = 52;
        panel.Controls.Add(valueLabel);
        panel.Controls.Add(titleLabel);
        return panel;
    }
}
