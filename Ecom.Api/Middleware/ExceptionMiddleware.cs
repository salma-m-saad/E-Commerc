using Ecom.Api.Helper;

namespace Ecom.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
       /* public async Task InvokeAsync(HttpContext context)
        {
            try
            {
            }
            catch (Exception ex)
            {
                return (new ResponseAPI)
            }
        }*/
    }
}
