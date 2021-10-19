using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages.Shared.ViewComponents
{
    public class WeatherViewComponent : ViewComponent
    {
        private readonly GetWeatherHttpClient _weatherHttpClient;

        public WeatherViewComponent(GetWeatherHttpClient httpClient)
        {
            this._weatherHttpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(string adcode= "410100")
        {
            var result = await _weatherHttpClient.GetLocationWeatherInfo(adcode);
            var weatherModel = JsonConvert.DeserializeObject<Weather>(result);
            return View("default1", weatherModel);
        }
    }
}
