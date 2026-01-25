namespace TeamChoice.WebApis.Contracts.Exchanges;

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
