

using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octolamp.Contracts.Dtos;
using Octolamp.Contracts.Extensions;
using Octolamp.Contracts.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Octolamp.Contracts.Protos.Covid;
using static Octolamp.Contracts.Protos.Stocks;

namespace Octolamp.DaemonService
{
    public class LocationTracker : IHostedService, IDisposable
    {
        private async Task DoWork()
        {
            try
            {
                var allLocations = new List<RawCovidLocation>();
                var hubContext = await _signalRServiceManager.CreateHubContextAsync("notificationhub");
                
                var locations = await _covid19ApiClient.GetAllCovidLatLongAsync();
                foreach (var loc in locations.Where(l=> l.Latitude.HasValue && l.Longitude.HasValue))
                {
                    await hubContext.Clients.All.SendCoreAsync("ReceiveMessage", new object[] { loc });
                }
                await hubContext.DisposeAsync();             
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }


        #region Service methods
        private readonly CovidClient _covidClient;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly Covid19ApiClient _covid19ApiClient;
        private readonly IServiceManager _signalRServiceManager;
        private Timer _timer;

        public LocationTracker(
            CovidClient covidClient,
            Covid19ApiClient covid19ApiClient,
            IServiceManager serviceManagerBuilder,
            ILogger<TimedHostedService> logger)
        {
            _covidClient = covidClient;
            _covid19ApiClient = covid19ApiClient;
            _signalRServiceManager = serviceManagerBuilder;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(new TimerCallback(_ => { DoWork().Wait(); }), null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60 * 5));
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
