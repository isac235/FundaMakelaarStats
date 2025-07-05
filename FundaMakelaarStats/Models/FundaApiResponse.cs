namespace FundaMakelaarStats.Models
{
    public record FundaApiResponse(
        int AccountStatus,
        bool EmailNotConfirmed,
        bool ValidationFailed,
        Metadata Metadata,
        List<RealEstateObject> Objects,
        Paging Paging,
        int TotaalAantalObjecten
        );
}
