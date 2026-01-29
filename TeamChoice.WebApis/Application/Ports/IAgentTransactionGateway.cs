namespace TeamChoice.WebApis.Application.Ports;

public interface IAgentTransactionGateway
{
    Task<string?> ResolveServiceTypeAsync(string bankServiceCode, CancellationToken cancellationToken);
}