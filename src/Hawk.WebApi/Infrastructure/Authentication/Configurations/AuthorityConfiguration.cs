namespace Hawk.WebApi.Infrastructure.Authentication.Configurations
{
    using static System.String;

    using static Hawk.Infrastructure.Logging.Logger;

    internal sealed class AuthorityConfiguration
    {
        public string? Protocol { get; set; }

        public string? Host { get; set; }

        public int? Port { get; set; }

        public string Uri => $"{this.Protocol}://{this.Host}:{this.Port}";

        public bool IsEnabled()
        {
            if (!IsNullOrEmpty(this.Protocol) && !IsNullOrEmpty(this.Host) && this.Port.HasValue)
            {
                return true;
            }

            LogError("Authority configuration is not valid.", this);

            return false;
        }
    }
}
