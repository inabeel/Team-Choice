using TeamChoice.WebApis.Application.Commands.Transactions;
using TeamChoice.WebApis.Application.Ports;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models.Enums;
using TeamChoice.WebApis.Domain.Models.Transactions;
using TeamChoice.WebApis.Utils;

namespace TeamChoice.WebApis.Infrastructure.Persistence;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly IDatabaseService _databaseService;

    public TransactionRepository(IDatabaseService databaseService)
    {
        _databaseService = databaseService
            ?? throw new ArgumentNullException(nameof(databaseService));
    }

    /// <summary>
    /// Checks whether a transaction already exists for the given partner reference.
    /// Java equivalent: agentTransactionFacade.validateTransaction(...)
    /// </summary>
    public async Task<bool> ExistsByPartnerReferenceAsync(
        string partnerReference,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(partnerReference);

        var parameters = new Dictionary<string, object>
        {
            { "@AgtRefNo", partnerReference }
        };

        var result = await _databaseService.QueryOneAsync(
            TransactionSql.VALIDATE_DUPLICATE_TRANSACTION,
            parameters,
            reader =>
            {
                var value = reader["Result"];
                return value != DBNull.Value && Convert.ToInt32(value) > 0;
            }, cancellationToken);

        return result;
    }

    /// <summary>
    /// Persists a new transaction.
    /// Mirrors Java behavior where transaction is saved before forwarding.
    /// </summary>
    public async Task SaveAsync(
        Transaction transaction,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        var parameters = new Dictionary<string, object>
        {
            { "@AgtRefNo", transaction.PartnerReference },
            { "@Reference", transaction.Reference },
            { "@Amount", transaction.Amount },
            { "@Currency", transaction.Currency },
            { "@ServiceType", transaction.ServiceType },
            { "@Status", transaction.Status.ToString() }
        };

        await _databaseService.ExecuteNonQueryAsync(
            TransactionSql.INSERT_TRANSACTION,
            parameters, cancellationToken);
    }

    /// <summary>
    /// Retrieves a transaction by its internal reference.
    /// </summary>
    public async Task<Transaction?> GetByReferenceAsync(string transactionReference, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(transactionReference);

        var parameters = new Dictionary<string, object> { { "@Reference", transactionReference } };

        return await _databaseService.QueryOneAsync(
            TransactionSql.GET_TRANSACTION_BY_REFERENCE,
            parameters,
            reader =>
            {
                var statusText = reader["Status"]?.ToString();

                var status = Enum.TryParse<TransactionStatus>(
                    statusText,
                    ignoreCase: true,
                    out var parsedStatus)
                        ? parsedStatus
                        : TransactionStatus.Pending;

                return Transaction.Rehydrate(
                    reference: reader["Reference"]!.ToString()!,
                    partnerReference: reader["AgtRefNo"]!.ToString()!,
                    amount: reader["Amount"] != DBNull.Value
                        ? Convert.ToDecimal(reader["Amount"])
                        : 0m,
                    currency: reader["Currency"]!.ToString()!,
                    serviceType: reader["ServiceType"]!.ToString()!,
                    status: status
                );
            },
            cancellationToken);
    }


    /// <summary>
    /// Updates transaction state (e.g. cancel / status change).
    /// </summary>
    public async Task UpdateAsync(
        Transaction transaction,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        var parameters = new Dictionary<string, object>
        {
            { "@Reference", transaction.Reference },
            { "@Status", transaction.Status.ToString() }
        };

        await _databaseService.ExecuteNonQueryAsync(
            TransactionSql.UPDATE_TRANSACTION_STATUS,
            parameters, cancellationToken);
    }

    public async Task<string> ValidateTransactionStatusAsync(
    string transactionCode,
    CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, object>
    {
        { "@trnsCode", transactionCode }
    };

        var result = await _databaseService.QueryOneAsync(
            TransactionSql.VALIDATE_TRANSACTION_STATUS,
            parameters,
            reader => reader["TrnsStatus"]?.ToString(),
            cancellationToken);

        // Java: switchIfEmpty(Mono.just("NOT_FOUND"))
        return string.IsNullOrWhiteSpace(result)
            ? "NOT_FOUND"
            : result;
    }

    public async Task CancelAsync(CancelReceiveRequest request, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, object>
        {
            ["@refno"] = request.Refno,
            ["@reason"] = request.Reason,
            ["@agtcode"] = request.Agtcode,
            ["@subcode"] = request.Subcode,
            ["@loccode"] = request.Loccode,
            ["@rqstuserid"] = request.Rqstuserid,
            ["@rqstdate"] = request.Rqstdate,
            ["@agtaprvduser"] = request.Agtaprvduser,
            ["@agtaprvddate"] = request.Agtaprvddate,
            ["@smtaprvduser"] = request.Smtaprvduser,
            ["@smtaprvddate"] = request.Smtaprvddate,
            ["@refundrate"] = request.Refundrate,
            ["@recagtcode"] = request.Recagtcode,
            ["@refundamt"] = request.Refundamt,
            ["@refundackflg"] = request.Refundackflg,
            ["@refundackuser"] = request.Refundackuser,
            ["@refundackdate"] = request.Refundackdate,
            ["@refundfrom"] = request.Refundfrom,
            ["@module"] = request.Module,
            ["@rateoption"] = request.Rateoption,
            ["@commoption"] = request.Commoption,
            ["@commdesc"] = request.Commdesc,
            ["@trnsstatus"] = request.Trnsstatus,
            ["@trnssubstatus"] = request.Trnssubstatus,
            ["@agenttype"] = request.Agenttype,
            ["@action"] = request.Action,
            ["@bmapruser"] = request.Bmapruser,
            ["@errorsource"] = request.Errorsource,
            ["@trnssndmode"] = request.Trnssndmode
        };

        await _databaseService.ExecuteStoredProcedureAsync(
            CancellationSql.CALL_PROCEDURE,
            parameters,
            _ => true,
            cancellationToken);
    }
}
