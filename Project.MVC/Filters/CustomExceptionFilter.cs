using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Project.MVC.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode = context.Exception switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,  // 404 Not Found
                ArgumentException => HttpStatusCode.BadRequest,   // 400 Bad Request
                DbUpdateException => HttpStatusCode.Conflict,     // 409 Conflict
                _ => HttpStatusCode.InternalServerError           // 500 Internal Server Error
            };

            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new JsonResult(new
            {
                Error = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            });

            context.ExceptionHandled = true;
        }
    }
}
