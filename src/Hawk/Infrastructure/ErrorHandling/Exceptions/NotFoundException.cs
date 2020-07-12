namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class NotFoundException : BaseException
    {
        public NotFoundException(in string message)
            : base(message)
        {
        }
    }
}
