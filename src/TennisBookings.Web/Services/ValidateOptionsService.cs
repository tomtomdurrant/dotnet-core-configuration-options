using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TennisBookings.Web.Configuration;

namespace TennisBookings.Web.Services
{
    public class ValidateOptionsService : IHostedService
    {
        private readonly ILogger<ValidateOptionsService> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IOptions<HomePageConfiguration> _homePageConfig;
        private readonly IOptionsMonitor<ExternalServicesConfig> _externalServicesConfig;

        public ValidateOptionsService(ILogger<ValidateOptionsService> logger,
            IHostApplicationLifetime applicationLifetime, IOptions<HomePageConfiguration> homePageConfig,
            IOptionsMonitor<ExternalServicesConfig> externalServicesConfig)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
            _homePageConfig = homePageConfig;
            _externalServicesConfig = externalServicesConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _ = _homePageConfig.Value; // Accessing this triggers validation
                _ = _externalServicesConfig.Get(ExternalServicesConfig.ProductsApi);
                _ = _externalServicesConfig.Get(ExternalServicesConfig.WeatherApi);

            }
            catch (OptionsValidationException ex)
            {
                _logger.LogError("One or more options validation checks failed");
                foreach (var failure in ex.Failures)
                {
                    _logger.LogError(failure);
                }
                _applicationLifetime.StopApplication(); // stop the app now
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
