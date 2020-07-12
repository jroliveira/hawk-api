namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    using System;

    public sealed class InternalException : BaseException
    {
        public InternalException(in string message)
            : base(message)
        {
        }

        public InternalException(in string message, in Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
