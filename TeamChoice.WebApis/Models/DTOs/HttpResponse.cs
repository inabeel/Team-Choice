using TeamChoice.WebApis.Models.DTOs.Transactions;

namespace TeamChoice.WebApis.Models.DTOs.HttpResponse;

public record HttpResponseDto<T>
{
    public DateTime? TimeStamp { get; init; }
    public int StatusCode { get; init; }
    public string? Status { get; init; }
    public string? Source { get; init; }
    public string? Reason { get; init; }
    public string? Message { get; init; }
    public string? DeveloperMessage { get; init; }

    public T? Data { get; init; }
    public LocationDto? LocationDetails { get; init; }
    public PaymentDto? PaymentDetails { get; init; }
}

public sealed record ErrorResponse(
    string Error,
    string? Code,
    string Message,
    string Path,
    IReadOnlyList<object>? Details
);