using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TennisBookings.Web.Services;

namespace TennisBookings.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGreetingService _greetingService;
        private readonly IConfiguration _configuration;
        private readonly IWeatherForecaster _weatherForecaster;

        public IndexModel(IGreetingService greetingService, IConfiguration configuration,
            IWeatherForecaster weatherForecaster)
        {
            _greetingService = greetingService;
            _configuration = configuration;
            _weatherForecaster = weatherForecaster;
        }

        public string Greeting { get; private set; }
        public bool ShowGreeting => !string.IsNullOrEmpty(Greeting);
        public string ForecastSectionTitle { get; private set; }
        public string WeatherDescription { get; private set; }
        public bool ShowWeatherForecast { get; private set; }

        public async Task OnGet()
        {
            var features = new Features();
            _configuration.Bind("Features:HomePage", features);

            if (features.EnableGreeting)
            {
                Greeting = _greetingService.GetRandomGreeting();
            }

            ShowWeatherForecast = features.EnableWeatherForecast &&
                                  _weatherForecaster.ForecastEnabled;


            if (ShowWeatherForecast)
            {
                var title = features.ForecastSectionTitle;
                ForecastSectionTitle = string.IsNullOrEmpty(title) ? "How's the weather" : title;

                var currentWeather = await _weatherForecaster.GetCurrentWeatherAsync();

                if (currentWeather != null)
                {
                    switch (currentWeather.Description)
                    {
                        case "Sun":
                            WeatherDescription = "Sunny";
                            break;
                        case "Cloud":
                            WeatherDescription = "Cloudy";
                            break;
                        case "Rain":
                            WeatherDescription = "Rainy";
                            break;
                        case "Snow":
                            WeatherDescription = "Snowy";
                            break;
                    }
                }
            }
        }

        private class Features
        {
            public bool EnableGreeting { get; set; }
            public bool EnableWeatherForecast { get; set; }
            public string ForecastSectionTitle { get; set; }
        }
    }
}
