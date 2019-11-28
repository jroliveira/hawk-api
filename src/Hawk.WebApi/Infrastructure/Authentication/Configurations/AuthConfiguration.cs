namespace Hawk.WebApi.Infrastructure.Authentication.Configurations
{
    internal sealed class AuthConfiguration
    {
        public bool? Enabled { get; set; }

        public AuthorityConfiguration? Authority { get; set; }
    }
}
