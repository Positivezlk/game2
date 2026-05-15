using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;
using System.Data;

namespace CertDesk.Forms;

public partial class MchdEditForm : Form
{
    private readonly Mchd mchd;
    private readonly MchdService service;

    public MchdEditForm(Mchd mchd, MchdService service)
    {
        this.mchd = mchd;
        this.service = service;
        InitializeComponent();
        BindLookups();
        LoadMchd();
    }

    private void BindLookups()
    {
        Bind(principalComboBox, LookupService.Employees());
        Bind(representativeComboBox, LookupService.Employees());
        var certificates = LookupService.Certificates();
        certificates.Rows.Add(DBNull.Value, "— без сертификата —");
        Bind(certificateComboBox, certificates);
    }

    private void LoadMchd()
    {
        numberTextBox.Text = mchd.Number;
        powersTextBox.Text = mchd.Powers;
        codesTextBox.Text = mchd.PowersCodes;
        notesTextBox.Text = mchd.Notes;
        validFromPicker.Value = mchd.ValidFrom == default ? DateTime.Today : mchd.ValidFrom;
        validToPicker.Value = mchd.ValidTo == default ? DateTime.Today.AddYears(1) : mchd.ValidTo;
        registeredCheckBox.Checked = mchd.IsRegistered;
        SetSelected(principalComboBox, mchd.PrincipalEmployeeId);
        SetSelected(representativeComboBox, mchd.RepresentativeEmployeeId);
        SetSelected(certificateComboBox, mchd.CertificateId);
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        try
        {
            mchd.Number = numberTextBox.Text;
            mchd.PrincipalEmployeeId = SelectedId(principalComboBox);
            mchd.RepresentativeEmployeeId = SelectedId(representativeComboBox) ?? 0;
            mchd.CertificateId = SelectedId(certificateComboBox);
            mchd.Powers = powersTextBox.Text;
            mchd.PowersCodes = codesTextBox.Text;
            mchd.ValidFrom = validFromPicker.Value;
            mchd.ValidTo = validToPicker.Value;
            mchd.IsRegistered = registeredCheckBox.Checked;
            mchd.Notes = notesTextBox.Text;
            service.Save(mchd);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { MessageHelper.Error(ex); }
    }

    private static void Bind(ComboBox comboBox, DataTable table) { comboBox.DataSource = table; comboBox.ValueMember = "id"; comboBox.DisplayMember = "name"; }
    private static int? SelectedId(ComboBox comboBox) => comboBox.SelectedValue is null or DBNull ? null : Convert.ToInt32(comboBox.SelectedValue);
    private static void SetSelected(ComboBox comboBox, int? value) { if (value.HasValue) comboBox.SelectedValue = value.Value; }
}
