namespace Hawk.Infrastructure.Logging.Methods
{
    public interface ILogMethod
    {
        void Write(in LogLevel logLevel, in string data);
    }
}
