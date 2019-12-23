namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class NotFoundException : HawkException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
