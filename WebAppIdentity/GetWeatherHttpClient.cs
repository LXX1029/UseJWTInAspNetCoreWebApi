using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAppIdentity
{
    /// <summary>
    ///  自定义HttpClient类
    /// </summary>
    public class GetWeatherHttpClient
    {
        private readonly HttpClient _client;

        public GetWeatherHttpClient(HttpClient client)
        {
            this._client = client;

        }
        /// <summary>
        /// 获取本地天气
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLocationWeatherInfo()
        {
            var response = await _client.GetAsync("/v3/weather/weatherInfo?key=05716fae19f88ecd42f4325f89dacefb&city=410100&extensions=base");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // TODO  可能增加的其它方法
    }

    /// <summary>
    /// 处理瞬时Http错误
    /// </summary>
    public static class TransientHttpErrorPolicyExtension
    {
        public static IHttpClientBuilder AddTransientHttpError(this IHttpClientBuilder httpClient)
        {

            return httpClient
                .AddHttpMessageHandler<CustomMessageHandler>()
                .AddTransientHttpErrorPolicy(policy =>
            {
                //return policy.WaitAndRetryAsync(3, i =>
                //{
                //    System.Diagnostics.Debug.WriteLine("11212");
                //    return TimeSpan.FromSeconds(300);
                //});
                return policy.OrResult(response => !response.IsSuccessStatusCode)
                .WaitAndRetryAsync(new[] {
                   TimeSpan.FromSeconds(1), // 进行第一次重试之前延时1秒
                   TimeSpan.FromSeconds(2), // 进行第二次重试之前延时2秒
                   TimeSpan.FromSeconds(10),
                });
            });
        }
    }

    public static class CustomPolicyHandlerExtension
    {
        public static IHttpClientBuilder AddCustomPolicyHandler(this IHttpClientBuilder httpClient)
        {
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(5, timeoutStrategy: Polly.Timeout.TimeoutStrategy.Pessimistic);
            return httpClient.AddPolicyHandler(timeoutPolicy);
        }
    }

}
