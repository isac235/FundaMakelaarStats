namespace FundaMakelaarStats.APIClients
{
    using FundaMakelaarStats.Models.ApiResponse;

    public interface IFundaApiClient
    {
        Task<FundaApiResponse> GetOffers(string url, CancellationToken cancellationToken = default);
    }
}
