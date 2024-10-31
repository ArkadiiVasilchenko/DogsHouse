using DogsHouse.API.Middlewares;
using DogsHouse.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace DogsHouse.MiddlewaresTests
{
    public class RequestLimiterMiddlewareTests
    {
        private readonly RequestLimiterMiddleware middleware;
        private readonly Mock<RequestDelegate> next;

        public RequestLimiterMiddlewareTests()
        {
            next = new Mock<RequestDelegate>();

            var options = Options.Create(new RequestLimiterOptions
            {
                MaxRequestsPerSecond = 5,
                ResetIntervalInSeconds = 60
            });

            middleware = new RequestLimiterMiddleware(options);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturn429_WhenLimitExceeded()
        {
            // Arrange
            var context = new DefaultHttpContext();

            next.Setup(n => n(context)).Returns(Task.CompletedTask);

            // Act
            for (int i = 0; i < 5; i++)
            {
                await middleware.InvokeAsync(context, next.Object);
            }
            await middleware.InvokeAsync(context, next.Object);

            // Assert
            Assert.Equal(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
        }
    }
}