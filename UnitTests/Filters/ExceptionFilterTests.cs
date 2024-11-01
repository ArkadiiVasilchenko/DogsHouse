using DogsHouse.API.Controllers;
using DogsHouse.Application.Filters;
using FakeItEasy;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace DogsHouse.ExceptionFilterTests
{
    public class ExceptionFilterTests
    {
        private readonly ExceptionFilter _filter;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilterTests()
        {
            _logger = A.Fake<ILogger<ExceptionFilter>>();
            _filter = new ExceptionFilter(_logger);
        }

        [Fact]
        public void OnException_ShouldReturnBadRequest_WhenValidationExceptionThrown()
        {
            // Arrange
            var context = CreateExceptionContext(new ValidationException("Validation failed"));

            // Act
            _filter.OnException(context);

            // Assert
            var result = context.Result as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public void OnException_ShouldReturnBadRequest_WhenArgumentNullExceptionThrown()
        {
            // Arrange
            var context = CreateExceptionContext(new ArgumentNullException("ArgumentNullException"));

            // Act
            _filter.OnException(context);

            // Assert
            var result = context.Result as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        private ExceptionContext CreateExceptionContext(Exception exception)
        {
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new ActionDescriptor());

            return new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = exception
            };
        }
    }
}