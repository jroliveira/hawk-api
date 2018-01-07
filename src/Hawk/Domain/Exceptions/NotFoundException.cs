namespace Hawk.Domain.Exceptions
{
    using System;

    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
