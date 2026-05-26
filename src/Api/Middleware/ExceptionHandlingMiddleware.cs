using System.Net;
using System.Text.Json;
using FluentValidation;
using WizCo.Orders.Api.Models;
using WizCo.Orders.Domain.Exceptions;

namespace WizCo.Orders.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (PedidoNaoEncontradoException)
        {
            _logger.LogWarning("Pedido não encontrado");
            await WriteErrorAsync(context, HttpStatusCode.NotFound, new ErrorResponse
            {
                Success = false,
                Message = "Pedido não encontrado"
            });
        }
        catch (RegraNegocioException ex)
        {
            _logger.LogWarning("Regra de negócio: {Message}", ex.Message);
            await WriteErrorAsync(context, HttpStatusCode.Conflict, new ErrorResponse
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (ValidationException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, new ErrorResponse
            {
                Success = false,
                Message = "Erro de validação",
                Errors = ex.Errors.Select(e => e.ErrorMessage).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, new ErrorResponse
            {
                Success = false,
                Message = "Ocorreu um erro interno."
            });
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, ErrorResponse response)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
}
