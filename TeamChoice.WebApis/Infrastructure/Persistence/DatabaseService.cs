using System.Data;
using System.Data.SqlClient;

namespace TeamChoice.WebApis.Infrastructure.Persistence;

public interface IDatabaseService
{
    Task<T?> QueryOneAsync<T>(
        string sql,
        Dictionary<string, object> parameters,
        Func<IDataReader, T> mapper,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<T>> QueryAsync<T>(
        string sql,
        Dictionary<string, object> parameters,
        Func<IDataReader, T> mapper,
        CancellationToken cancellationToken);

    Task<int> ExecuteNonQueryAsync(
        string sql,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken);

    Task<T?> ExecuteStoredProcedureAsync<T>(
        string procedureName,
        Dictionary<string, object> parameters,
        Func<IDataReader, T> mapper,
        CancellationToken cancellationToken);
}

public sealed class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Missing connection string.");
    }

    public async Task<T?> QueryOneAsync<T>(
        string sql,
        Dictionary<string, object> parameters,
        Func<IDataReader, T> mapper,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);

        AddParameters(command, parameters);

        await connection.OpenAsync(cancellationToken);

        await using var reader =
            await command.ExecuteReaderAsync(
                CommandBehavior.SingleRow,
                cancellationToken);

        return await reader.ReadAsync(cancellationToken)
            ? mapper(reader)
            : default;
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(
        string sql,
        Dictionary<string, object> parameters,
        Func<IDataReader, T> mapper,
        CancellationToken cancellationToken)
    {
        var results = new List<T>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);

        AddParameters(command, parameters);

        await connection.OpenAsync(cancellationToken);

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(mapper(reader));
        }

        return results;
    }

    public async Task<int> ExecuteNonQueryAsync(
        string sql,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);

        AddParameters(command, parameters);

        await connection.OpenAsync(cancellationToken);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<T?> ExecuteStoredProcedureAsync<T>(
        string procedureName,
        Dictionary<string, object> parameters,
        Func<IDataReader, T> mapper,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(procedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        AddParameters(command, parameters);

        await connection.OpenAsync(cancellationToken);

        await using var reader =
            await command.ExecuteReaderAsync(
                CommandBehavior.SingleRow,
                cancellationToken);

        return await reader.ReadAsync(cancellationToken)
            ? mapper(reader)
            : default;
    }

    private static void AddParameters(
        SqlCommand command,
        Dictionary<string, object> parameters)
    {
        if (parameters is null)
        {
            return;
        }

        foreach (var (key, value) in parameters)
        {
            var parameter = command.Parameters.Add(
                key,
                SqlDbType.VarChar);

            parameter.Value = value ?? DBNull.Value;
        }
    }
}
