namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class NullObjectException : BaseException
    {
        public NullObjectException(in string message)
            : base(message)
        {
        }
    }
}
