namespace TeamChoice.WebApis.Domain.Models.DTOs;

public record ExchangePayloadDto(
    string SendingCountry,
    string CurrencyCode,
    string ServiceCode,
    string RecipientCountry,
    double SendingAmount
);

public record ExchangeRatePayloadDto
{
    public string? LocationCode { get; init; }
    public string ServiceCode { get; init; } = default!;
    public double RecipientAmount { get; init; }
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
        public double AmountDue { get; init; }
        public double ExchangeRate { get; init; }
        public double TransactionFee { get; init; }
    }

    public record RecipientDto
    {
        public double Amount { get; init; }
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
        public double AmountDue { get; init; }
        public string? CurrencyCode { get; init; }
        public double ExchangeRate { get; init; }
        public double TransactionFee { get; init; }
    }

    public record RecipientDto
    {
        public double Amount { get; init; }
        public string? CurrencyCode { get; init; }
    }
}