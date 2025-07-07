namespace FundaMakelaarStats
{
    using FundaMakelaarStats.APIClients;
    using FundaMakelaarStats.Configs;
    using FundaMakelaarStats.Services;
    using FundaMakelaarStats.Utils;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Polly;
    using Polly.Extensions.Http;
    using System.Net;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFundaApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register options
            services.Configure<FundaApiConfigurations>(
                configuration.GetSection(nameof(FundaApiConfigurations)));

            // Register retry policy configuration
            services.Configure<RetryPolicyConfigurations>(
                configuration.GetSection(nameof(RetryPolicyConfigurations)));

            // Register HttpClient and FundaApiClient with retry policy
            services.AddHttpClient<IFundaApiClient, FundaApiClient>()
                .AddFundaRetryPolicy();

            services.AddSingleton<IFundaJsonSerializer, FundaJsonSerializer>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<App>(); // app entry point
            services.AddTransient<IMakelaarService, MakelaarService>();
            services.AddTransient<IPrinter, Printer>();

            return services;
        }

        public static IHttpClientBuilder AddFundaRetryPolicy(this IHttpClientBuilder builder)
        {
            return builder.AddPolicyHandler((services, request) =>
            {
                var retryConfig = services.GetRequiredService<IOptions<RetryPolicyConfigurations>>().Value;

                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests ||
                                     msg.StatusCode == HttpStatusCode.Unauthorized)
                    .WaitAndRetryAsync(
                        retryCount: retryConfig.RetryCount,
                        sleepDurationProvider: retryAttempt =>
                            TimeSpan.FromSeconds(retryConfig.InitialBackoffSeconds *
                                               Math.Pow(retryConfig.BackoffMultiplier, retryAttempt - 1)),
                        onRetry: (outcome, timespan, retryAttempt, context) =>
                        {
                            Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to {outcome.Result?.StatusCode}");
                        });
            });
        }
    }
}
