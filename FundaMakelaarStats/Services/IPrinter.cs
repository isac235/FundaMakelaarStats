namespace FundaMakelaarStats.Services
{
    using FundaMakelaarStats.Models.ViewModels;
    using System.Collections.Generic;

    public interface IPrinter
    {
        void PrintTop10Makelaars(Dictionary<int, MakelaarsOffers> makelaarsOffers);
    }
}
