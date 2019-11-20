namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class SinksConfiguration
    {
        public FileConfiguration File { get; set; }

        public ElasticSearchConfiguration ElasticSearch { get; set; }
    }
}
