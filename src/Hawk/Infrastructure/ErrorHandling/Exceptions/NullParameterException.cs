namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class NullParameterException : BaseException
    {
        public NullParameterException(in string message, in string parameter)
            : base(message) => this.Parameter = parameter;

        public string Parameter { get; }
    }
}
