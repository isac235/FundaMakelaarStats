namespace FundaMakelaarStats.Services
{
    using FundaMakelaarStats.APIClients;
    using FundaMakelaarStats.Configs;
    using FundaMakelaarStats.Models;
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

        public async Task<Dictionary<int, MakelaarsOffers>> GetMakelaarsOffersInfo(bool hasTuinFilter = false)
        {
            var pageCounter = 1;
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
                var url = $"{_configurations.BaseUrl}/{_configurations.FeedEndpoint}/{_configurations.ApiKey}/?type={_configurations.Type}&zo=/{_configurations.SearchCommand}{tuinFilter}/&page={pageCounter}&pagesize={_configurations.PageSize}";
                var response = await _apiClient.GetOffers(url);
                listOfObjects.AddRange(response.Objects);
                numberOfObjects = response.Objects.Count;
                pageCounter++;

                await Task.Delay(_configurations.DelayBetweenRequestsMs); // Wait to avoid hitting the limit (650ms ~92 req/min, under the limit of 100 per minute)
            }

            foreach (var offer in listOfObjects)
            {
                int id = offer.MakelaarId;
                string name = offer.MakelaarNaam;

                if (result.ContainsKey(id))
                {
                    // Increment the count
                    var existing = result[id];
                    result[id] = existing with { NumberOfOffers = existing.NumberOfOffers + 1 };
                }
                else
                {
                    // New makelaar entry
                    result[id] = new MakelaarsOffers(1, name);
                }
            }

            return result;
        }
    }
}
