using TeamChoice.WebApis.Contracts.DTOs.Transactions;
using TeamChoice.WebApis.Domain.Models.Enums;
using TeamChoice.WebApis.Domain.Models.Transactions;

namespace TeamChoice.WebApis.Application.Mappers;

public sealed class TransactionResultMapper
{
    public TransactionResultDto Map(TransactionResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new TransactionResultDto
        {
            TransactionReference = result.Reference,
            Status = MapStatus(result.Status)
        };
    }

    private static string MapStatus(TransactionStatus status)
    {
        // Explicit mapping to avoid leaking enum names blindly
        return status switch
        {
            TransactionStatus.Pending => "PENDING",
            TransactionStatus.Completed => "COMPLETED",
            TransactionStatus.Cancelled => "CANCELLED",
            TransactionStatus.Failed => "FAILED",
            _ => "UNKNOWN"
        };
    }
}
