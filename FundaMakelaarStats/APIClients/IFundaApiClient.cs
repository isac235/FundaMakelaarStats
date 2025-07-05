namespace FundaMakelaarStats.APIClients
{
    using FundaMakelaarStats.Models;

    public interface IFundaApiClient
    {
        Task<FundaApiResponse> GetOffers(string url);
    }
}
