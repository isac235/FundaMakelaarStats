namespace FundaMakelaarStats.APIClients
{
    using FundaMakelaarStats.Models;
    using FundaMakelaarStats.Utils;

    public class FundaApiClient : IFundaApiClient
    {
        private readonly HttpClient _httpClient;

        private readonly IFundaJsonSerializer _jsonSerializer;

        public FundaApiClient(HttpClient httpClient, IFundaJsonSerializer jsonSerializer)
        {
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }
        public async Task<FundaApiResponse> GetOffers(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            return await _jsonSerializer.DeserializeAsync<FundaApiResponse>(stream);
        }
    }
}
