namespace FundaMakelaarStats.Utils
{
    public interface IFundaJsonSerializer
    {
        Task<T> DeserializeAsync<T>(Stream jsonStream, CancellationToken cancellationToken = default);
    }
}
