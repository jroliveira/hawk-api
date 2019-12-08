namespace Hawk.WebApi.Infrastructure.Authentication.Configurations
{
    using static Hawk.Infrastructure.Logging.Logger;

    internal sealed class AuthConfiguration
    {
        public bool? Enabled { get; set; }

        public AuthorityConfiguration? Authority { get; set; }

        internal bool IsEnabled()
        {
            if (!this.Enabled.GetValueOrDefault(false))
            {
                return false;
            }

            if (this.Authority != null && this.Authority.IsEnabled())
            {
                return true;
            }

            LogError("Auth configuration is not valid.", this);

            return false;
        }
    }
}
