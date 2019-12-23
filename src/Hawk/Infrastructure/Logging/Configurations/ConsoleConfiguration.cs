namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class ConsoleConfiguration
    {
        public bool? Enabled { get; set; }

        public void Deconstruct(out bool enabled) => enabled = this.Enabled.GetValueOrDefault(false);
    }
}
