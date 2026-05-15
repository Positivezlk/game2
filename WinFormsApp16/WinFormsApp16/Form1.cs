using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp16;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        WireEvents();
        LoadAllData();
        ApplyRole();
    }

    private string CurrentRole => roleComboBox.Text;
    private int SelectedId(DataGridView grid) => grid.CurrentRow == null ? 0 : Convert.ToInt32(grid.CurrentRow.Cells["ID"].Value);

    private void WireEvents()
    {
        roleComboBox.SelectedIndexChanged += (_, _) => ApplyRole();
        txtEmployeeSearch.TextChanged += (_, _) => LoadEmployees();
        txtCertificateSearch.TextChanged += (_, _) => LoadCertificates();
        txtMchdSearch.TextChanged += (_, _) => LoadMchd();
        txtTokenSearch.TextChanged += (_, _) => LoadTokens();
        cmbCertificateStatus.Items.AddRange(new object[] { "Все", "active", "warning", "expired", "revoked", "archived" }); cmbCertificateStatus.SelectedIndex = 0; cmbCertificateStatus.SelectedIndexChanged += (_, _) => LoadCertificates();
        cmbMchdStatus.Items.AddRange(new object[] { "Все", "active", "warning", "expired", "revoked", "archived" }); cmbMchdStatus.SelectedIndex = 0; cmbMchdStatus.SelectedIndexChanged += (_, _) => LoadMchd();
        cmbTokenStatus.Items.AddRange(new object[] { "Все", "storage", "issued", "damaged", "written_off" }); cmbTokenStatus.SelectedIndex = 0; cmbTokenStatus.SelectedIndexChanged += (_, _) => LoadTokens();
        btnEmployeeAdd.Click += (_, _) => AddEmployee(); btnEmployeeEdit.Click += (_, _) => EditEmployee(); btnEmployeeArchive.Click += (_, _) => ArchiveEmployee(); btnEmployeeRefresh.Click += (_, _) => LoadEmployees();
        btnCertificateAdd.Click += (_, _) => AddCertificate(); btnCertificateEdit.Click += (_, _) => EditCertificate(); btnCertificateRevoke.Click += (_, _) => SetCertificateStatus("revoked"); btnCertificateArchive.Click += (_, _) => SetCertificateStatus("archived"); btnCertificateRefresh.Click += (_, _) => LoadCertificates();
        btnMchdAdd.Click += (_, _) => AddMchd(); btnMchdEdit.Click += (_, _) => EditMchd(); btnMchdRevoke.Click += (_, _) => SetMchdStatus("revoked"); btnMchdArchive.Click += (_, _) => SetMchdStatus("archived"); btnMchdRefresh.Click += (_, _) => LoadMchd();
        btnTokenAdd.Click += (_, _) => AddToken(); btnTokenEdit.Click += (_, _) => EditToken(); btnTokenIssue.Click += (_, _) => IssueToken(); btnTokenReturn.Click += (_, _) => ReturnToken(); btnTokenWriteOff.Click += (_, _) => WriteOffToken(); btnTokenRefresh.Click += (_, _) => LoadTokens();
        btnExportCsv.Click += (_, _) => ExportReport(false); btnExportXlsx.Click += (_, _) => ExportReport(true);
        btnAuditRefresh.Click += (_, _) => LoadAudit(); btnAuditClear.Click += (_, _) => ClearAudit();
    }

    private void ApplyRole()
    {
        var canEdit = AppRoles.CanEdit(CurrentRole);
        foreach (var button in new[] { btnEmployeeAdd, btnEmployeeEdit, btnEmployeeArchive, btnCertificateAdd, btnCertificateEdit, btnCertificateRevoke, btnCertificateArchive, btnMchdAdd, btnMchdEdit, btnMchdRevoke, btnMchdArchive, btnTokenAdd, btnTokenEdit, btnTokenIssue, btnTokenReturn, btnTokenWriteOff })
            button.Enabled = canEdit;
        btnAuditClear.Enabled = AppRoles.CanClearAudit(CurrentRole);
    }

    private bool EnsureEdit()
    {
        if (AppRoles.CanEdit(CurrentRole)) return true;
        MessageBox.Show("Недостаточно прав для выполнения операции.", "CertDesk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void LoadAllData()
    {
        Database.RefreshStatuses();
        LoadDashboard(); LoadEmployees(); LoadCertificates(); LoadMchd(); LoadTokens(); LoadAudit();
    }

    private void LoadDashboard()
    {
        lblEmployeesCount.Text = "Сотрудников\n" + Database.Scalar("SELECT COUNT(*) FROM employees");
        lblCertificatesCount.Text = "Сертификатов\n" + Database.Scalar("SELECT COUNT(*) FROM certificates");
        lblCertificateWarnings.Text = "Истекают сертификаты\n" + Database.Scalar("SELECT COUNT(*) FROM certificates WHERE status='warning'");
        lblMchdCount.Text = "МЧД\n" + Database.Scalar("SELECT COUNT(*) FROM mchd");
        lblTokensCount.Text = "Токенов\n" + Database.Scalar("SELECT COUNT(*) FROM tokens");
        lblIssuedTokens.Text = "Выдано токенов\n" + Database.Scalar("SELECT COUNT(*) FROM tokens WHERE status='issued'");
        gridAttention.DataSource = Database.Query("SELECT 'Сертификат' AS 'Тип записи', c.serial_number AS 'Номер', e.full_name AS 'Сотрудник', c.valid_to AS 'Действует до', c.status AS 'Статус' FROM certificates c LEFT JOIN employees e ON e.id=c.employee_id WHERE c.status IN ('warning','expired') UNION ALL SELECT 'МЧД', m.number, e.full_name, m.valid_to, m.status FROM mchd m LEFT JOIN employees e ON e.id=m.representative_id WHERE m.status IN ('warning','expired')");
    }

    private void LoadEmployees() => gridEmployees.DataSource = Database.Query("SELECT id AS 'ID', full_name AS 'ФИО', position AS 'Должность', department AS 'Подразделение', email AS 'Email', phone AS 'Телефон', CASE is_active WHEN 1 THEN 'активен' ELSE 'архивный' END AS 'Статус' FROM employees WHERE full_name LIKE @q OR position LIKE @q OR department LIKE @q OR email LIKE @q ORDER BY full_name", ("@q", $"%{txtEmployeeSearch.Text}%"));

    private void LoadCertificates()
    {
        Database.RefreshStatuses();
        var statusWhere = cmbCertificateStatus.Text == "Все" ? "" : " AND c.status=@status";
        gridCertificates.DataSource = Database.Query("SELECT c.id AS 'ID', c.serial_number AS 'Серийный номер', e.full_name AS 'Владелец', c.signature_type AS 'Тип подписи', c.authority AS 'УЦ', c.valid_from AS 'Действует с', c.valid_to AS 'Действует до', CAST(julianday(c.valid_to)-julianday('now','start of day') AS INT) AS 'Осталось дней', c.status AS 'Статус', c.purpose AS 'Назначение' FROM certificates c LEFT JOIN employees e ON e.id=c.employee_id WHERE (c.serial_number LIKE @q OR e.full_name LIKE @q OR c.authority LIKE @q)" + statusWhere + " ORDER BY c.valid_to", ("@q", $"%{txtCertificateSearch.Text}%"), ("@status", cmbCertificateStatus.Text));
        LoadDashboard();
    }

    private void LoadMchd()
    {
        Database.RefreshStatuses();
        var statusWhere = cmbMchdStatus.Text == "Все" ? "" : " AND m.status=@status";
        gridMchd.DataSource = Database.Query("SELECT m.id AS 'ID', m.number AS 'Номер МЧД', m.principal AS 'Доверитель', e.full_name AS 'Представитель', m.powers AS 'Полномочия', m.valid_from AS 'Действует с', m.valid_to AS 'Действует до', CAST(julianday(m.valid_to)-julianday('now','start of day') AS INT) AS 'Осталось дней', CASE m.is_registered WHEN 1 THEN 'да' ELSE 'нет' END AS 'Зарегистрирована', m.status AS 'Статус' FROM mchd m LEFT JOIN employees e ON e.id=m.representative_id WHERE (m.number LIKE @q OR e.full_name LIKE @q OR m.powers LIKE @q)" + statusWhere + " ORDER BY m.valid_to", ("@q", $"%{txtMchdSearch.Text}%"), ("@status", cmbMchdStatus.Text));
        LoadDashboard();
    }

    private void LoadTokens()
    {
        var statusWhere = cmbTokenStatus.Text == "Все" ? "" : " AND t.status=@status";
        gridTokens.DataSource = Database.Query("SELECT t.id AS 'ID', t.inventory_number AS 'Инвентарный номер', t.token_type AS 'Тип', t.model AS 'Модель', t.serial_number AS 'Серийный номер', t.status AS 'Статус', e.full_name AS 'Держатель' FROM tokens t LEFT JOIN employees e ON e.id=t.holder_id WHERE (t.inventory_number LIKE @q OR t.model LIKE @q OR t.serial_number LIKE @q OR e.full_name LIKE @q)" + statusWhere + " ORDER BY t.inventory_number", ("@q", $"%{txtTokenSearch.Text}%"), ("@status", cmbTokenStatus.Text));
        LoadDashboard();
    }

    private void LoadAudit() => gridAudit.DataSource = Database.Query("SELECT id AS 'ID', created_at AS 'Дата', action AS 'Действие', entity AS 'Объект', description AS 'Описание' FROM audit_log ORDER BY created_at DESC");

    private void AddEmployee()
    {
        if (!EnsureEdit()) return;
        var values = ShowEditor("Сотрудник", new Dictionary<string, string> { ["ФИО"] = "", ["Должность"] = "Специалист", ["Подразделение"] = "", ["Email"] = "", ["Телефон"] = "" });
        if (values == null || string.IsNullOrWhiteSpace(values["ФИО"])) return;
        Database.Execute("INSERT INTO employees(full_name,position,department,email,phone,is_active) VALUES(@n,@p,@d,@e,@ph,1)", ("@n", values["ФИО"]), ("@p", values["Должность"]), ("@d", values["Подразделение"]), ("@e", values["Email"]), ("@ph", values["Телефон"]));
        Database.Log("создание сотрудника", "employee", values["ФИО"]); LoadEmployees(); LoadDashboard(); LoadAudit();
    }

    private void EditEmployee()
    {
        if (!EnsureEdit()) return; var id = SelectedId(gridEmployees); if (id == 0) return;
        var row = Database.Query("SELECT * FROM employees WHERE id=@id", ("@id", id)).Rows[0];
        var values = ShowEditor("Сотрудник", new Dictionary<string, string> { ["ФИО"] = row["full_name"].ToString()!, ["Должность"] = row["position"].ToString()!, ["Подразделение"] = row["department"].ToString()!, ["Email"] = row["email"].ToString()!, ["Телефон"] = row["phone"].ToString()! });
        if (values == null) return;
        Database.Execute("UPDATE employees SET full_name=@n,position=@p,department=@d,email=@e,phone=@ph WHERE id=@id", ("@n", values["ФИО"]), ("@p", values["Должность"]), ("@d", values["Подразделение"]), ("@e", values["Email"]), ("@ph", values["Телефон"]), ("@id", id));
        Database.Log("изменение сотрудника", "employee", values["ФИО"]); LoadEmployees(); LoadAudit();
    }

    private void ArchiveEmployee()
    {
        if (!EnsureEdit()) return; var id = SelectedId(gridEmployees); if (id == 0) return;
        Database.Execute("UPDATE employees SET is_active=0 WHERE id=@id", ("@id", id)); Database.Log("архивирование сотрудника", "employee", $"ID {id}"); LoadEmployees(); LoadDashboard(); LoadAudit();
    }

    private void AddCertificate()
    {
        if (!EnsureEdit()) return;
        var values = ShowEditor("Сертификат", new Dictionary<string, string> { ["ID сотрудника"] = "1", ["Серийный номер"] = "", ["Тип подписи"] = "QES", ["УЦ"] = "УЦ Федерального казначейства", ["Действует с"] = DateTime.Today.ToString("yyyy-MM-dd"), ["Действует до"] = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd"), ["Назначение"] = "" });
        if (values == null) return; var status = StatusText.CertificateStatus(DateTime.Parse(values["Действует до"]), "active");
        Database.Execute("INSERT INTO certificates(employee_id,serial_number,signature_type,authority,valid_from,valid_to,status,purpose) VALUES(@e,@s,@t,@a,@vf,@vt,@st,@p)", ("@e", int.Parse(values["ID сотрудника"])), ("@s", values["Серийный номер"]), ("@t", values["Тип подписи"]), ("@a", values["УЦ"]), ("@vf", values["Действует с"]), ("@vt", values["Действует до"]), ("@st", status), ("@p", values["Назначение"]));
        Database.Log("создание сертификата", "certificate", values["Серийный номер"]); LoadCertificates(); LoadAudit();
    }

    private void EditCertificate()
    {
        if (!EnsureEdit()) return; var id = SelectedId(gridCertificates); if (id == 0) return; var row = Database.Query("SELECT * FROM certificates WHERE id=@id", ("@id", id)).Rows[0];
        var values = ShowEditor("Сертификат", new Dictionary<string, string> { ["ID сотрудника"] = row["employee_id"].ToString()!, ["Серийный номер"] = row["serial_number"].ToString()!, ["Тип подписи"] = row["signature_type"].ToString()!, ["УЦ"] = row["authority"].ToString()!, ["Действует с"] = row["valid_from"].ToString()!, ["Действует до"] = row["valid_to"].ToString()!, ["Назначение"] = row["purpose"].ToString()! });
        if (values == null) return; var status = StatusText.CertificateStatus(DateTime.Parse(values["Действует до"]), row["status"].ToString()!);
        Database.Execute("UPDATE certificates SET employee_id=@e,serial_number=@s,signature_type=@t,authority=@a,valid_from=@vf,valid_to=@vt,status=@st,purpose=@p WHERE id=@id", ("@e", int.Parse(values["ID сотрудника"])), ("@s", values["Серийный номер"]), ("@t", values["Тип подписи"]), ("@a", values["УЦ"]), ("@vf", values["Действует с"]), ("@vt", values["Действует до"]), ("@st", status), ("@p", values["Назначение"]), ("@id", id));
        Database.Log("изменение сертификата", "certificate", values["Серийный номер"]); LoadCertificates(); LoadAudit();
    }

    private void SetCertificateStatus(string status) { if (!EnsureEdit()) return; var id = SelectedId(gridCertificates); if (id == 0) return; Database.Execute("UPDATE certificates SET status=@s WHERE id=@id", ("@s", status), ("@id", id)); Database.Log(status == "revoked" ? "отзыв сертификата" : "архивирование сертификата", "certificate", $"ID {id}"); LoadCertificates(); LoadAudit(); }

    private void AddMchd()
    {
        if (!EnsureEdit()) return; var values = ShowEditor("МЧД", new Dictionary<string, string> { ["Номер"] = "", ["Доверитель"] = "Западный филиал РАНХиГС", ["ID представителя"] = "1", ["Полномочия"] = "", ["Действует с"] = DateTime.Today.ToString("yyyy-MM-dd"), ["Действует до"] = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd"), ["Зарегистрирована 1/0"] = "1" }); if (values == null) return;
        var status = StatusText.CertificateStatus(DateTime.Parse(values["Действует до"]), "active"); Database.Execute("INSERT INTO mchd(number,principal,representative_id,powers,valid_from,valid_to,is_registered,status) VALUES(@n,@p,@r,@pow,@vf,@vt,@reg,@st)", ("@n", values["Номер"]), ("@p", values["Доверитель"]), ("@r", int.Parse(values["ID представителя"])), ("@pow", values["Полномочия"]), ("@vf", values["Действует с"]), ("@vt", values["Действует до"]), ("@reg", int.Parse(values["Зарегистрирована 1/0"])), ("@st", status)); Database.Log("создание МЧД", "mchd", values["Номер"]); LoadMchd(); LoadAudit();
    }

    private void EditMchd()
    {
        if (!EnsureEdit()) return; var id = SelectedId(gridMchd); if (id == 0) return; var row = Database.Query("SELECT * FROM mchd WHERE id=@id", ("@id", id)).Rows[0];
        var values = ShowEditor("МЧД", new Dictionary<string, string> { ["Номер"] = row["number"].ToString()!, ["Доверитель"] = row["principal"].ToString()!, ["ID представителя"] = row["representative_id"].ToString()!, ["Полномочия"] = row["powers"].ToString()!, ["Действует с"] = row["valid_from"].ToString()!, ["Действует до"] = row["valid_to"].ToString()!, ["Зарегистрирована 1/0"] = row["is_registered"].ToString()! }); if (values == null) return;
        var status = StatusText.CertificateStatus(DateTime.Parse(values["Действует до"]), row["status"].ToString()!); Database.Execute("UPDATE mchd SET number=@n,principal=@p,representative_id=@r,powers=@pow,valid_from=@vf,valid_to=@vt,is_registered=@reg,status=@st WHERE id=@id", ("@n", values["Номер"]), ("@p", values["Доверитель"]), ("@r", int.Parse(values["ID представителя"])), ("@pow", values["Полномочия"]), ("@vf", values["Действует с"]), ("@vt", values["Действует до"]), ("@reg", int.Parse(values["Зарегистрирована 1/0"])), ("@st", status), ("@id", id)); Database.Log("изменение МЧД", "mchd", values["Номер"]); LoadMchd(); LoadAudit();
    }

    private void SetMchdStatus(string status) { if (!EnsureEdit()) return; var id = SelectedId(gridMchd); if (id == 0) return; Database.Execute("UPDATE mchd SET status=@s WHERE id=@id", ("@s", status), ("@id", id)); Database.Log(status == "revoked" ? "отзыв МЧД" : "архивирование МЧД", "mchd", $"ID {id}"); LoadMchd(); LoadAudit(); }

    private void AddToken()
    {
        if (!EnsureEdit()) return; var values = ShowEditor("Токен", new Dictionary<string, string> { ["Инвентарный номер"] = "", ["Тип"] = "USB-токен", ["Модель"] = "", ["Серийный номер"] = "", ["Статус"] = "storage", ["ID держателя"] = "" }); if (values == null) return;
        Database.Execute("INSERT INTO tokens(inventory_number,token_type,model,serial_number,status,holder_id) VALUES(@i,@t,@m,@s,@st,@h)", ("@i", values["Инвентарный номер"]), ("@t", values["Тип"]), ("@m", values["Модель"]), ("@s", values["Серийный номер"]), ("@st", values["Статус"]), ("@h", string.IsNullOrWhiteSpace(values["ID держателя"]) ? null : (int?)int.Parse(values["ID держателя"]))); Database.Log("создание токена", "token", values["Инвентарный номер"]); LoadTokens(); LoadAudit();
    }

    private void EditToken()
    {
        if (!EnsureEdit()) return; var id = SelectedId(gridTokens); if (id == 0) return; var row = Database.Query("SELECT * FROM tokens WHERE id=@id", ("@id", id)).Rows[0];
        var values = ShowEditor("Токен", new Dictionary<string, string> { ["Инвентарный номер"] = row["inventory_number"].ToString()!, ["Тип"] = row["token_type"].ToString()!, ["Модель"] = row["model"].ToString()!, ["Серийный номер"] = row["serial_number"].ToString()!, ["Статус"] = row["status"].ToString()!, ["ID держателя"] = row["holder_id"].ToString()! }); if (values == null) return;
        Database.Execute("UPDATE tokens SET inventory_number=@i,token_type=@t,model=@m,serial_number=@s,status=@st,holder_id=@h WHERE id=@id", ("@i", values["Инвентарный номер"]), ("@t", values["Тип"]), ("@m", values["Модель"]), ("@s", values["Серийный номер"]), ("@st", values["Статус"]), ("@h", string.IsNullOrWhiteSpace(values["ID держателя"]) ? null : (int?)int.Parse(values["ID держателя"])), ("@id", id)); Database.Log("изменение токена", "token", values["Инвентарный номер"]); LoadTokens(); LoadAudit();
    }

    private void IssueToken()
    {
        if (!EnsureEdit()) return; var id = SelectedId(gridTokens); if (id == 0) return; var values = ShowEditor("Выдать токен", new Dictionary<string, string> { ["ID сотрудника"] = "1", ["Номер акта"] = "", ["Комментарий"] = "" }); if (values == null) return;
        Database.Execute("UPDATE tokens SET status='issued', holder_id=@e WHERE id=@id", ("@e", int.Parse(values["ID сотрудника"])), ("@id", id)); Database.Execute("INSERT INTO token_operations(token_id,employee_id,operation,act_number,comment,created_at) VALUES(@t,@e,'issue',@a,@c,@dt)", ("@t", id), ("@e", int.Parse(values["ID сотрудника"])), ("@a", values["Номер акта"]), ("@c", values["Комментарий"]), ("@dt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))); Database.Log("выдача токена", "token", $"ID {id}"); LoadTokens(); LoadAudit();
    }

    private void ReturnToken() { if (!EnsureEdit()) return; var id = SelectedId(gridTokens); if (id == 0) return; Database.Execute("UPDATE tokens SET status='storage', holder_id=NULL WHERE id=@id", ("@id", id)); Database.Execute("INSERT INTO token_operations(token_id,operation,comment,created_at) VALUES(@t,'return','Возврат токена',@dt)", ("@t", id), ("@dt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))); Database.Log("возврат токена", "token", $"ID {id}"); LoadTokens(); LoadAudit(); }
    private void WriteOffToken() { if (!EnsureEdit()) return; var id = SelectedId(gridTokens); if (id == 0) return; Database.Execute("UPDATE tokens SET status='written_off', holder_id=NULL WHERE id=@id", ("@id", id)); Database.Execute("INSERT INTO token_operations(token_id,operation,comment,created_at) VALUES(@t,'write_off','Списание токена',@dt)", ("@t", id), ("@dt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))); Database.Log("списание токена", "token", $"ID {id}"); LoadTokens(); LoadAudit(); }

    private void ExportReport(bool xlsx)
    {
        var table = ReportExporter.BuildReport(cmbReportType.Text);
        var path = xlsx ? ReportExporter.ExportXlsx(cmbReportType.Text, table) : ReportExporter.ExportCsv(cmbReportType.Text, table);
        lblReportPath.Text = path;
        Database.Log("экспорт отчета", "report", cmbReportType.Text);
        LoadAudit();
    }

    private void ClearAudit()
    {
        if (!AppRoles.CanClearAudit(CurrentRole)) return;
        Database.Execute("DELETE FROM audit_log");
        Database.Log("очистка аудита", "audit", "Журнал аудита очищен");
        LoadAudit();
    }

    private static Dictionary<string, string>? ShowEditor(string title, Dictionary<string, string> fields)
    {
        using var form = new Form { Text = title, StartPosition = FormStartPosition.CenterParent, Width = 430, Height = Math.Max(220, fields.Count * 42 + 110), FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false };
        var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = fields.Count + 1, AutoScroll = true };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        var boxes = new Dictionary<string, TextBox>();
        var row = 0;
        foreach (var field in fields)
        {
            table.Controls.Add(new Label { Text = field.Key, AutoSize = true, Margin = new Padding(4, 8, 4, 4) }, 0, row);
            var box = new TextBox { Text = field.Value, Dock = DockStyle.Fill, Margin = new Padding(4) };
            boxes[field.Key] = box; table.Controls.Add(box, 1, row); row++;
        }
        var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 90 };
        var cancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Width = 90 };
        var buttons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
        buttons.Controls.Add(cancel); buttons.Controls.Add(ok); table.Controls.Add(buttons, 1, row);
        form.Controls.Add(table); form.AcceptButton = ok; form.CancelButton = cancel;
        return form.ShowDialog() == DialogResult.OK ? boxes.ToDictionary(x => x.Key, x => x.Value.Text) : null;
    }
}
