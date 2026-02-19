using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NquilinCode.Communication.Responses;
using NquilinCode.Exceptions.BaseException;
using NquilinCode.Exceptions.Resources;

namespace NquilinCode.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NquilinCodeException)
        {
            HandleMyNquilinCodeException(context);
        }
        else
        {
            ThrowUnknowException(context);
        }
    }
    
    private static void HandleMyNquilinCodeException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case RegisterUserValidationException exception:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages.ToList()));
                break;
            case InvalidLoginException:
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new UnauthorizedObjectResult(context.Exception.Message);
                break;
        }
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ValidationMessages.UNKNOWN_ERROR));
    }
}