namespace Hawk.Infrastructure.Resilience.Configurations
{
    using static Hawk.Infrastructure.Logging.Logger;

    public sealed class ResilienceConfiguration
    {
        public bool? Enabled { get; set; }

        public RetryConfiguration? Retry { get; set; }

        public void Deconstruct(out bool enabled, out RetryConfiguration retry) => (enabled, retry) = (
            this.Enabled.GetValueOrDefault(false),
            this.Retry ?? new RetryConfiguration());

        internal bool IsEnabled()
        {
            if (this.Enabled.GetValueOrDefault(false))
            {
                return true;
            }

            if (this.Retry != null && this.Retry.IsEnabled())
            {
                return true;
            }

            LogError("Resilience configuration is not valid", this);

            return false;
        }
    }
}
