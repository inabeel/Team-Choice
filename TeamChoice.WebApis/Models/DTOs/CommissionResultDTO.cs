namespace TeamChoice.WebApis.Models.DTOs
{
    public class CommissionResultDTO
    {
        public decimal Amount { get; set; }
        public string ReceivingCurrencyCode { get; set; }
        public decimal AmountDue { get; set; }
        public string SendingCurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal TransactionFee { get; set; }
    }
    public class ExchangePayload
    {
        public string SendingCountry { get; set; }
        public string CurrencyCode { get; set; }
        public string ServiceCode { get; set; }
        public string RecipientCountry { get; set; }
        public decimal SendingAmount { get; set; }
        // Added RecipientAmount to support RateRequestController logic seen earlier
        public decimal RecipientAmount { get; set; }
    }

}
