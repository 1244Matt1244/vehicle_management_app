using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Project.MVC.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(new
            {
                Error = "An unexpected error occurred",
                Details = context.Exception.Message
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            
            context.ExceptionHandled = true;
        }
    }
}