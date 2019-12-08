namespace Hawk.Infrastructure.Tracing.Configurations
{
    using static System.String;

    using static Hawk.Infrastructure.Logging.Logger;

    public sealed class JaegerConfiguration
    {
        public string? AgentHost { get; set; }

        public int? AgentPort { get; set; }

        public bool IsEnabled()
        {
            if (!IsNullOrEmpty(this.AgentHost)
                && this.AgentPort.HasValue)
            {
                return true;
            }

            LogError("Jaeger configuration is not valid.", this);

            return false;
        }
    }
}
