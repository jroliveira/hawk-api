namespace Hawk.Infrastructure.Data.Neo4J
{
    public sealed class Neo4JConfiguration
    {
        public string? Protocol { get; set; }

        public string? Host { get; set; }

        public int? Port { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string Uri => $"{this.Protocol}://{this.Host}:{this.Port}";

        public void Deconstruct(
            out string? protocol,
            out string? host,
            out int? port,
            out string? username,
            out string? password) => (protocol, host, port, username, password) = (
                this.Protocol ?? "bolt",
                this.Host ?? "localhost",
                this.Port.GetValueOrDefault(7687),
                this.Username ?? "neo4j",
                this.Password ?? "neo4j");
    }
}
