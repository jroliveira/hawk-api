namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    using System;

    public sealed class GenericException : BaseException
    {
        public GenericException(Exception exception, bool isDevelopment)
            : base("An error has occurred.")
        {
            if (isDevelopment)
            {
                this.DeveloperException = new DeveloperException(exception);
            }
        }

        public DeveloperException? DeveloperException { get; }
    }
}
