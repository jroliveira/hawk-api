namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class NullParameterException : BaseException
    {
        public NullParameterException(string message, string parameter)
            : base(message) => this.Parameter = parameter;

        public string Parameter { get; }
    }
}
