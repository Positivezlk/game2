using Microsoft.Data.Sqlite;

namespace CertDesk.Data;

public static class Db
{
    public static string DatabasePath => Path.Combine(AppContext.BaseDirectory, "DataFiles", "certdesk.db");
    public static string ConnectionString => new SqliteConnectionStringBuilder { DataSource = DatabasePath }.ToString();

    public static SqliteConnection OpenConnection()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath)!);
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var pragma = connection.CreateCommand();
        pragma.CommandText = "PRAGMA foreign_keys = ON;";
        pragma.ExecuteNonQuery();
        return connection;
    }

    public static SqliteCommand Command(SqliteConnection connection, string sql, params (string Name, object? Value)[] parameters)
    {
        var command = connection.CreateCommand();
        command.CommandText = sql;
        foreach (var parameter in parameters)
            command.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
        return command;
    }

    public static SqliteCommand Command(SqliteConnection connection, SqliteTransaction transaction, string sql, params (string Name, object? Value)[] parameters)
    {
        var command = Command(connection, sql, parameters);
        command.Transaction = transaction;
        return command;
    }
}
