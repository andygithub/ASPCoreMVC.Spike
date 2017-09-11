//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using ASPCoreMVC.Spike.Models;
//using Microsoft.Extensions.Options;
//using System.Net.Http;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Routing;

//namespace ASPCoreMVC_Spike.Controllers
//{
//    [Route("api/[controller]")]
//    public class WeatherDataController : Controller
//    {

//        private readonly ApplicationOptions _options;

//        public WeatherDataController(IOptions<ApplicationOptions> optionsAccessor)
//        {
//            _options = optionsAccessor.Value;
//        }

//        [HttpGet("[action]/{city}")]
//        public async Task<IActionResult> City(string City)
//        {
//            string zip = "17011";

//            using (var client = new HttpClient())
//            {
//                try
//                {
//                    client.BaseAddress = _options.weatherURL;
//                    var response = await client.GetAsync($"/data/2.5/weather?zip={zip}&appid={_options.weatherToken}&units=Imperial");
//                    response.EnsureSuccessStatusCode();

//                    var stringResult = await response.Content.ReadAsStringAsync();
//                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
//                    return Ok(new
//                    {
//                        Temp = rawWeather.Main.Temp,
//                        Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
//                        City = rawWeather.Name
//                    });
//                }
//                catch (HttpRequestException httpRequestException)
//                {
//                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
//                }
//            }
//        }


//    }
//}
