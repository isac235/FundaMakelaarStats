namespace FundaMakelaarStats.Configs
{
    public class FundaApiConfigurations
    {
        public string BaseUrl { get; set; } = default!;

        public string FeedEndpoint { get; set; } = default!;

        public string ApiKey { get; set; } = default!;

        public string Type { get; set; } = "koop";

        public string SearchCommand { get; set; } = "amsterdam";

        public int PageSize { get; set; } = 25;

        public int DelayBetweenRequestsMs { get; set; } = 650;
    }
}
