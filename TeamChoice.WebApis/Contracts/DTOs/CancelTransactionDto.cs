namespace TeamChoice.WebApis.Contracts.DTOs;

public sealed record CancelTransactionDto
{
    public string TawakalTxnRef { get; init; }
    public string LocationCode { get; init; }
    public string Reason { get; init; }
}
