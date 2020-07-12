namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class UnauthorizedException : BaseException
    {
        public UnauthorizedException(in string message)
            : base(message)
        {
        }
    }
}
