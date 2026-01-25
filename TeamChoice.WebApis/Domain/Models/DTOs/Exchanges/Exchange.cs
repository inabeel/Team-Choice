namespace TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

public record ExchangeRatePayloadDto
{
    public string SendingCountry { get; init; } = default!;

    public string CurrencyCode { get; init; } = default!;
    
    public string ServiceCode { get; init; } = default!;
    
    public string RecipientCountry { get; init; } = default!;

    public decimal SendingAmount { get; init; }

    public decimal RecipientAmount { get; init; }

    public string? LocationCode { get; init; }

    public string RecipientCurrencyCode { get; init; } = default!;
    
    public string CountryCode { get; init; } = default!;
}

public record ExchangeRateResponseDto
{
    public ExchangeDetailsDto? ExchangeDetails { get; init; }

    public record ExchangeDetailsDto
    {
        public PayerDto? Payer { get; init; }

        public RecipientDto? Recipient { get; init; }
    }

    public record PayerDto
    {
        public decimal AmountDue { get; init; }

        public decimal ExchangeRate { get; init; }

        public decimal TransactionFee { get; init; }
    }

    public record RecipientDto
    {
        public decimal Amount { get; init; }
    }
}

public record ExchangeResponseBuilderDto
{
    public DateTime Timestamp { get; init; }

    public string? StatusMessage { get; init; }
    
    public int StatusCode { get; init; }
    
    public ExchangeDetailsDto? ExchangeDetails { get; init; }

    public record ExchangeDetailsDto
    {
        public PayerDto? Payer { get; init; }
        
        public RecipientDto? Recipient { get; init; }
    }

    public record PayerDto
    {
        public decimal AmountDue { get; init; }

        public string? CurrencyCode { get; init; }
        
        public decimal ExchangeRate { get; init; }
        
        public decimal TransactionFee { get; init; }
    }

    public record RecipientDto
    {
        public decimal Amount { get; init; }
        
        public string? CurrencyCode { get; init; }
    }
}