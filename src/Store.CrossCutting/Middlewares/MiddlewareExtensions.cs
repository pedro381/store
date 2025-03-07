using Microsoft.AspNetCore.Builder;

namespace Store.CrossCutting.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }

        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseBodyLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BodyLoggingMiddleware>();
        }
    }
}
