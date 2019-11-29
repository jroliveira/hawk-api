namespace Hawk.Infrastructure.Logging.Methods
{
    public interface ILogMethod
    {
        void Write(LogLevel logLevel, string data);
    }
}
