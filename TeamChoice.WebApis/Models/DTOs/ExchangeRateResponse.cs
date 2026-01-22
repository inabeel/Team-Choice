namespace TeamChoice.WebApis.Models.DTOs
{
    public class ExchangeRateResponse
    {
        public DateTime Timestamp { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public ExchangeDetails ExchangeDetails { get; set; }
    }

    public class ExchangeDetails
    {
        public Recipient Recipient { get; set; }
        public Payer Payer { get; set; }
    }

    public class Recipient
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Payer
    {
        public decimal AmountDue { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal TransactionFee { get; set; }
    }
}
