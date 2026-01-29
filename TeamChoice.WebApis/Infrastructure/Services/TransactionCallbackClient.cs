using TeamChoice.WebApis.Application.Ports;
using TeamChoice.WebApis.Domain.Models.Transactions;
using TeamChoice.WebApis.Infrastructure.Transport;

public sealed class TransactionCallbackClient : ITransactionCallbackClient
{
    private readonly TransactionCallbackMapper _mapper;

    public TransactionCallbackClient(TransactionCallbackMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task NotifyAsync(
        Transaction transaction,
        ForwardingResult forwardingResult,
        CancellationToken cancellationToken)
    {
        var payload = _mapper.Map(transaction, forwardingResult);
        // send payload
    }
}
