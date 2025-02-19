using Microsoft.AspNetCore.Mvc.Filters;

namespace Project.MVC.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ViewResult
            {
                ViewName = "Error",
                StatusCode = 500
            };
        }
    }
}