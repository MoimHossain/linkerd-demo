

using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Octolamp.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using static Octolamp.Contracts.Protos.Covid;
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
            services.Configure<AzureSettings>(Configuration.GetSection("Azure"));

            var configureAction = new Action<IServiceProvider, GrpcClientFactoryOptions>((provider, options) =>
            {
                var settings = provider.GetRequiredService<IOptionsMonitor<BackendSettings>>();
                options.Address = new Uri(settings.CurrentValue.Address);
                Console.WriteLine($"Backend address from configuration: {options.Address}");
            });

            services.AddSingleton(provider => 
            {
                var configSettings = provider.GetRequiredService<IOptionsMonitor<AzureSettings>>();
                var serviceManager = new ServiceManagerBuilder()
                     .WithOptions(option =>
                     {
                         option.ConnectionString = configSettings.CurrentValue.SignalR.ConnectionString;
                     })
                     .Build();
                return (IServiceManager)serviceManager;
            });
            services.AddSingleton(new HttpClient { BaseAddress = new Uri("https://api.covid19api.com") });
            services.AddSingleton<Covid19ApiClient>();
            services.AddGrpcClient<CovidClient>(configureAction);
            services.AddGrpcClient<StocksClient>(configureAction);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
