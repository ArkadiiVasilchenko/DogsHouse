using DogsHouse.Domain.Models;
using Microsoft.Extensions.Options;

namespace DogsHouse.API.Middlewares
{
    public class RequestLimiterMiddleware : IMiddleware, IDisposable
    {
        private int _requestCount = 0;
        private readonly Timer _timer;
        private readonly RequestLimiterOptions _options;

        public RequestLimiterMiddleware(IOptions<RequestLimiterOptions> options)
        {
            _options = options.Value;
            _timer = new Timer(_ => ResetRequestCount(), null, TimeSpan.Zero, TimeSpan.FromSeconds(_options.ResetIntervalInSeconds));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Interlocked.Increment(ref _requestCount) > _options.MaxRequestsPerSecond)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too Many Requests");
                return;
            }

            await next(context);
        }

        private void ResetRequestCount()
        {
            Interlocked.Exchange(ref _requestCount, 0);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}