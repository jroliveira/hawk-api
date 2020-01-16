namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    using System;

    public sealed class InternalException : BaseException
    {
        public InternalException(string message)
            : base(message)
        {
        }

        public InternalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
