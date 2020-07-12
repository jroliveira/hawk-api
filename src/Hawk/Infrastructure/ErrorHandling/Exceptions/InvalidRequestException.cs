namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class InvalidRequestException : BaseException
    {
        public InvalidRequestException(in string message)
            : base(message)
        {
        }
    }
}
