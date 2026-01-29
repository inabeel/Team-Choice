using TeamChoice.WebApis.Application.Mappers;
using TeamChoice.WebApis.Application.Services;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;

namespace TeamChoice.WebApis.Application.Facades;

public interface ITransactionFacade
{
    Task<TransactionResultDto> CreateAsync(TransactionRequestDto request, CancellationToken cancellationToken = default);

    Task<TransactionStatusDto> ValidateStatusAsync(TransactionStatusRequestDto request, CancellationToken cancellationToken = default);

    Task<TransactionResultDto> CancelAsync(CancelTransactionRequest request, CancellationToken cancellationToken = default);
}

public sealed class TransactionFacade : ITransactionFacade
{
    private readonly TransactionWorkflowService _workflow;
    private readonly TransactionRequestMapper _requestMapper;
    private readonly TransactionResultMapper _resultMapper;

    public TransactionFacade(TransactionWorkflowService workflow, TransactionRequestMapper requestMapper, TransactionResultMapper resultMapper)
    {
        _workflow = workflow ?? throw new ArgumentNullException(nameof(workflow));
        _requestMapper = requestMapper ?? throw new ArgumentNullException(nameof(requestMapper));
        _resultMapper = resultMapper ?? throw new ArgumentNullException(nameof(resultMapper));
    }

    public async Task<TransactionResultDto> CreateAsync(TransactionRequestDto request, CancellationToken cancellationToken = default)
    {
        // Application boundary mapping
        var domainRequest = _requestMapper.Map(request);

        var domainResult = await _workflow.ExecuteAsync(domainRequest, cancellationToken);

        return _resultMapper.Map(domainResult);
    }

    public async Task<TransactionStatusDto> ValidateStatusAsync(TransactionStatusRequestDto request, CancellationToken cancellationToken = default)
    {
        var domainRequest = _requestMapper.MapStatus(request);

        var domainResult = await _workflow.ValidateStatusAsync(domainRequest, cancellationToken);

        var response = new TransactionStatusDto(domainResult, "Transaction status retrieved successfully");

        return response;
    }

    public async Task<TransactionResultDto> CancelAsync(CancelTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var domainRequest = _requestMapper.MapCancel(request);

        var domainResult = await _workflow.CancelAsync(domainRequest, cancellationToken);

        return _resultMapper.Map(domainResult);
    }
}
