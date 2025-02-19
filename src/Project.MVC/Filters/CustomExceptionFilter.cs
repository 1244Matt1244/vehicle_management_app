using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Project.MVC.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            // Log the exception details
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            // Prepare the error view with additional details
            var result = new ViewResult
            {
                ViewName = "Error"
            };

            // Pass exception details to the view (optional, for debugging or user-friendly error messages)
            result.ViewData["ErrorMessage"] = context.Exception.Message;
            result.ViewData["StackTrace"] = context.Exception.StackTrace;

            context.Result = result;
            context.ExceptionHandled = true; // Mark the exception as handled
        }
    }
}
