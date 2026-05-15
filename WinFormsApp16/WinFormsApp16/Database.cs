using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;

namespace WinFormsApp16;

public static class Database
{
    private static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certdesk.db");
    private static string ConnectionString => new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();

    public static void Initialize()
    {
        var firstRun = !File.Exists(DbPath);
        using var connection = OpenConnection();
        ExecuteScript(connection, SchemaSql);
        if (firstRun || Convert.ToInt32(Scalar("SELECT COUNT(*) FROM employees")) == 0)
            SeedDemoData();
        RefreshStatuses();
    }

    public static SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON";
        command.ExecuteNonQuery();
        return connection;
    }

    public static DataTable Query(string sql, params (string Name, object? Value)[] parameters)
    {
        using var connection = OpenConnection();
        using var command = CreateCommand(connection, sql, parameters);
        var table = new DataTable();
        table.Load(command.ExecuteReader());
        return table;
    }

    public static int Execute(string sql, params (string Name, object? Value)[] parameters)
    {
        using var connection = OpenConnection();
        using var command = CreateCommand(connection, sql, parameters);
        return command.ExecuteNonQuery();
    }

    public static object? Scalar(string sql, params (string Name, object? Value)[] parameters)
    {
        using var connection = OpenConnection();
        using var command = CreateCommand(connection, sql, parameters);
        return command.ExecuteScalar();
    }

    public static void Log(string action, string entity, string description) =>
        Execute("INSERT INTO audit_log(action, entity, description, created_at) VALUES(@a,@e,@d,@t)",
            ("@a", action), ("@e", entity), ("@d", description), ("@t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

    public static void RefreshStatuses()
    {
        Execute("UPDATE certificates SET status = CASE WHEN date(valid_to) < date('now') THEN 'expired' WHEN julianday(valid_to)-julianday('now','start of day') <= 30 THEN 'warning' ELSE 'active' END WHERE status NOT IN ('revoked','archived')");
        Execute("UPDATE mchd SET status = CASE WHEN date(valid_to) < date('now') THEN 'expired' WHEN julianday(valid_to)-julianday('now','start of day') <= 30 THEN 'warning' ELSE 'active' END WHERE status NOT IN ('revoked','archived')");
    }

    public static SqliteCommand CreateCommand(SqliteConnection connection, string sql, params (string Name, object? Value)[] parameters)
    {
        var command = connection.CreateCommand();
        command.CommandText = sql;
        foreach (var parameter in parameters)
            command.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
        return command;
    }

    private static void ExecuteScript(SqliteConnection connection, string script)
    {
        using var command = connection.CreateCommand();
        command.CommandText = script;
        command.ExecuteNonQuery();
    }

    private static void SeedDemoData()
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();
        try
        {
            void Exec(string sql, params (string Name, object? Value)[] parameters)
            {
                using var command = CreateCommand(connection, sql, parameters);
                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }

            string[] names =
            [
                "Иванова Мария Сергеевна", "Петров Алексей Николаевич", "Смирнова Ольга Викторовна", "Кузнецов Дмитрий Андреевич", "Попова Елена Михайловна",
                "Васильев Сергей Павлович", "Соколова Анна Игоревна", "Михайлов Кирилл Олегович", "Новикова Дарья Романовна", "Герасимов Кирилл Александрович"
            ];
            string[] departments = ["Учебный отдел", "Бухгалтерия", "ИТ-отдел", "Кадровая служба", "Юридический отдел"];
            for (int i = 0; i < names.Length; i++)
            {
                Exec("INSERT INTO employees(full_name,position,department,email,phone,is_active) VALUES(@n,@p,@d,@e,@ph,1)",
                    ("@n", names[i]), ("@p", i % 3 == 0 ? "Начальник отдела" : "Специалист"), ("@d", departments[i % departments.Length]),
                    ("@e", $"user{i + 1}@ranepa.local"), ("@ph", $"+7 (4012) 55-{10 + i:00}-{20 + i:00}"));
            }

            for (int i = 1; i <= 15; i++)
            {
                var validTo = i <= 8 ? DateTime.Today.AddDays(80 + i) : i <= 12 ? DateTime.Today.AddDays(5 + i) : DateTime.Today.AddDays(-i);
                var status = i == 15 ? "revoked" : StatusText.CertificateStatus(validTo, "active");
                Exec("INSERT INTO certificates(employee_id,serial_number,signature_type,authority,valid_from,valid_to,status,purpose) VALUES(@e,@s,@t,@a,@vf,@vt,@st,@p)",
                    ("@e", (i % 10) + 1), ("@s", $"CERT-2026-{i:0000}"), ("@t", i % 3 == 0 ? "SES" : i % 3 == 1 ? "NES" : "QES"),
                    ("@a", i % 2 == 0 ? "УЦ Федерального казначейства" : "Контур Удостоверяющий центр"),
                    ("@vf", DateTime.Today.AddMonths(-6).ToString("yyyy-MM-dd")), ("@vt", validTo.ToString("yyyy-MM-dd")), ("@st", status),
                    ("@p", "Подписание документов образовательной организации"));
            }

            for (int i = 1; i <= 6; i++)
            {
                var validTo = i <= 3 ? DateTime.Today.AddDays(90 + i) : i <= 5 ? DateTime.Today.AddDays(12 + i) : DateTime.Today.AddDays(-3);
                Exec("INSERT INTO mchd(number,principal,representative_id,powers,valid_from,valid_to,is_registered,status) VALUES(@n,@p,@r,@pow,@vf,@vt,@reg,@st)",
                    ("@n", $"МЧД-39-2026-{i:000}"), ("@p", "Западный филиал РАНХиГС"), ("@r", i + 1),
                    ("@pow", "Представление интересов и подписание документов"), ("@vf", DateTime.Today.AddMonths(-2).ToString("yyyy-MM-dd")),
                    ("@vt", validTo.ToString("yyyy-MM-dd")), ("@reg", i % 2), ("@st", StatusText.CertificateStatus(validTo, "active")));
            }

            for (int i = 1; i <= 8; i++)
            {
                var status = i <= 4 ? "issued" : i <= 6 ? "storage" : i == 7 ? "damaged" : "written_off";
                Exec("INSERT INTO tokens(inventory_number,token_type,model,serial_number,status,holder_id) VALUES(@inv,@type,@model,@serial,@status,@holder)",
                    ("@inv", $"ЗФ-ТК-{i:000}"), ("@type", "USB-токен"), ("@model", i % 2 == 0 ? "Рутокен ЭЦП" : "JaCarta ГОСТ"),
                    ("@serial", $"TK2026{i:0000}"), ("@status", status), ("@holder", status == "issued" ? (int?)i : null));
            }

            for (int i = 1; i <= 4; i++)
                Exec("INSERT INTO token_operations(token_id,employee_id,operation,act_number,comment,created_at) VALUES(@t,@e,'issue',@a,@c,@dt)",
                    ("@t", i), ("@e", i), ("@a", $"АКТ-{i:000}"), ("@c", "Демонстрационная выдача токена"), ("@dt", DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm:ss")));

            for (int i = 1; i <= 8; i++)
                Exec("INSERT INTO audit_log(action,entity,description,created_at) VALUES(@a,@e,@d,@dt)",
                    ("@a", i % 2 == 0 ? "создание записи" : "выдача токена"), ("@e", i % 2 == 0 ? "certificate" : "token"),
                    ("@d", "Демонстрационное событие аудита"), ("@dt", DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm:ss")));

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private const string SchemaSql = """
CREATE TABLE IF NOT EXISTS employees (
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 full_name TEXT NOT NULL,
 position TEXT,
 department TEXT,
 email TEXT,
 phone TEXT,
 is_active INTEGER DEFAULT 1
);
CREATE TABLE IF NOT EXISTS certificates (
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 employee_id INTEGER,
 serial_number TEXT NOT NULL UNIQUE,
 signature_type TEXT,
 authority TEXT,
 valid_from TEXT,
 valid_to TEXT,
 status TEXT,
 purpose TEXT
);
CREATE TABLE IF NOT EXISTS mchd (
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 number TEXT NOT NULL UNIQUE,
 principal TEXT,
 representative_id INTEGER,
 powers TEXT,
 valid_from TEXT,
 valid_to TEXT,
 is_registered INTEGER DEFAULT 0,
 status TEXT
);
CREATE TABLE IF NOT EXISTS tokens (
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 inventory_number TEXT NOT NULL UNIQUE,
 token_type TEXT,
 model TEXT,
 serial_number TEXT,
 status TEXT,
 holder_id INTEGER
);
CREATE TABLE IF NOT EXISTS token_operations (
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 token_id INTEGER,
 employee_id INTEGER,
 operation TEXT,
 act_number TEXT,
 comment TEXT,
 created_at TEXT
);
CREATE TABLE IF NOT EXISTS audit_log (
 id INTEGER PRIMARY KEY AUTOINCREMENT,
 action TEXT,
 entity TEXT,
 description TEXT,
 created_at TEXT
);
""";
}
