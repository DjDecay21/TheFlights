
using LOT_Project.Exeptions;
using System.Linq.Expressions;

namespace LOT_Project.Middleware
{
    public class ErrorHadlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(BadRequestExeption badRequestExeption)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestExeption.Message);
            }
        }
    }
}
