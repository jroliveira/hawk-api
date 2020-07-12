namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    public sealed class BusinessException : BaseException
    {
        public BusinessException(in string message)
            : base(message)
        {
        }
    }
}
