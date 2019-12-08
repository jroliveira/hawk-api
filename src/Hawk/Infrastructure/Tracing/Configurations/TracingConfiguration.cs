namespace Hawk.Infrastructure.Tracing.Configurations
{
    using static Hawk.Infrastructure.Logging.Logger;

    public sealed class TracingConfiguration
    {
        public bool? Enabled { get; set; }

        public JaegerConfiguration? Jaeger { get; set; }

        public bool IsEnabled()
        {
            if (!this.Enabled.GetValueOrDefault(false))
            {
                return false;
            }

            if (this.Jaeger != null && this.Jaeger.IsEnabled())
            {
                return true;
            }

            LogError("Tracing configuration is not valid.", this);

            return false;
        }
    }
}
