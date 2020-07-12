namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    using System;

    public sealed class DeveloperException : BaseException
    {
        public DeveloperException(in Exception exception)
            : base(exception.Message)
        {
            this.StackTrace = exception.StackTrace;

            if (exception.InnerException != default)
            {
                this.InnerDeveloperException = new DeveloperException(exception.InnerException);
            }
        }

        public new string? StackTrace { get; }

        public DeveloperException? InnerDeveloperException { get; }
    }
}
