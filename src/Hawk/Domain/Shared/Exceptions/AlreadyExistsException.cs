namespace Hawk.Domain.Shared.Exceptions
{
    public sealed class AlreadyExistsException : HawkException
    {
        public AlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
