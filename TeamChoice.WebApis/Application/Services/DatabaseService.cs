namespace TeamChoice.WebApis.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    namespace YourNamespace.Services
    {
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
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

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

            public async Task<long> ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters)
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

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

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

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
        }

        public interface IDatabaseService
        {
            Task<T> QueryOneAsync<T>(string sql, Dictionary<string, object> parameters, Func<IDataReader, T> mapper);
            Task<long> ExecuteNonQueryAsync(string sql, Dictionary<string, object> parameters);
            Task<T> ExecuteStoredProcedureAsync<T>(string procedureName, Dictionary<string, object> parameters, Func<IDataReader, T> mapper);
        }
    }
}
