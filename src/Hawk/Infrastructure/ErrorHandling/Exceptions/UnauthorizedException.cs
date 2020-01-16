namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
