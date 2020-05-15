using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TennisBookings.Web.Configuration;
using TennisBookings.Web.Services;

namespace TennisBookings.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGreetingService _greetingService;
        private readonly HomePageConfiguration _homePageConfiguration;
        private readonly IWeatherForecaster _weatherForecaster;

        public IndexModel(IGreetingService greetingService, IOptions<HomePageConfiguration> homePageConfiguration,
            IWeatherForecaster weatherForecaster)
        {
            _greetingService = greetingService;
            _homePageConfiguration = homePageConfiguration.Value;
            _weatherForecaster = weatherForecaster;
        }

        public string Greeting { get; private set; }
        public bool ShowGreeting => !string.IsNullOrEmpty(Greeting);
        public string ForecastSectionTitle { get; private set; }
        public string WeatherDescription { get; private set; }
        public bool ShowWeatherForecast { get; private set; }

        public async Task OnGet()
        {


            if (_homePageConfiguration.EnableGreeting)
            {
                Greeting = _greetingService.GetRandomGreeting();
            }

            ShowWeatherForecast = _homePageConfiguration.EnableWeatherForecast &&
                                  _weatherForecaster.ForecastEnabled;


            if (ShowWeatherForecast)
            {
                var title = _homePageConfiguration.ForecastSectionTitle;
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
    }
}
