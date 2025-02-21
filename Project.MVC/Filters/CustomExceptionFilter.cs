public void OnException(ExceptionContext context)
{
    var statusCode = context.Exception switch
    {
        KeyNotFoundException => HttpStatusCode.NotFound,
        ArgumentException => HttpStatusCode.BadRequest,
        DbUpdateException => HttpStatusCode.Conflict,
        _ => HttpStatusCode.InternalServerError
    };

    context.HttpContext.Response.StatusCode = (int)statusCode;
    context.Result = new JsonResult(new
    {
        Error = context.Exception.Message,
        context.Exception.Source,
        StackTrace = _env.IsDevelopment() ? context.Exception.StackTrace : null
    });
    
    context.ExceptionHandled = true;
}