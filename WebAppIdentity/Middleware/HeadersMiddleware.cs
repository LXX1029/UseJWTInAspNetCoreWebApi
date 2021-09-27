using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHeaders(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HeadersMiddleware>();
        }

    }

    public class HeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public HeadersMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
       
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("execute-time", "21121");
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}
