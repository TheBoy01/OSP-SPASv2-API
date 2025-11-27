using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OSP.Common.Repository.Middleware.ErrorLoggerModel
{
    public class ErrorLogger
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public ErrorLogger(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;

            logFactory.AddFile("C:/Logs/LogFile-{Date}.txt");
            _logger = logFactory.CreateLogger("ErrorLogger");
            
        }

        public Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation(DateTime.Now.ToString());

            return _next(httpContext);
        }
    }

    public static class ErrorLoggerExtensions
    {
        public static IApplicationBuilder UseErrorLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLogger>();
        }
    }

   
}
