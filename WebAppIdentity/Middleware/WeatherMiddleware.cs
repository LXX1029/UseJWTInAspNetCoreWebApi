using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.CustomConfigure;

namespace WebAppIdentity.Middleware
{

    /// <summary>
    /// 匹配weather 终结点路由扩展
    /// </summary>
    public static class WeatherEndpointRouteBuilderExtension
    {
        public static IEndpointConventionBuilder MapWeather(this IEndpointRouteBuilder endpoint, string routeTemplate)
        {
            var pipeline = endpoint.CreateApplicationBuilder().UseMiddleware<WeatherMiddleware>().Build();
            return endpoint.Map(routeTemplate, pipeline);
        }
    }
    /// <summary>
    /// weather 中间件
    /// </summary>
    public class WeatherMiddleware
    {
        private readonly RequestDelegate _next;

        public WeatherMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task Invoke(HttpContext context, CustomHttpClient customHttpClient)
        {
            var weatherJson = await customHttpClient.GetLocationWeatherInfo();
            await context.Response.WriteAsync(weatherJson);   // 输出天气json
            //await this._next(context);
        }
    }
}
