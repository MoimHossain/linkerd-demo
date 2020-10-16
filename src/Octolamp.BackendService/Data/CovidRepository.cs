

using Google.Protobuf.WellKnownTypes;
using Octolamp.Contracts.Extensions;
using Octolamp.Contracts.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Octolamp.BackendService.Data
{
    public class CovidRepository
    {
        private static CovidGlobalReport _globalReport = new CovidGlobalReport() ;
        private static List<CovidCountryReport> _countries = new List<CovidCountryReport>();

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
            var existingEntry = _countries
                .FirstOrDefault(c => c.Slug.Equals(country.Slug, StringComparison.OrdinalIgnoreCase));

            if (existingEntry == null)
            {
                existingEntry = new CovidCountryReport();
                _countries.Add(existingEntry);
            }
            existingEntry.Date = country.Date;
            existingEntry.NewConfirmed = country.NewConfirmed;
            existingEntry.TotalConfirmed = country.TotalConfirmed;
            existingEntry.NewDeaths = country.NewDeaths;
            existingEntry.TotalDeaths = country.TotalDeaths;
            existingEntry.NewRecovered = country.NewRecovered;
            existingEntry.TotalRecovered = country.TotalRecovered;
            await Task.CompletedTask;
        }

    }
}
