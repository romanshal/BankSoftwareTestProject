using FluentValidation;
using System.Net;

namespace BankSoftware.API.Middlewares
{
    public class ExceptionWrappingMiddleware(
        RequestDelegate requestDelegate,
        ILogger<ExceptionWrappingMiddleware> logger)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;
        private readonly ILogger<ExceptionWrappingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing request({traceId}): {error}", httpContext.TraceIdentifier, ex.Message);
                await HandleExeptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExeptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)GetErrorCode(ex);
            var responseDate = GetResponseDate(ex);

            httpContext.Response.ContentType = "application/json";
            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(responseDate);

            return httpContext.Response.WriteAsync(jsonResponse);
        }

        private static HttpStatusCode GetErrorCode(Exception ex)
        {
            return ex switch
            {
                ArgumentNullException or ValidationException => HttpStatusCode.BadRequest,
                NotImplementedException => HttpStatusCode.NotImplemented,
                _ => HttpStatusCode.InternalServerError,
            };
        }

        private static string GetResponseDate(Exception ex)
        {
            return ex switch
            {
                ArgumentNullException or ValidationException => ex.Message,
                _ => "Something went wrong...",
            };
        }
    }

    public static class ExeptionWrappingMiddlewareExtention
    {
        public static IApplicationBuilder UseExeptionWrappingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionWrappingMiddleware>();
        }
    }
}
