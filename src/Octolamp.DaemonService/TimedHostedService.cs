

using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octolamp.Contracts.Dtos;
using Octolamp.Contracts.Protos;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Octolamp.Contracts.Protos.Covid;
using static Octolamp.Contracts.Protos.Stocks;

namespace Octolamp.DaemonService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private async Task DoWork()
        {
            try
            {
                var response = await _covidClient.DoHandshakeAsync(new HandshakeRequest { ClientToken = DateTime.Now.ToLongTimeString() });
                _logger.LogInformation($"Handshake success. Client Token = {response.ClientToken}; Server Token: {response.ServerToken}");
                
                var report = await _covid19ApiClient.GetSummaryAsync();
                if (report != null && report.Global != null && report.Countries != null)
                {
                    await RelayGlobalSummaryAsync(report);
                    await RelayCountryUpdatesAsync(report);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        private async Task RelayGlobalSummaryAsync(RawCovidReport report)
        {
            var sumRespose = await _covidClient.RegisterReportSummaryAsync(new CovidGlobalReport
            {
                Date = Timestamp.FromDateTimeOffset(DateTime.UtcNow),
                NewConfirmed = report.Global.NewConfirmed,
                TotalConfirmed = report.Global.TotalConfirmed,
                NewDeaths = report.Global.NewDeaths,
                TotalDeaths = report.Global.TotalDeaths,
                NewRecovered = report.Global.NewRecovered,
                TotalRecovered = report.Global.TotalRecovered,
            });

            if (!string.IsNullOrWhiteSpace(sumRespose.ServerToken))
            {
                this._logger.LogInformation("Successfully registered the Global summary payload");
            }
        }

        private async Task RelayCountryUpdatesAsync(RawCovidReport report)
        {
            foreach (var country in report.Countries)
            {
                var conResponse = await _covidClient.RegisterCountryReportAsync(new CovidCountryReport
                {
                    CountryCode = country.CountryCode,
                    CountryCountry = country.Country,
                    Slug = country.Slug,
                    Date = Timestamp.FromDateTimeOffset(DateTime.UtcNow),
                    NewConfirmed = country.NewConfirmed,
                    TotalConfirmed = country.TotalConfirmed,
                    NewDeaths = country.NewDeaths,
                    TotalDeaths = country.TotalDeaths,
                    NewRecovered = country.NewRecovered,
                    TotalRecovered = country.TotalRecovered,
                });

                if (!string.IsNullOrWhiteSpace(conResponse.ServerToken))
                {
                    this._logger.LogInformation($"Successfully registered the Country {country.Country} summary payload");
                }
            }
        }

        #region Service methods
        private readonly StocksClient _stockClient;
        private readonly CovidClient _covidClient;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly Covid19ApiClient _covid19ApiClient;
        private Timer _timer;

        public TimedHostedService(
            CovidClient covidClient,
            StocksClient stockClient,
            Covid19ApiClient covid19ApiClient,
            ILogger<TimedHostedService> logger)
        {
            _covidClient = covidClient;
            _stockClient = stockClient;
            _covid19ApiClient = covid19ApiClient;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(new TimerCallback(_ => { DoWork().Wait(); }), null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
        #endregion
    }
}
