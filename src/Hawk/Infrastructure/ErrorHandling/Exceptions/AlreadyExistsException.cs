namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class AlreadyExistsException : HawkException
    {
        public AlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
