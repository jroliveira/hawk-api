namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class LogConfiguration
    {
        public string? Level { get; set; }

        public SinksConfiguration? Sinks { get; set; }

        public void Deconstruct(out string level, out SinksConfiguration sinks) => (level, sinks) = (
            this.Level ?? "Warn",
            this.Sinks ?? new SinksConfiguration());
    }
}
