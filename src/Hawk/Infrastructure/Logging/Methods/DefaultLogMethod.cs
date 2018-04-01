namespace Hawk.Infrastructure.Logging.Methods
{
    using Serilog;

    public sealed class DefaultLogMethod : ILogMethod
    {
        private readonly ILogger logger;

        public DefaultLogMethod(string path)
        {
            this.logger = new LoggerConfiguration()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, outputTemplate: "{Message}{NewLine}")
                .CreateLogger();
        }

        public void Write(string data)
        {
            this.logger.Information(data);
        }
    }
}