namespace FundaMakelaarStats.Services
{
    using FundaMakelaarStats.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Printer : IPrinter
    {
        public void PrintTop10Makelaars(Dictionary<int, MakelaarsOffers> makelaarsOffers)
        {
            Console.WriteLine("Top 10 Makelaars by Number of Offers");
            Console.WriteLine(new string('-', 67));
            Console.WriteLine("{0,-5} {1,-50} {2,10}", "#", "Makelaar", "Offers");
            Console.WriteLine(new string('-', 67));

            var top10 = makelaarsOffers
                .OrderByDescending(kvp => kvp.Value.NumberOfOffers)
                .Take(10)
                .Select((kvp, index) => new
                {
                    Rank = index + 1,
                    Name = kvp.Value.Name,
                    Offers = kvp.Value.NumberOfOffers
                });

            foreach (var item in top10)
            {
                Console.WriteLine("{0,-5} {1,-50} {2,10}", item.Rank, item.Name, item.Offers);
            }

            Console.WriteLine(new string('-', 67));
        }
    }
}
