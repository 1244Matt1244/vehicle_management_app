using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class StatusHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StatusHandlerMiddleware> _logger;

    // Constructor to inject the next delegate and logger
    public StatusHandlerMiddleware(RequestDelegate next, ILogger<StatusHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // The Invoke method processes the request and handles the response
    public async Task Invoke(HttpContext context)
    {
        // Process the request
        await _next(context);

        // Check if the response status code is 404 and customize the response
        if (context.Response.StatusCode == 404)
        {
            _logger.LogWarning("404 Not Found: {Path}", context.Request.Path);
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<h1>Custom 404 Page</h1><p>The page you're looking for is not found.</p>");
        }
    }
}
