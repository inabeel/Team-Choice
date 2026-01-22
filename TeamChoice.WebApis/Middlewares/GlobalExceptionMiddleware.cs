using System.Net;
using System.Text.Json;
using TeamChoice.WebApis.Domain.Exceptions;
using TeamChoice.WebApis.Models.DTOs.HttpResponse;

namespace TeamChoice.WebApis.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            InvalidTransactionStatusException => HttpStatusCode.BadRequest,
            AlreadyPaidException => HttpStatusCode.Conflict,
            TransactionForwardingException => HttpStatusCode.BadGateway,
            _ => HttpStatusCode.InternalServerError
        };

        var response = new ErrorResponse(
            Error: statusCode.ToString(),
            Code: exception.GetType().Name,
            Message: exception.Message,
            Path: context.Request.Path,
            Details: null
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }
}

