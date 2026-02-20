using System.Net;
using System.Text.Json;
using ApiVertrau.Domain.Exceptions;

namespace ApiVertrau.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, "Não Encontrado"),
            ConflictException => (HttpStatusCode.Conflict, "Conflito de Dados"),
            DomainException => (HttpStatusCode.BadRequest, "Requisição Inválida"),
            _ => (HttpStatusCode.InternalServerError, "Erro Interno no Servidor"),
        };

        var response = new
        {
            Status = (int)status,
            Title = title,
            Message = exception.Message,
            Timestamp = DateTime.Now,
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
