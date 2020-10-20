

using Microsoft.Extensions.Logging;
using Octolamp.Contracts.Dtos;
using Octolamp.DaemonService.Supports;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Octolamp.DaemonService
{
    public class Covid19ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Covid19ApiClient> _logger;

        public Covid19ApiClient(HttpClient httpClient, ILogger<Covid19ApiClient> logger)
        {
            this._httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<List<RawCovidLocation>> GetAllCovidLatLongAsync()
        {
            var response = await _httpClient.SendAsync(new HttpRequestMessage(
                HttpMethod.Get, 
                "https://cloudoven.blob.core.windows.net/covid/CovidData.json"));
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Covid19 API invoked and request was successful");
                var value = await response.Content.ReadContentAsync<List<RawCovidLocation>>();
                return value;
            }
            return default(List<RawCovidLocation>);
        }

        public async Task<RawCovidReport> GetSummaryAsync()
        {
            var response = await _httpClient.GetAsync("summary");

            if(response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Covid19 API invoked and request was successful");
                var value = await response.Content.ReadContentAsync<RawCovidReport>();
                return value;
            }
            return default(RawCovidReport);
        }
    }
}
