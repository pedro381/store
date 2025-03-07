using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.Domain.Configurations;

namespace Store.CrossCutting.Middlewares
{
    public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(Contants.CorrelationId, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Append(Contants.CorrelationId, correlationId);
            }

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(Contants.CorrelationId))
                {
                    context.Response.Headers.Append(Contants.CorrelationId, correlationId);
                }
                return Task.CompletedTask;
            });

            using (logger.BeginScope(correlationId!))
            {
                await next(context);
            }
        }
    }
}
