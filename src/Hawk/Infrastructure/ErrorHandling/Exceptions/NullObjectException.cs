namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class NullObjectException : BaseException
    {
        public NullObjectException(string message)
            : base(message)
        {
        }
    }
}
