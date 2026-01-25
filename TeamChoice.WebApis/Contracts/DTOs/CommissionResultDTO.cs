namespace TeamChoice.WebApis.Contracts.DTOs;

public class CommissionResultDTO
{
    public decimal Amount { get; set; }
    public string ReceivingCurrencyCode { get; set; }
    public decimal AmountDue { get; set; }
    public string SendingCurrencyCode { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal TransactionFee { get; set; }
}
