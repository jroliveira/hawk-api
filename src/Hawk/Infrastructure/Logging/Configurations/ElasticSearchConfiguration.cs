namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class ElasticSearchConfiguration
    {
        public bool Enabled { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Uri => $"{this.Protocol}://{this.Host}:{this.Port}";
    }
}
