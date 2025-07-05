namespace FundaMakelaarStats.Models
{
    public record Paging(
        int AantalPaginas,
        int HuidigePagina,
        string VolgendeUrl,
        string VorigeUrl
        );
}
