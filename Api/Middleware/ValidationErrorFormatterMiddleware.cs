using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Middleware
{
    public class ValidationErrorFormatterMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationErrorFormatterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 400 && !context.Response.HasStarted)
            {
                
                if (context.Items["ValidationProblemDetails"] is ValidationProblemDetails validationDetails)
                {
                    var errorDetails = new
                    {
                        Title = validationDetails.Title ?? "Validation Error",
                        Status = context.Response.StatusCode,
                        validationDetails.Errors
                    };

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
                }
            }
        }
    }
}
