using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Data.Common;

namespace DogsHouse.Application.Filters
{
    public class ControllerExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                var errors = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                context.Result = new BadRequestObjectResult(new
                {
                    title = validationException.Message,
                    status = StatusCodes.Status400BadRequest,
                    errors
                });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is ArgumentNullException argNullException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    title = argNullException.Message,
                    status = StatusCodes.Status400BadRequest
                });
                context.ExceptionHandled = true;
            }
            else if (context.Exception is DbException dbException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    title = dbException.Message,
                    status = StatusCodes.Status500InternalServerError
                });
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    title = "An unexpected error occurred.",
                    status = StatusCodes.Status500InternalServerError,
                    detail = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }
        }
    }
}