using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Forms;

public partial class TokenEditForm : Form
{
    private readonly Token token;
    private readonly TokenService service;

    public TokenEditForm(Token token, TokenService service)
    {
        this.token = token;
        this.service = service;
        InitializeComponent();
        BindLookups();
        LoadToken();
    }

    private void BindLookups()
    {
        statusComboBox.Items.AddRange(["storage", "issued", "damaged", "written_off"]);
        holderComboBox.DataSource = LookupService.Employees();
        holderComboBox.ValueMember = "id";
        holderComboBox.DisplayMember = "name";
    }

    private void LoadToken()
    {
        inventoryTextBox.Text = token.InventoryNumber;
        typeTextBox.Text = token.TokenType;
        modelTextBox.Text = token.Model;
        serialTextBox.Text = token.SerialNumber;
        notesTextBox.Text = token.Notes;
        statusComboBox.SelectedItem = token.Status;
        if (token.HolderId.HasValue) holderComboBox.SelectedValue = token.HolderId.Value;
        receivedDatePicker.Value = token.ReceivedAt ?? DateTime.Today;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        try
        {
            token.InventoryNumber = inventoryTextBox.Text;
            token.TokenType = typeTextBox.Text;
            token.Model = modelTextBox.Text;
            token.SerialNumber = serialTextBox.Text;
            token.ReceivedAt = receivedDatePicker.Value;
            token.Status = statusComboBox.Text;
            token.HolderId = statusComboBox.Text == "issued" ? (int?)Convert.ToInt32(holderComboBox.SelectedValue) : null;
            token.Notes = notesTextBox.Text;
            service.Save(token);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { MessageHelper.Error(ex); }
    }
}
