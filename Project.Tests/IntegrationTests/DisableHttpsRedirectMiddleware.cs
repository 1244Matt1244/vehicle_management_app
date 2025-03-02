using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Project.Tests.IntegrationTests
{
    public class DisableHttpsRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public DisableHttpsRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Bypass HTTPS redirection
            await _next(context);
        }
    }
}