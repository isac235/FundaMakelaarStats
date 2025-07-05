namespace FundaMakelaarStats.Models
{
    public record Prijs(
        bool GeenExtraKosten,
        string HuurAbbreviation,
        int? Huurprijs,
        string HuurprijsOpAanvraag,
        int? HuurprijsTot,
        string KoopAbbreviation,
        int? Koopprijs,
        string KoopprijsOpAanvraag,
        int? KoopprijsTot,
        object OriginelePrijs,
        string VeilingText
        );
}
