namespace Hawk.WebApi.Features.Shared.ErrorModels
{
    using System;

    public sealed class DeveloperErrorModel
    {
        public DeveloperErrorModel(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            this.Message = exception.Message;
            this.StackTrace = exception.StackTrace;
            this.InnerDeveloperError = new DeveloperErrorModel(exception.InnerException);
        }

        public string Message { get; }

        public string StackTrace { get; }

        public DeveloperErrorModel InnerDeveloperError { get; }

        public static implicit operator DeveloperErrorModel(Exception exception) => new DeveloperErrorModel(exception);
    }
}
