namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class LogConfiguration
    {
        public string? Level { get; set; }

        public SinksConfiguration? Sinks { get; set; }
    }
}
