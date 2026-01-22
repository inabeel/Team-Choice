using System;
using TeamChoice.WebApis.Models.DTOs.Transactions;
using TeamChoice.WebApis.Domain.Processors;

namespace TeamChoice.WebApis.Domain.Services;

public interface ITransactionProcessor
{
    PartnerTransaction BuildPartnerTransaction(TransactionRequestDto request);
}

public sealed class TransactionProcessor : ITransactionProcessor
{
    private const string StatusPending = "PENDING";

    public PartnerTransaction BuildPartnerTransaction(TransactionRequestDto request)
    {
        // Java parity: hard-coded date (timestamp usage intentionally skipped)
        var date = "2025-10-17T02:44:00Z";

        return new PartnerTransaction
        {
            TransactionDate = date,
            PartnerReference = request.PartnerReference,
            PartnerCode = request.SendingLocation?.LocationCode,
            Status = StatusPending,
            Payload = request.ToString()
        };
    }
}
