namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class UnauthorizedException : HawkException
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
