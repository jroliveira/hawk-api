namespace Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels
{
    using System;

    public abstract class ErrorModel
    {
        protected ErrorModel(Exception exception)
            : this(exception?.Message)
        {
        }

        protected ErrorModel(string message) => this.Message = message;

        public string Message { get; }
    }
}
