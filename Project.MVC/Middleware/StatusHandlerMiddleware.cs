using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class StatusHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public StatusHandlerMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
        
        if (context.Response.StatusCode == 404)
            await context.Response.WriteAsync("Custom 404 Page");
    }
}