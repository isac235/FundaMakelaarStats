namespace FundaMakelaarStats.Utils
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class FundaJsonSerializer : IFundaJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public FundaJsonSerializer()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T> DeserializeAsync<T>(Stream jsonStream, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<T>(jsonStream, _options, cancellationToken) ?? throw new InvalidOperationException("Deserialization returned null");
        }
    }
}
