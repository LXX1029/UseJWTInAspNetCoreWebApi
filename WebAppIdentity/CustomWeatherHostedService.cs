using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAppIdentity
{
    /// <summary>
    /// 自定义一个后台任务、用于定时获取当前天气，当前天气接口调用使用当前请求 scope 容器中的服务。
    /// 该BackgroundService 在程序启动时紧紧被调用一次
    /// </summary>
    public class CustomWeatherHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomWeatherHostedService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedProvider = scope.ServiceProvider;
                    var httpClient = scopedProvider.GetRequiredService<GetWeatherHttpClient>();
                    var weather = await httpClient.GetLocationWeatherInfo();
                    // 可将当前weather 存储到缓存中，以供在全局范围内使用
                    System.Diagnostics.Debug.WriteLine(weather);
                }
                // 10s 中运行一次
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
