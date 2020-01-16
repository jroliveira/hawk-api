namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    using System;
    using System.Collections;

    public abstract class BaseException : Exception
    {
        protected BaseException(string message)
            : base(message)
        {
        }

        protected BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public override IDictionary? Data => null;

        public new int? HResult => null;
    }
}
