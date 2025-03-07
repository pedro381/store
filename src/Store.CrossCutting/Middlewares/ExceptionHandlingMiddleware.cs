using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.Domain.Base;
using Store.Domain.Configurations;
using System.Net;
using System.Text.Json;

namespace Store.CrossCutting.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = Contants.ApplicationJson;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new ResponseBase();
            errorResponse.AddErro(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);

            return context.Response.WriteAsync(result);
        }
    }
}
