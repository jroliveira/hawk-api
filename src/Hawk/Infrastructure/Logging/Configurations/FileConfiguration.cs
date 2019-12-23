namespace Hawk.Infrastructure.Logging.Configurations
{
    public sealed class FileConfiguration
    {
        public bool? Enabled { get; set; }

        public string? Path { get; set; }

        public void Deconstruct(out bool enabled, out string path) => (enabled, path) = (
            this.Enabled.GetValueOrDefault(false),
            this.Path ?? "logs/.log");
    }
}
