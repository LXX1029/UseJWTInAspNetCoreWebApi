using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAppIdentity.Quartz
{
    /// <summary>
    /// 定义获取天气信息任务
    /// 当任务被执行时，都会创建新的Job引用
    ///  DisallowConcurrentExecution  不进行并发执行
    /// </summary>
    [DisallowConcurrentExecution]
    public class GetWeatherHttpClientJob : IJob
    {
        private readonly GetWeatherHttpClient _client;
        private readonly ILogger<GetWeatherHttpClientJob> _logger;

        public GetWeatherHttpClientJob(ILogger<GetWeatherHttpClientJob> logger, GetWeatherHttpClient client)
        {
            this._client = client;
            this._logger = logger;

        }

        /// <summary>
        /// 执行具体业务逻辑，可被调度执行多次
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            this._logger.LogDebug("=============================================");
            this._logger.LogInformation("开始获取天气信息");
            var response = await _client.GetLocationWeatherInfo();
            this._logger.LogInformation($"DateTime：{DateTime.Now}  weather:{response}");
            this._logger.LogInformation("获取信息完成");
            this._logger.LogInformation("=============================================");
        }
    }
}
