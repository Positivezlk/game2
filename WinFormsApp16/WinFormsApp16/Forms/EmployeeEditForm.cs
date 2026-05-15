using CertDesk.Common;
using CertDesk.Models;
using CertDesk.Services;

namespace CertDesk.Forms;

public partial class EmployeeEditForm : Form
{
    private readonly Employee employee;
    private readonly EmployeeService service;

    public EmployeeEditForm(Employee employee, EmployeeService service)
    {
        this.employee = employee;
        this.service = service;
        InitializeComponent();
        LoadEmployee();
    }

    private void LoadEmployee()
    {
        fullNameTextBox.Text = employee.FullName;
        positionTextBox.Text = employee.Position;
        departmentTextBox.Text = employee.Department;
        emailTextBox.Text = employee.Email;
        phoneTextBox.Text = employee.Phone;
        snilsTextBox.Text = employee.Snils;
        innTextBox.Text = employee.Inn;
        activeCheckBox.Checked = employee.IsActive;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        try
        {
            employee.FullName = fullNameTextBox.Text;
            employee.Position = positionTextBox.Text;
            employee.Department = departmentTextBox.Text;
            employee.Email = emailTextBox.Text;
            employee.Phone = phoneTextBox.Text;
            employee.Snils = snilsTextBox.Text;
            employee.Inn = innTextBox.Text;
            employee.IsActive = activeCheckBox.Checked;
            service.Save(employee);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { MessageHelper.Error(ex); }
    }
}
