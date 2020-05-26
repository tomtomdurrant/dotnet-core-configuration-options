using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisBookings.Web.Configuration
{
    public class ExternalServicesConfig
    {
        public const string WeatherApi = "WeatherApi";
        public const string ProductsApi = "ProductsApi";

        public string Url { get; set; }
        public int MinsToCache { get; set; }
    }
}
