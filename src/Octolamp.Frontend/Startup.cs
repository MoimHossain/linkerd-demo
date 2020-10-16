

using System;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Octolamp.Contracts.Settings;
using Octolamp.Frontend.Hubs;
using static Octolamp.Contracts.Protos.Covid;
using static Octolamp.Contracts.Protos.Stocks;

namespace Octolamp.Frontend
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

            services.AddSignalR();

            ConfigureGrpcServices(services);
        }

        private void ConfigureGrpcServices(IServiceCollection services)
        {
            services.Configure<BackendSettings>(Configuration.GetSection("Backend"));
            var configureAction = new Action<IServiceProvider, GrpcClientFactoryOptions>((provider, options) =>
            {
                var settings = provider.GetRequiredService<IOptionsMonitor<BackendSettings>>();
                options.Address = new Uri(settings.CurrentValue.Address);
                Console.WriteLine($"Backend address from configuration: {options.Address}");
            });
            services.AddGrpcClient<CovidClient>(configureAction);
            services.AddGrpcClient<StocksClient>(configureAction);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}