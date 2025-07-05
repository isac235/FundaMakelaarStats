namespace FundaMakelaarStats.Services
{
    using FundaMakelaarStats.Models.ViewModels;

    public interface IMakelaarService
    {
        Task<Dictionary<int, MakelaarsOffers>> GetMakelaarsOffersInfo(bool hasTuinFilter = false, CancellationToken cancellationToken = default);
    }
}
