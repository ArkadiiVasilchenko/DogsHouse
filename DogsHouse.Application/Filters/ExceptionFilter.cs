using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Data.Common;
using Microsoft.Extensions.Logging;

namespace DogsHouse.Application.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

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

                _logger.LogWarning(validationException, "Validation error occurred.");
                context.ExceptionHandled = true;
            }

            if (context.Exception is ArgumentNullException argNullException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    title = argNullException.Message,
                    status = StatusCodes.Status400BadRequest
                });

                _logger.LogError(argNullException, "Argument null exception occurred.");
                context.ExceptionHandled = true;
            }

            if (context.Exception is DbException dbException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    title = dbException.Message,
                    status = StatusCodes.Status500InternalServerError
                });

                _logger.LogError(dbException, "Database error occurred.");
                context.ExceptionHandled = true;
            }

            if (!context.ExceptionHandled)
            {
                context.Result = new ObjectResult(new
                {
                    title = "An unexpected error occurred.",
                    status = StatusCodes.Status500InternalServerError
                });

                _logger.LogCritical(context.Exception, "An unexpected error occurred.");
                context.ExceptionHandled = true;
            }
        }
    }
}