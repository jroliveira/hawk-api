namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class InvalidRequestException : HawkException
    {
        public InvalidRequestException(string message)
            : base(message)
        {
        }
    }
}
