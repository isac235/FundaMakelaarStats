namespace FundaMakelaarStats
{
    using FundaMakelaarStats.APIClients;
    using FundaMakelaarStats.Configs;
    using FundaMakelaarStats.Services;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;

    public class App
    {
        private readonly IMakelaarService _makelaarService;

        private readonly IPrinter _printer;

        public App(IMakelaarService makelaarService, IPrinter printer)
        {
            _makelaarService = makelaarService;
            _printer = printer;
        }

        public async Task RunAsync() {

            var responseForAmsterdam = await _makelaarService.GetMakelaarsOffersInfo();

            Console.WriteLine("Top 10 Makelaars in Amsterdam");
            _printer.PrintTop10Makelaars(responseForAmsterdam);

            var responseForAmsterdamWithGardenFilter = await _makelaarService.GetMakelaarsOffersInfo(true);

            Console.WriteLine("Top 10 Makelaars in Amsterdam with Garden Filter");
            _printer.PrintTop10Makelaars(responseForAmsterdamWithGardenFilter);
        }
    }
}
