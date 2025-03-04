using Microsoft.AspNetCore.Builder;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.MVC.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                
                // Handle 404 for undefined routes
                if (context.Response.StatusCode == 404)
                {
                    _logger.LogWarning("404 Not Found: {Path}", context.Request.Path);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
                    {
                        Title = "Resource Not Found",
                        Status = 404,
                        Detail = $"The requested resource '{context.Request.Path}' was not found."
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Global exception caught");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
            {
                Title = "Server Error",
                Status = 500,
                Detail = "An unexpected error occurred. Please try again later."
            }));
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}

