using DogsHouse.Application.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DogsHouse.ExceptionFilterTests
{
    public class ControllerExceptionFilterTests
    {
        private readonly ControllerExceptionFilter _filter;

        public ControllerExceptionFilterTests()
        {
            _filter = new ControllerExceptionFilter();
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