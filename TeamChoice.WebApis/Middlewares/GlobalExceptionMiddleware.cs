using System.Net;
using System.Text.Json;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Domain.Exceptions;

namespace TeamChoice.WebApis.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, status) = exception switch
        {
            TransactionValidationException =>
                (HttpStatusCode.BadRequest, "BAD_REQUEST"),

            InvalidServiceCodeException =>
                (HttpStatusCode.BadRequest, "INVALID_SERVICE_CODE"),

            DuplicateTransactionException =>
                (HttpStatusCode.Conflict, "DUPLICATE_TRANSACTION"),

            AlreadyPaidException =>
                (HttpStatusCode.Conflict, "ALREADY_PAID"),

            TransactionNotFoundException =>
                (HttpStatusCode.NotFound, "TRANSACTION_NOT_FOUND"),

            ServiceNotFoundException =>
                (HttpStatusCode.NotFound, "SERVICE_NOT_FOUND"),

            TransactionForwardingException or RemittanceFailedException =>
                (HttpStatusCode.ServiceUnavailable, "DOWNSTREAM_FAILURE"),

            _ =>
                (HttpStatusCode.InternalServerError, "INTERNAL_ERROR")
        };

        var response = new HttpResponseDto<object>
        {
            TimeStamp = DateTimeOffset.UtcNow,
            StatusCode = (int)statusCode,
            Status = status,
            Message = exception.Message,
            Data = null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, _jsonOptions));
    }
}
