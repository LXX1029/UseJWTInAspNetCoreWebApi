using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.CustomConfigure
{
    public class ConfigureWeatherOptions : IConfigureOptions<WeatherOptions>
    {
        private readonly GetWeatherHttpClient _customHttpClient;

        /// <summary>
        /// 可注入其它服务，以获取外部配置
        /// </summary>
        public ConfigureWeatherOptions(GetWeatherHttpClient customHttpClient)
        {
            this._customHttpClient = customHttpClient;
        }

        public void Configure(WeatherOptions options)
        {

        }
    }
}
