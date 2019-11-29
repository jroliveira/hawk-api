namespace Hawk.Domain.Shared.Exceptions
{
    public sealed class InvalidRequestException : HawkException
    {
        public InvalidRequestException(string message)
            : base(message)
        {
        }
    }
}
