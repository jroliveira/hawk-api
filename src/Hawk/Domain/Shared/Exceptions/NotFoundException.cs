namespace Hawk.Domain.Shared.Exceptions
{
    public sealed class NotFoundException : HawkException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
