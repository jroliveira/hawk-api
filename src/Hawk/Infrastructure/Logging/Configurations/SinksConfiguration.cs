namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class SinksConfiguration
    {
        public ConsoleConfiguration? Console { get; set; }

        public FileConfiguration? File { get; set; }

        public ElasticSearchConfiguration? ElasticSearch { get; set; }

        public TracingConfiguration? Tracing { get; set; }

        public void Deconstruct(
            out ConsoleConfiguration console,
            out FileConfiguration file,
            out ElasticSearchConfiguration elasticSearch,
            out TracingConfiguration tracing) => (console, file, elasticSearch, tracing) = (
                this.Console ?? new ConsoleConfiguration(),
                this.File ?? new FileConfiguration(),
                this.ElasticSearch ?? new ElasticSearchConfiguration(),
                this.Tracing ?? new TracingConfiguration());
    }
}
