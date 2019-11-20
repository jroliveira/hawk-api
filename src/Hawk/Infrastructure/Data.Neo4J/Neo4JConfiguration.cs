namespace Hawk.Infrastructure.Data.Neo4J
{
    public sealed class Neo4JConfiguration
    {
        public string Protocol { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Uri => $"{this.Protocol}://{this.Host}:{this.Port}";
    }
}
