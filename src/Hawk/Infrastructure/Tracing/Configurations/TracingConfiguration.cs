namespace Hawk.Infrastructure.Tracing.Configurations
{
    using static Hawk.Infrastructure.Logging.Logger;

    public sealed class TracingConfiguration
    {
        public bool? Enabled { get; set; }

        public JaegerConfiguration? Jaeger { get; set; }

        public void Deconstruct(out bool enabled, out JaegerConfiguration jaeger) => (enabled, jaeger) = (
            this.Enabled.GetValueOrDefault(false),
            this.Jaeger ?? new JaegerConfiguration());

        public bool IsEnabled()
        {
            if (!this.Enabled.GetValueOrDefault(false))
            {
                return false;
            }

            if (this.Jaeger != default && this.Jaeger.IsEnabled())
            {
                return true;
            }

            LogError("Tracing configuration is not valid.", this);

            return false;
        }
    }
}
