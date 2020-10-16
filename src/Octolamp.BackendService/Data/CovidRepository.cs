

using Google.Protobuf.WellKnownTypes;
using Octolamp.Contracts.Extensions;
using Octolamp.Contracts.Protos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octolamp.BackendService.Data
{
    public class CovidRepository
    {
        private static CovidGlobalReport _globalReport = new CovidGlobalReport();
        private static ConcurrentDictionary<string, CovidCountryReport> _countryList = new ConcurrentDictionary<string, CovidCountryReport>();

        public CovidRepository()
        {
            _globalReport.Date = DateTime.MinValue.ToUniversalTime().ToProtoDateTime();
        }

        public async Task RegisterGlobalAsync(CovidGlobalReport gr)
        {
            _globalReport.Date = gr.Date;
            _globalReport.NewConfirmed = gr.NewConfirmed;
            _globalReport.TotalConfirmed = gr.TotalConfirmed;
            _globalReport.NewDeaths = gr.NewDeaths;
            _globalReport.TotalDeaths = gr.TotalDeaths;
            _globalReport.NewRecovered = gr.NewRecovered;
            _globalReport.TotalRecovered = gr.TotalRecovered;

            await Task.CompletedTask;
        }

        public async Task RegisterCountryAsync(CovidCountryReport country)
        {
            _countryList.TryGetValue(country.Slug, out var entry);

            if (entry == null)
            {
                entry = new CovidCountryReport();
            }

            if(entry != null )
            {
                entry.Date = country.Date;
                entry.NewConfirmed = country.NewConfirmed;
                entry.TotalConfirmed = country.TotalConfirmed;
                entry.NewDeaths = country.NewDeaths;
                entry.TotalDeaths = country.TotalDeaths;
                entry.NewRecovered = country.NewRecovered;
                entry.TotalRecovered = country.TotalRecovered;
                _countryList.TryAdd(country.Slug, entry);
            }   
            await Task.CompletedTask;
        }

    }
}
