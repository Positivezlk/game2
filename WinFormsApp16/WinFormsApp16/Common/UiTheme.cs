namespace CertDesk.Common;

public static class UiTheme
{
    public static readonly Color Blue = ColorTranslator.FromHtml("#2F5F9F");
    public static readonly Color DarkBlue = ColorTranslator.FromHtml("#244A7C");
    public static readonly Color Background = ColorTranslator.FromHtml("#F2F4F7");
    public static readonly Color White = ColorTranslator.FromHtml("#FFFFFF");
    public static readonly Color Border = ColorTranslator.FromHtml("#D7DCE2");
    public static readonly Color Text = ColorTranslator.FromHtml("#1F2933");
    public static readonly Color Muted = ColorTranslator.FromHtml("#6B7280");
    public static readonly Color Active = ColorTranslator.FromHtml("#2E7D32");
    public static readonly Color Warning = ColorTranslator.FromHtml("#F39C12");
    public static readonly Color Expired = ColorTranslator.FromHtml("#C0392B");
    public static readonly Color Archived = ColorTranslator.FromHtml("#7F8C8D");

    public static Label Label(string text, int size = 10, bool bold = false) => new()
    {
        Text = text,
        AutoSize = true,
        ForeColor = Text,
        Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
        Margin = new Padding(4, 8, 4, 2)
    };

    public static TextBox TextBox(int width = 260) => new() { Width = width, Margin = new Padding(4) };

    public static ComboBox ComboBox(int width = 260) => new()
    {
        Width = width,
        DropDownStyle = ComboBoxStyle.DropDownList,
        Margin = new Padding(4)
    };

    public static Button Button(string text, int width = 120) => ApplyButton(new Button { Text = text, Width = width });

    public static Button ApplyButton(Button button)
    {
        button.Height = 34;
        button.BackColor = Blue;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Margin = new Padding(4);
        return button;
    }

    public static DataGridView Grid() => ApplyGrid(new DataGridView());

    public static DataGridView ApplyGrid(DataGridView grid)
    {
        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = White;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.RowHeadersVisible = false;
        return grid;
    }

    public static Color StatusColor(string status) => status switch
    {
        "active" or "issued" => Active,
        "warning" => Warning,
        "expired" or "revoked" => Expired,
        "archived" or "storage" or "damaged" or "written_off" => Archived,
        _ => Text
    };
}
