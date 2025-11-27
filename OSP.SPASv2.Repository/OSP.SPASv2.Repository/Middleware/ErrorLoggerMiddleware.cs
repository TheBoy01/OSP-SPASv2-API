using OSP.SPASv2.Repository.Middleware.ErrorLoggerModel;

namespace OSP.SPASv2.Repository.Middleware
{
    public static class ErrorLoggerMiddleware
    {

       
            public static IApplicationBuilder UseErrorLogger(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ErrorLogger>();
            }
        
    }
}
