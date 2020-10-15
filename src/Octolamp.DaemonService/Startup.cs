

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Octolamp.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using static Octolamp.Contracts.Protos.Stocks;

namespace Octolamp.DaemonService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<BackendSettings>(Configuration.GetSection("Backend"));

            services.AddGrpcClient<StocksClient>((provider, options) =>
            {
                var settings = provider.GetRequiredService<IOptionsMonitor<BackendSettings>>();
                options.Address = new Uri(settings.CurrentValue.Address);
                Console.WriteLine($"Backend address from configuration: {options.Address}");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
