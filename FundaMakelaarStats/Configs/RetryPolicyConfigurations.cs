namespace FundaMakelaarStats.Configs
{
    public class RetryPolicyConfigurations
    {
        public int RetryCount { get; set; }

        public int InitialBackoffSeconds { get; set; }

        public int BackoffMultiplier { get; set; }
    }
}
