using System.Data;
using System.Data.SqlClient;

namespace TeamChoice.WebApis.Application.Services
{

    public interface IDatabaseService
    {
        Task<T> QueryOneAsync<T>(string sql, Dictionary<string, object> parameters, Func<IDataReader, T> mapper);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object> parameters, Func<IDataReader, T> mapper);
        Task<long> ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters);
        Task<T> ExecuteStoredProcedureAsync<T>(string procedureName, Dictionary<string, object> parameters, Func<IDataReader, T> mapper);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<T> QueryOneAsync<T>(string sql, Dictionary<string, object> parameters, Func<IDataReader, T> mapper)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                AddParameters(command, parameters);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        return mapper(reader);
                    }
                }
            }
            return default;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object> parameters, Func<IDataReader, T> mapper)
        {
            var list = new List<T>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                AddParameters(command, parameters);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(mapper(reader));
                    }
                }
            }
            return list;
        }

        public async Task<long> ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                AddParameters(command, parameters);
                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<T> ExecuteStoredProcedureAsync<T>(string procedureName, Dictionary<string, object> parameters, Func<IDataReader, T> mapper)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                AddParameters(command, parameters);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (await reader.ReadAsync())
                    {
                        return mapper(reader);
                    }
                }
            }
            return default;
        }

        private void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
        }
    }
}


