

using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octolamp.Contracts.Extensions;
using Octolamp.Contracts.Protos;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Octolamp.Contracts.Protos.Stocks;

namespace Octolamp.DaemonService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private async Task DoWork()
        {
            var count = Interlocked.Increment(ref executionCount);

            var tasks = Enumerable.Range(1, 100).Select(GetAsync);
            var stocks = await Task.WhenAll(tasks);

            _logger.LogInformation($"Timed Hosted Service is working. Count: {count}, number of Stocks {stocks.Count()}" );
        }

        private async Task<Tuple<Stock, string>> GetAsync(int id)
        {
            try
            {
                var response = _stockClient.GetAsync(new GetRequest { Id = id + 1 });
                var getResponse = await response.ResponseAsync;
                var headers = await response.ResponseHeadersAsync;
                return new Tuple<Stock, string>(getResponse.Stock, headers.GetString("server-id"));
            }
            catch (RpcException e) when (e.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                _logger.LogWarning("Stock {id} not found", id);
                return null;
            }
            catch (RpcException e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }


        #region Service methods
        private int executionCount = 0;
        private readonly StocksClient _stockClient;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;

        public TimedHostedService(StocksClient stockClient, ILogger<TimedHostedService> logger)
        {
            _stockClient = stockClient;
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
