using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;
using System.Data;

namespace CertDesk.Forms;

public partial class CertificateEditForm : Form
{
    private readonly Certificate certificate;
    private readonly CertificateService service;

    public CertificateEditForm(Certificate certificate, CertificateService service)
    {
        this.certificate = certificate;
        this.service = service;
        InitializeComponent();
        BindLookups();
        LoadCertificate();
    }

    private void BindLookups()
    {
        Bind(employeeComboBox, LookupService.Employees());
        Bind(authorityComboBox, LookupService.Authorities());
        var tokens = LookupService.Tokens();
        tokens.Rows.Add(DBNull.Value, "— без токена —");
        Bind(tokenComboBox, tokens);
        signatureComboBox.Items.AddRange(["SES", "NES", "QES"]);
    }

    private void LoadCertificate()
    {
        serialTextBox.Text = certificate.SerialNumber;
        purposeTextBox.Text = certificate.Purpose;
        issuedDatePicker.Value = certificate.IssuedAt == default ? DateTime.Today : certificate.IssuedAt;
        validFromPicker.Value = certificate.ValidFrom == default ? DateTime.Today : certificate.ValidFrom;
        validToPicker.Value = certificate.ValidTo == default ? DateTime.Today.AddYears(1) : certificate.ValidTo;
        signatureComboBox.SelectedItem = certificate.SignatureType;
        SetSelected(employeeComboBox, certificate.EmployeeId);
        SetSelected(authorityComboBox, certificate.AuthorityId);
        SetSelected(tokenComboBox, certificate.TokenId);
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        try
        {
            certificate.EmployeeId = SelectedId(employeeComboBox) ?? 0;
            certificate.AuthorityId = SelectedId(authorityComboBox) ?? 0;
            certificate.TokenId = SelectedId(tokenComboBox);
            certificate.SerialNumber = serialTextBox.Text;
            certificate.SignatureType = signatureComboBox.Text;
            certificate.IssuedAt = issuedDatePicker.Value;
            certificate.ValidFrom = validFromPicker.Value;
            certificate.ValidTo = validToPicker.Value;
            certificate.Purpose = purposeTextBox.Text;
            service.Save(certificate);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { MessageHelper.Error(ex); }
    }

    private static void Bind(ComboBox comboBox, DataTable table) { comboBox.DataSource = table; comboBox.ValueMember = "id"; comboBox.DisplayMember = "name"; }
    private static int? SelectedId(ComboBox comboBox) => comboBox.SelectedValue is null or DBNull ? null : Convert.ToInt32(comboBox.SelectedValue);
    private static void SetSelected(ComboBox comboBox, int? value) { if (value.HasValue) comboBox.SelectedValue = value.Value; }
}
