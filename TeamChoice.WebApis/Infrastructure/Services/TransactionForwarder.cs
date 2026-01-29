using TeamChoice.WebApis.Application.Ports;
using TeamChoice.WebApis.Domain.Exceptions;
using TeamChoice.WebApis.Domain.Models.Transactions;
using TeamChoice.WebApis.Infrastructure.Transport;

namespace TeamChoice.WebApis.Infrastructure.Services;

public sealed class TransactionForwarder : ITransactionForwarder
{
    private readonly HttpClient _httpClient;
    private readonly TransactionForwardingMapper _mapper;

    public TransactionForwarder(HttpClient httpClient, TransactionForwardingMapper mapper)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ForwardingResult> ForwardAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        var request = _mapper.Map(transaction);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("transactions", request, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new TransactionForwardingException(transaction.Reference, ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new TransactionForwardingException(transaction.Reference, $"Downstream system returned {(int)response.StatusCode}");
        }

        var payload = await response.Content
            .ReadFromJsonAsync<ForwardingResponseDto>(cancellationToken);

        if (payload is null || string.IsNullOrWhiteSpace(payload.TransactionId))
        {
            throw new TransactionForwardingException(transaction.Reference, "Invalid forwarding response received.");
        }

        return new ForwardingResult(
            ExternalTransactionId: payload.TransactionId,
            ForwardedAt: payload.Timestamp,
            Status: payload.Status);
    }
}
