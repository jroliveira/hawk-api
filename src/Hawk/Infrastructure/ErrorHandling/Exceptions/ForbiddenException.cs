namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class ForbiddenException : BaseException
    {
        public ForbiddenException(in string message)
            : base(message)
        {
        }
    }
}
