namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using System;

    public sealed class DeveloperErrorModel : ErrorModel
    {
        public DeveloperErrorModel(Exception exception)
            : base(exception.Message)
        {
            this.StackTrace = exception.StackTrace;

            if (exception.InnerException != default)
            {
                this.InnerDeveloperError = new DeveloperErrorModel(exception.InnerException);
            }
        }

        public string? StackTrace { get; }

        public DeveloperErrorModel? InnerDeveloperError { get; }
    }
}
