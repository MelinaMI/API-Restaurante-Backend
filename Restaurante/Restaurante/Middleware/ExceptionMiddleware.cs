using Application.Models.Response;
using Restaurante.Models;
using System.Net;
using System.Text.Json;
using static Application.Validators.Exceptions;
//Captura todas las excepciones de la aplicación
namespace Restaurante.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            string message = ex.Message;
            if (ex is BadRequestException)
                status = HttpStatusCode.BadRequest;
            else if (ex is NotFoundException)
                status = HttpStatusCode.NotFound;
            else if (ex is ConflictException)
                status = HttpStatusCode.Conflict;
            else
                status = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = new ApiError { Message = message };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
