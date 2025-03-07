using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Store.Domain;

namespace Store.CrossCutting.Middlewares
{
    public class BodyLoggingMiddleware(RequestDelegate next, ILogger<BodyLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                logger.LogInformation(Resource.msgContentBody, body);
            }

            await next(context);
        }
    }
}
