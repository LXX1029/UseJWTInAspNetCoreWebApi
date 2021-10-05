using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Middleware
{
    /// <summary>
    /// 封装自定义endpoint
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// 自定义EndPoint终结点
        /// </summary>
        /// <param name="endpoints">route builder</param>
        /// <param name="routeTemplate">路由模板</param>
        /// <returns>IEndpointConventionBuilder</returns>
        public static IEndpointConventionBuilder MapPingPong(this IEndpointRouteBuilder endpoints, string routeTemplate)
        {
            var pipeline = endpoints.CreateApplicationBuilder().UseMiddleware<PingPongMiddleware>().Build();
            return endpoints.Map(routeTemplate, pipeline).WithDisplayName("ping-pong");
        }
    }

    public class PingPongMiddleware
    {
        private readonly RequestDelegate _next;

        public PingPongMiddleware(RequestDelegate next)
        {
            this._next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/ping"))
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"{DateTime.Now.ToLongDateString()} ping pong");
                return;
            }
            await this._next(context);
        }
    }
}
