namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class ElasticSearchConfiguration
    {
        public bool? Enabled { get; set; }

        public string? Protocol { get; set; }

        public string? Host { get; set; }

        public int? Port { get; set; }

        public string Uri => $"{this.Protocol}://{this.Host}:{this.Port}";

        public void Deconstruct(
            out bool enabled,
            out string protocol,
            out string host,
            out int port) => (enabled, protocol, host, port) = (
                this.Enabled.GetValueOrDefault(false),
                this.Protocol ?? "http",
                this.Host ?? "localhost",
                this.Port.GetValueOrDefault(9200));
    }
}
