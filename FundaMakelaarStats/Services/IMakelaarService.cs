namespace FundaMakelaarStats.Services
{
    using FundaMakelaarStats.Models;

    public interface IMakelaarService
    {
        Task<Dictionary<int, MakelaarsOffers>> GetMakelaarsOffersInfo(bool hasTuinFilter = false);
    }
}
