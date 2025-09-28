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

            switch (ex)
            {
                case BadRequestException:
                    status = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;
                case ConflictException:
                    status = HttpStatusCode.Conflict;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Ha ocurrido un error inesperado. Intente nuevamente más tarde.";
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = new ApiError { Message = message };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
