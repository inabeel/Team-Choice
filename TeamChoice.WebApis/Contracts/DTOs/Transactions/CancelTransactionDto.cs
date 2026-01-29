using System.ComponentModel.DataAnnotations;

namespace TeamChoice.WebApis.Contracts.DTOs.Transactions;

/// <summary>
/// Request to cancel a pending transaction
/// </summary>
public sealed class CancelTransactionRequest
{
    [Required(ErrorMessage = "tawakalTxnRef is required")]
    public string TawakalTxnRef { get; set; } = default!;

    [Required(ErrorMessage = "locationCode is required")]
    public string LocationCode { get; set; } = default!;

    [Required(ErrorMessage = "reason is required")]
    public string Reason { get; set; } = default!;
}
