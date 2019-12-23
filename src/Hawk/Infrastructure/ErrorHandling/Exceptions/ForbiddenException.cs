namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class ForbiddenException : HawkException
    {
        public ForbiddenException(string message)
            : base(message)
        {
        }
    }
}
