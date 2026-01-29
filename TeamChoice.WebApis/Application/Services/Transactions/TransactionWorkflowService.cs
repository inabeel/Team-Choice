using TeamChoice.WebApis.Application.Commands.Transactions;
using TeamChoice.WebApis.Application.Mappers;
using TeamChoice.WebApis.Application.Ports;
using TeamChoice.WebApis.Domain.Exceptions;
using TeamChoice.WebApis.Domain.Models.Transactions;
using TeamChoice.WebApis.Domain.Services.Transactions;

namespace TeamChoice.WebApis.Application.Services;

public sealed class TransactionWorkflowService
{
    private readonly TransactionValidator _validator;
    private readonly TransactionFactory _transactionFactory;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IServiceCodeResolver _serviceCodeResolver;
    private readonly ITransactionForwarder _transactionForwarder;
    private readonly ITransactionCallbackClient _callbackClient;
    private readonly IRemittanceService _remittanceService;

    public TransactionWorkflowService(
        TransactionValidator validator,
        TransactionFactory transactionFactory,
        ITransactionRepository transactionRepository,
        IServiceCodeResolver serviceCodeResolver,
        ITransactionForwarder transactionForwarder,
        ITransactionCallbackClient callbackClient,
        IRemittanceService remittanceService)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _transactionFactory = transactionFactory ?? throw new ArgumentNullException(nameof(transactionFactory));
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        _serviceCodeResolver = serviceCodeResolver ?? throw new ArgumentNullException(nameof(serviceCodeResolver));
        _transactionForwarder = transactionForwarder ?? throw new ArgumentNullException(nameof(transactionForwarder));
        _callbackClient = callbackClient ?? throw new ArgumentNullException(nameof(callbackClient));
        _remittanceService = remittanceService;
    }

    public async Task<TransactionResult> ExecuteAsync(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        // 1. Validate input + business rules
        _validator.ValidateCreate(command);

        // 2. Ensure uniqueness
        if (await _transactionRepository.ExistsByPartnerReferenceAsync(command.PartnerReference, cancellationToken))
        {
            throw new DuplicateTransactionException(command.PartnerReference);
        }

        // 3. Resolve service type
        var serviceType = await _serviceCodeResolver.ResolveAsync(command.ServiceCode, cancellationToken);

        // 4. Create domain transaction
        var transaction = _transactionFactory.Create(command, serviceType);

        // 5. Persist
        await _transactionRepository.SaveAsync(transaction, cancellationToken);

        // 6. Forward transaction
        ForwardingResult forwardingResult;
        try
        {
            forwardingResult = await _transactionForwarder.ForwardAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new TransactionForwardingException(transaction.Reference, ex);
        }

        // 7. Callback (best effort, but explicit)
        await _callbackClient.NotifyAsync(
            transaction,
            forwardingResult,
            cancellationToken);

        // 8. Return result
        return TransactionResult.Pending(
            transaction.Reference);
    }

    public async Task<string> ValidateStatusAsync(ValidateTransactionStatusCommand command, CancellationToken cancellationToken)
    {
        var transactionStatus = await _transactionRepository
            .ValidateTransactionStatusAsync(command.TransactionReference, cancellationToken)
            ?? throw new TransactionNotFoundException(command.TransactionReference);

        return transactionStatus;
    }

    public async Task<TransactionResult> CancelAsync(CancelTransactionCommand command, CancellationToken cancellationToken)
    {
        var transactionStatus = await _transactionRepository.ValidateTransactionStatusAsync(command.TransactionReference, cancellationToken);

        if (string.IsNullOrWhiteSpace(transactionStatus))
            throw new TransactionNotFoundException(command.TransactionReference);

        // Java logic: only REM / READY / PENDING is cancellable
        if (!transactionStatus.Equals("READY", StringComparison.OrdinalIgnoreCase) &&
            !transactionStatus.Equals("REM", StringComparison.OrdinalIgnoreCase))
        {
            throw new AlreadyPaidException(command.TransactionReference);
        }

        var cancelRequest = CancelRequestMapper.Map(command);

        // 2. Execute cancellation stored procedure
        await _transactionRepository.CancelAsync(
            cancelRequest,
            cancellationToken);

        return TransactionResult.Cancelled(command.TransactionReference);
    }
}
