
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Octolamp.Contracts.Extensions;
using Octolamp.Contracts.Protos;
using Octolamp.Frontend.Internal;
using Octolamp.Frontend.Models;
using static Octolamp.Contracts.Protos.Covid;

namespace Octolamp.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly Stocks.StocksClient _stocksClient;
        private readonly ILogger<HomeController> _logger;
        private readonly CovidClient covidClient;

        public HomeController(
            ILogger<HomeController> logger,
            CovidClient covidClient,
            Stocks.StocksClient stocksClient)
        {
            _stocksClient = stocksClient;
            _logger = logger;
            this.covidClient = covidClient;
        }

        public async Task<List<CovidCountryReport>> GetAllCountry()
        {
            var data = await covidClient.GetAllCountryReportAsync(new HandshakeRequest { ClientToken = Guid.NewGuid().ToString() });

            return  data.Countries.OrderByDescending(r=> r.NewConfirmed).ToList();
        }

        public async Task<CovidGlobalReport> GetGlobal()
        {
            var response = await covidClient
                .GetGlobalReportAsync(new HandshakeRequest { ClientToken = Guid.NewGuid().ToString() });
            return response;
        }

        public async Task<IActionResult> Index()
        {
            await Task.CompletedTask;
            return View(new IndexViewModel(new List<StockViewModel>()));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}