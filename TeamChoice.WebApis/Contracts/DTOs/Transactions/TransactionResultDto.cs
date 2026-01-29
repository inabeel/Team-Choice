namespace TeamChoice.WebApis.Contracts.DTOs.Transactions;

public sealed class TransactionResultDto
{
    public string TransactionReference { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Message { get; set; }
}