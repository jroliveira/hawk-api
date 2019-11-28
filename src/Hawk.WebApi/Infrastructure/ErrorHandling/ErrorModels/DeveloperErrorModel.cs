namespace Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels
{
    using System;

    public sealed class DeveloperErrorModel : ErrorModel
    {
        public DeveloperErrorModel(Exception? exception)
            : base(exception)
        {
            if (exception == null)
            {
                return;
            }

            this.StackTrace = exception.StackTrace;
            this.InnerDeveloperError = new DeveloperErrorModel(exception.InnerException);
        }

        public string? StackTrace { get; }

        public DeveloperErrorModel? InnerDeveloperError { get; }
    }
}
