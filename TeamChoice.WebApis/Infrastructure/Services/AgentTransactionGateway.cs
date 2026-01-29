using TeamChoice.WebApis.Application.Ports;
using TeamChoice.WebApis.Infrastructure.Persistence;
using TeamChoice.WebApis.Utils;

namespace TeamChoice.WebApis.Infrastructure.Services;

public sealed class AgentTransactionGateway : IAgentTransactionGateway
{
    private readonly IDatabaseService _databaseService;

    public AgentTransactionGateway(IDatabaseService databaseService)
    {
        _databaseService = databaseService
            ?? throw new ArgumentNullException(nameof(databaseService));
    }

    public async Task<string?> ResolveServiceTypeAsync(string serviceCode, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@ServiceCode", serviceCode }
        };

        // SAME stored procedure / query as Java
        return await _databaseService.QueryOneAsync(
            TransactionSql.RESOLVE_SERVICE_TYPE,
            parameters,
            reader => reader["ServiceType"]?.ToString(), CancellationToken.None
        );
    }
}
