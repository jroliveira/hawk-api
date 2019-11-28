namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class SinksConfiguration
    {
        public ConsoleConfiguration? Console { get; set; }

        public FileConfiguration? File { get; set; }

        public ElasticSearchConfiguration? ElasticSearch { get; set; }
    }
}
