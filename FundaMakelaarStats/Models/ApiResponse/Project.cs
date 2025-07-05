namespace FundaMakelaarStats.Models.ApiResponse
{
    public record Project(
        int? AantalKamersTotEnMet,
        int? AantalKamersVan,
        int? AantalKavels,
        string Adres,
        string FriendlyUrl,
        string GewijzigdDatum,
        int? GlobalId,
        string HoofdFoto,
        bool IndIpix,
        bool IndPDF,
        bool IndPlattegrond,
        bool IndTop,
        bool IndVideo,
        Guid InternalId,
        int? MaxWoonoppervlakte,
        int? MinWoonoppervlakte,
        string Naam,
        string Omschrijving,
        List<object> OpenHuizen,
        string Plaats,
        object Prijs,
        string PrijsGeformatteerd,
        string PublicatieDatum,
        int? Type,
        object Woningtypen
        );
}
