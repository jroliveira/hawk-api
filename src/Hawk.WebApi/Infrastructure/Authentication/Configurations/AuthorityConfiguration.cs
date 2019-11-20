namespace Hawk.WebApi.Infrastructure.Authentication.Configurations
{
    internal sealed class AuthorityConfiguration
    {
        public string Protocol { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Uri => $"{this.Protocol}://{this.Host}:{this.Port}";
    }
}
