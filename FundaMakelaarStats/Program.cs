namespace FundaMakelaarStats
{
    using FundaMakelaarStats.APIClients;
    using FundaMakelaarStats.Configs;
    using FundaMakelaarStats.Services;
    using FundaMakelaarStats.Utils;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Polly;
    using Polly.Extensions.Http;
    using System.Net;

    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddFundaApiServices(context.Configuration);
                    services.AddApplicationServices();
                })
                .Build();

            var app = host.Services.GetRequiredService<App>();
            var cancellationToken = host.Services
                .GetRequiredService<IHostApplicationLifetime>()
                .ApplicationStopping;

            await app.RunAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}