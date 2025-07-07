namespace FundaMakelaarStats.Services
{
    using FundaMakelaarStats.APIClients;
    using FundaMakelaarStats.Configs;
    using FundaMakelaarStats.Models.ApiResponse;
    using FundaMakelaarStats.Models.ViewModels;
    using Microsoft.Extensions.Options;

    public class MakelaarService : IMakelaarService
    {
        private readonly IFundaApiClient _apiClient;

        private readonly FundaApiConfigurations _configurations;

        public MakelaarService(IFundaApiClient apiClient, IOptions<FundaApiConfigurations> configurations)
        {
            _apiClient = apiClient;
            _configurations = configurations.Value;
        }

        public async Task<Dictionary<int, MakelaarsOffers>> GetMakelaarsOffersInfo(bool hasTuinFilter = false, CancellationToken cancellationToken = default)
        {
            var pageCounter = 0;
            var numberOfObjects = _configurations.PageSize;
            var listOfObjects = new List<RealEstateObject>();
            var result = new Dictionary<int, MakelaarsOffers>();

            // If the tuin filter is applied, we need to adjust the search command
            var tuinFilter = string.Empty;
            if (hasTuinFilter)
            {
                tuinFilter = "/tuin";
            }

            while (numberOfObjects >= _configurations.PageSize)
            {
                var url = BuildUrl(pageCounter, tuinFilter);
                var response = await _apiClient.GetOffers(url, cancellationToken).ConfigureAwait(false);
                numberOfObjects = response.Objects.Count;

                foreach (var offer in response.Objects)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    int id = offer.MakelaarId;
                    string name = offer.MakelaarNaam;

                    if (result.TryGetValue(id, out var existing))
                    {
                        result[id] = existing with { NumberOfOffers = existing.NumberOfOffers + 1 };
                    }
                    else
                    {
                        result[id] = new MakelaarsOffers(1, name);
                    }
                }
                pageCounter++;
                await Task.Delay(_configurations.DelayBetweenRequestsMs, cancellationToken).ConfigureAwait(false);
            }

            return result;
        }

        private string BuildUrl(int page, string tuinFilter)
        {
            return $"{_configurations.BaseUrl}/{_configurations.FeedEndpoint}/{_configurations.ApiKey}/?type={_configurations.Type}&zo=/{_configurations.SearchCommand}{tuinFilter}/&page={page}&pagesize={_configurations.PageSize}";
        }
    }
}
