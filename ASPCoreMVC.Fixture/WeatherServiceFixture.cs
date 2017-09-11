using System;
using Xunit;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace ASPCoreMVC.Fixture
{
    public class WeatherServiceFixture
    {

        public WeatherServiceFixture()
        {
            //setup automapper mappings
            InitializeMapping();
        }

        [Fact]
        public void ValidateConfigurationSettingsLosdFileDirectly()
        {
            IConfigurationRoot Configuration;
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string configValue = Configuration["weatherURL"];
            Assert.NotNull(configValue);
        }


        //[Fact]
        //public void ValidateConfigurationSettingsLosdFromOptionClass()
        //{
        //    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration

        //}

        [Fact]
        public async void SearchCurrentTestAsync()
        {
            var configSettings = GetWeatherConfiguration();
            string zip = "17011";
            Assert.NotNull(configSettings.url);
            Assert.NotNull(configSettings.token);
            OpenWeatherCurrentResponse rawWeather;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configSettings.url);
                var response = await client.GetAsync($"/data/2.5/weather?zip={zip}&appid={configSettings.token}&units=Imperial");
                response.EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();
                rawWeather = JsonConvert.DeserializeObject<OpenWeatherCurrentResponse>(stringResult);
            }
            Assert.NotNull(rawWeather);
            //use automapper to put into viewmodel
            var mapper = _mapperConfig.CreateMapper();
            CurrentWeatherModel item = mapper.Map<CurrentWeatherModel>(rawWeather);
            Assert.NotNull(item);
        }

        [Fact]
        public async void SearchForecastTestAsync()
        {
            var configSettings = GetWeatherConfiguration();
            string zip = "17011";
            Assert.NotNull(configSettings.url);
            Assert.NotNull(configSettings.token);
            OpenWeatherForecastResponse rawWeather;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(configSettings.url);
                var response = await client.GetAsync($"/data/2.5/forecast?zip={zip}&appid={configSettings.token}&units=Imperial");
                response.EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();
                rawWeather = JsonConvert.DeserializeObject<OpenWeatherForecastResponse>(stringResult);
            }
            Assert.NotNull(rawWeather);
            //use automapper to put into viewmodel
            var mapper = _mapperConfig.CreateMapper();
            ForecastWeatherModel item = mapper.Map<ForecastWeatherModel>(rawWeather);
            Assert.NotNull(item);
        }

        public (string url, string token) GetWeatherConfiguration()
        {
            string url = string.Empty;
            string token = string.Empty;
            IConfigurationRoot Configuration;
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            url = Configuration["weatherURL"];
            token = Configuration["weatherToken"];
            return (url, token);
        }

        MapperConfiguration _mapperConfig;

        public void InitializeMapping()
        {
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UnitProfile>();
            });
            _mapperConfig.AssertConfigurationIsValid();
        }
    }

    public class OpenWeatherCurrentResponse
    {
        public string Name { get; set; }

        public IEnumerable<WeatherDescription> Weather { get; set; }

        public Main Main { get; set; }

        public string GetWeatherNames()
        {
            return string.Join(",", Weather.Select(x => x.Main));
        }

        public string GetWeatherDescriptions()
        {
            return string.Join(",", Weather.Select(x => x.Description));
        }

        public string GetWeatherIcon()
        {
            return string.Join(",", Weather.Select(x => x.Icon));
        }

    }

    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Main
    {
        public string Temp { get; set; }
        public string Temp_Max { get; set; }
        public string Temp_Min { get; set; }
    }

    public class OpenWeatherForecastResponse
    {
        public CityStruct City { get; set; }

        public IEnumerable<Forecast> list { get; set; }


        public IEnumerable<ForecastDetail> GetForecastDetails()
        {
            List<ForecastDetail> _list = new List<ForecastDetail>();
            foreach (Forecast _item in list)
            {
                ForecastDetail _mapped = new ForecastDetail();
                _mapped.ForecastDate =DateTime.Parse(_item.dt_txt);
                _mapped.Temperature =_item.Main.Temp;
                _mapped.WeatherDescription = string.Join(",", _item.Weather.Select(x => x.Description));
                _mapped.WeatherIcon = string.Join(",", _item.Weather.Select(x => x.Icon));
                _mapped.WeatherName = string.Join(",", _item.Weather.Select(x => x.Main)); 
                _list.Add(_mapped);
            }
            return _list;
        }

    }

    public class CityStruct
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class Forecast
    {
        public IEnumerable<WeatherDescription> Weather { get; set; }

        public ForecastMain Main { get; set; }

        public string dt_txt { get; set; }

    }

    public class ForecastMain
    {
        public string Temp { get; set; }
        public string Temp_Max { get; set; }
        public string Temp_Min { get; set; }
        public string Humidity { get; set; }

    }

    public class CurrentWeatherModel
    {
        public string CityName { get; set; }
        public string CurrentTemperature { get; set; }
        public string CurrentWeatherName { get; set; }
        public string CurrentWeatherDescription { get; set; }
        public string CurrentWeatherIcon { get; set; }
    }

    public class ForecastWeatherModel
    {
        public string CityName { get; set; }
        public IEnumerable<ForecastDetail> Details { get; set; }
    }

    public class ForecastDetail
    {
        public string Temperature { get; set; }
        public string WeatherName { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public DateTime ForecastDate { get; set; }

    }

    //public class TestingOptions
    //{
    //    public TestingOptions()
    //    {
    //        // Set default value.
    //        Option1 = "value1_from_ctor";
    //    }
    //    public string Option1 { get; set; }
    //    public int Option2 { get; set; } = 5;
    //}


    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            CreateMap<OpenWeatherCurrentResponse, CurrentWeatherModel>()
                .ForMember(dto => dto.CityName, conf => conf.MapFrom(ol => ol.Name))
                .ForMember(dto => dto.CurrentTemperature, conf => conf.MapFrom(ol => ol.Main.Temp))
                .ForMember(dto => dto.CurrentWeatherName, conf => conf.MapFrom(ol => ol.GetWeatherNames()))
                .ForMember(dto => dto.CurrentWeatherDescription, conf => conf.MapFrom(ol => ol.GetWeatherDescriptions()))
                .ForMember(dto => dto.CurrentWeatherIcon, conf => conf.MapFrom(ol => ol.GetWeatherIcon()));

            CreateMap<OpenWeatherForecastResponse, ForecastWeatherModel>()
                .ForMember(dto => dto.CityName, conf => conf.MapFrom(ol => ol.City.Name))
                .ForMember(dto => dto.Details, conf => conf.MapFrom(ol => ol.GetForecastDetails()));


        }
    }

}
