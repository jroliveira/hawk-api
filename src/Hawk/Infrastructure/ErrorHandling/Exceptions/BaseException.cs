namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    using System;
    using System.Collections;

    public abstract class BaseException : Exception
    {
        protected BaseException(in string message)
            : base(message)
        {
        }

        protected BaseException(in string message, in Exception innerException)
            : base(message, innerException)
        {
        }

        public override IDictionary? Data => null;

        public new int? HResult => null;
    }
}
