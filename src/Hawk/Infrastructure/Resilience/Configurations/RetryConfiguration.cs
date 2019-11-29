namespace Hawk.Infrastructure.Resilience.Configurations
{
    using static Hawk.Infrastructure.Logging.Logger;

    public sealed class RetryConfiguration
    {
        public int? Count { get; set; }

        public int? TimeInMs { get; set; }

        public void Deconstruct(out int count, out int timeInMs) => (count, timeInMs) = (
            this.Count.GetValueOrDefault(3),
            this.TimeInMs.GetValueOrDefault(1000));

        public bool IsEnabled()
        {
            if (this.Count.HasValue && this.TimeInMs.HasValue)
            {
                return true;
            }

            LogError("Retry configuration is not valid.", this);

            return false;
        }
    }
}
