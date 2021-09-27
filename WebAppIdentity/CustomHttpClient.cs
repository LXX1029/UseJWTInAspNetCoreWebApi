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
    public class CustomHttpClient
    {
        private readonly HttpClient _client;

        public CustomHttpClient(HttpClient client)
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
}
