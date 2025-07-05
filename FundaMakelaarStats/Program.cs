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
                     // Register options
                     services.Configure<FundaApiConfigurations>(
                         context.Configuration.GetSection(nameof(FundaApiConfigurations)));

                     // Register HttpClient and FundaApiClient
                     services.AddHttpClient<IFundaApiClient, FundaApiClient>()
                    .AddPolicyHandler(HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests || msg.StatusCode == HttpStatusCode.Unauthorized)
                        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (outcome, timespan, retryAttempt, context) =>
                            {
                                Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to {outcome.Result?.StatusCode}");
                            }));

                     // Register other services
                     services.AddSingleton<IFundaJsonSerializer, FundaJsonSerializer>();
                     services.AddTransient<App>(); //app entry point
                     services.AddScoped<IMakelaarService, MakelaarService>();
                     services.AddScoped<IPrinter, Printer>();
                 })
                 .Build();

            // Run the app
            var app = host.Services.GetRequiredService<App>();

            var cancellationToken = host.Services
                .GetRequiredService<IHostApplicationLifetime>()
                .ApplicationStopping;

            await app.RunAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
