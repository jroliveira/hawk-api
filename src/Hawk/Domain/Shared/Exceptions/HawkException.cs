namespace Hawk.Domain.Shared.Exceptions
{
    using System;

    public abstract class HawkException : Exception
    {
        protected HawkException(string message)
            : base(message)
        {
        }
    }
}
