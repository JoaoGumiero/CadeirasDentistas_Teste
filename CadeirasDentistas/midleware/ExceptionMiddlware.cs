using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CadeirasDentistas.midleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Passa para o próximo middleware caso houver
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                var error = new { Message = "Erro interno no servidor", details = ex.Message, stackTrace = ex.StackTrace};
                // var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                //var json = JsonSerializer.Serialize(options);
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }   
}