namespace FundaMakelaarStats.Models.ApiResponse
{
    public record Paging(
        int AantalPaginas,
        int HuidigePagina,
        string VolgendeUrl,
        string VorigeUrl
        );
}
