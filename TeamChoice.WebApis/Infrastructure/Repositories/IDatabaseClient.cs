namespace TeamChoice.WebApis.Infrastructure.Repositories;

public interface IDatabaseClient
{
    Task<T> ExecuteStoredProcedureAsync<T>(string procedureName, Dictionary<string, object> parameters);

    Task<decimal> QueryOneAsync(string fIND_RECEIVE_COMMISSION_BY_AGENT_CODE, Dictionary<string, object> parameters, Func<object, decimal> value);
}