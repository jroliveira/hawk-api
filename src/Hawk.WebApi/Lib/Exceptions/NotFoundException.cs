namespace Hawk.WebApi.Lib.Exceptions
{
    using System;

    internal sealed class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
