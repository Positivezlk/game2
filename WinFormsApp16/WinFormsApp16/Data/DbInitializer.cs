using Microsoft.Data.Sqlite;

namespace CertDesk.Data;

public static class DbInitializer
{
    public static void EnsureCreated()
    {
        var firstRun = !File.Exists(Db.DatabasePath);
        Directory.CreateDirectory(AppContext.BaseDirectory);
        using var connection = Db.OpenConnection();
        using (var command = connection.CreateCommand()) { command.CommandText = Schema.Sql; command.ExecuteNonQuery(); }
        if (firstRun || IsEmpty(connection)) Seed(connection);
    }

    private static bool IsEmpty(SqliteConnection connection)
    {
        using var command = Db.Command(connection, "SELECT COUNT(*) FROM users");
        return Convert.ToInt32(command.ExecuteScalar()) == 0;
    }

    private static void Seed(SqliteConnection connection)
    {
        using var tx = connection.BeginTransaction();
        try
        {
            string[] depts = ["Учебный отдел", "Кадровая служба", "Бухгалтерия", "ИТ-отдел", "Юридический отдел", "Дирекция"];
            string[] names = ["Иванова Мария Сергеевна", "Петров Алексей Николаевич", "Смирнова Ольга Викторовна", "Кузнецов Дмитрий Андреевич", "Попова Елена Михайловна", "Васильев Сергей Павлович", "Соколова Анна Игоревна", "Михайлов Кирилл Олегович", "Новикова Дарья Романовна", "Федоров Максим Владимирович", "Морозова Наталья Евгеньевна", "Волков Артем Денисович", "Алексеева Татьяна Юрьевна", "Лебедев Роман Александрович", "Семенова Ирина Петровна", "Егорова Светлана Олеговна", "Павлов Илья Сергеевич", "Козлова Алина Дмитриевна", "Степанов Павел Игоревич", "Николаева Вероника Андреевна", "Орлов Андрей Валерьевич", "Макарова Ксения Павловна", "Зайцев Денис Кириллович", "Соловьева Екатерина Денисовна", "Герасимов Кирилл Александрович"];
            for (int i = 0; i < names.Length; i++)
                Exec(connection, tx, "INSERT INTO employees(full_name,position,department,email,phone,snils,inn,is_active) VALUES(@n,@p,@d,@e,@ph,@s,@inn,@a)", ("@n", names[i]), ("@p", i % 5 == 0 ? "Начальник отдела" : "Специалист"), ("@d", depts[i % depts.Length]), ("@e", $"user{i + 1}@ranepa.local"), ("@ph", $"+7 (4012) 55-{10 + i:00}-{20 + i:00}"), ("@s", $"{100 + i:000}-{200 + i:000}-{300 + i:000} {10 + i:00}"), ("@inn", $"3906{100000 + i}"), ("@a", i > 22 ? 0 : 1));
            string[,] auth = { { "УЦ Федерального казначейства", "7710568760", "АК-001", "https://roskazna.gov.ru" }, { "Контур Удостоверяющий центр", "6663003127", "АК-143", "https://ca.kontur.ru" }, { "Тензор УЦ", "7605016030", "АК-212", "https://tensor.ru" } };
            for (int i = 0; i < 3; i++) Exec(connection, tx, "INSERT INTO authorities(name,inn,accreditation_number,website) VALUES(@n,@i,@a,@w)", ("@n", auth[i,0]), ("@i", auth[i,1]), ("@a", auth[i,2]), ("@w", auth[i,3]));
            for (int i = 1; i <= 18; i++)
            {
                var status = i <= 9 ? "issued" : i <= 15 ? "storage" : i == 16 ? "damaged" : "written_off";
                Exec(connection, tx, "INSERT INTO tokens(inventory_number,token_type,model,serial_number,status,holder_id,received_at,notes) VALUES(@inv,@t,@m,@s,@st,@h,@r,@n)", ("@inv", $"ЗФ-ТК-{i:000}"), ("@t", "USB-токен"), ("@m", i % 2 == 0 ? "Рутокен ЭЦП 3.0" : "JaCarta ГОСТ"), ("@s", $"TK2026{i:0000}"), ("@st", status), ("@h", status == "issued" ? (int?)i : null), ("@r", DateTime.Today.AddDays(-120 - i).ToString("yyyy-MM-dd")), ("@n", "Демонстрационная запись"));
            }
            for (int i = 1; i <= 34; i++)
            {
                var to = i <= 18 ? DateTime.Today.AddDays(90 + i) : i <= 26 ? DateTime.Today.AddDays(i - 17) : i <= 31 ? DateTime.Today.AddDays(-i) : DateTime.Today.AddDays(180);
                var status = i > 31 ? "revoked" : StatusByDate(to);
                Exec(connection, tx, "INSERT INTO certificates(employee_id,authority_id,token_id,serial_number,signature_type,issued_at,valid_from,valid_to,status,purpose,is_archived) VALUES(@e,@a,@t,@s,@sg,@ia,@vf,@vt,@st,@p,0)", ("@e", (i % 25) + 1), ("@a", (i % 3) + 1), ("@t", i <= 18 ? (int?)i : null), ("@s", $"CERT-2026-{i:00000}"), ("@sg", i % 3 == 0 ? "SES" : i % 3 == 1 ? "NES" : "QES"), ("@ia", DateTime.Today.AddMonths(-6).ToString("yyyy-MM-dd")), ("@vf", DateTime.Today.AddMonths(-6).ToString("yyyy-MM-dd")), ("@vt", to.ToString("yyyy-MM-dd")), ("@st", status), ("@p", "Подписание документов образовательной организации"));
            }
            for (int i = 1; i <= 12; i++)
            {
                var to = i <= 6 ? DateTime.Today.AddDays(60 + i) : i <= 9 ? DateTime.Today.AddDays(5 + i) : DateTime.Today.AddDays(-i);
                Exec(connection, tx, "INSERT INTO mchd(number,principal_employee_id,representative_employee_id,certificate_id,powers,powers_codes,valid_from,valid_to,is_registered,status,notes) VALUES(@n,@p,@r,@c,@pw,@pc,@vf,@vt,@reg,@st,@note)", ("@n", $"МЧД-39-2026-{i:0000}"), ("@p", 1), ("@r", i + 2), ("@c", i), ("@pw", "Представление интересов, подписание заявлений и получение документов"), ("@pc", "EDU.SIGN;DOC.RECEIVE"), ("@vf", DateTime.Today.AddMonths(-2).ToString("yyyy-MM-dd")), ("@vt", to.ToString("yyyy-MM-dd")), ("@reg", i % 2), ("@st", StatusByDate(to)), ("@note", "Демонстрационная МЧД"));
            }
            AddUser(connection, tx, "admin", "admin123", "administrator"); AddUser(connection, tx, "spec", "spec123", "specialist"); AddUser(connection, tx, "view", "view123", "viewer");
            for (int i = 1; i <= 60; i++) Exec(connection, tx, "INSERT INTO audit_log(user_id,login,action,entity_type,entity_id,description,created_at) VALUES(1,'admin',@a,@e,@id,@d,@dt)", ("@a", i % 3 == 0 ? "экспорт отчета" : i % 3 == 1 ? "изменение сертификата" : "выдача токена"), ("@e", i % 2 == 0 ? "certificate" : "token"), ("@id", (i % 20) + 1), ("@d", "Демонстрационное событие аудита"), ("@dt", DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm:ss")));
            tx.Commit();
        }
        catch { tx.Rollback(); throw; }
    }

    private static void AddUser(SqliteConnection c, SqliteTransaction tx, string login, string password, string role) =>
        Exec(c, tx, "INSERT INTO users(login,password_hash,role,is_active) VALUES(@l,@p,@r,1)", ("@l", login), ("@p", BCrypt.Net.BCrypt.HashPassword(password)), ("@r", role));
    private static string StatusByDate(DateTime to) => to.Date < DateTime.Today ? "expired" : (to.Date - DateTime.Today).TotalDays <= 30 ? "warning" : "active";
    private static void Exec(SqliteConnection c, SqliteTransaction tx, string sql, params (string Name, object? Value)[] p) { using var cmd = Db.Command(c, tx, sql, p); cmd.ExecuteNonQuery(); }
}
