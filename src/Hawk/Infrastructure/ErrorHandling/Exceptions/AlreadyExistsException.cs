namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class AlreadyExistsException : BaseException
    {
        public AlreadyExistsException(in string message)
            : base(message)
        {
        }
    }
}
