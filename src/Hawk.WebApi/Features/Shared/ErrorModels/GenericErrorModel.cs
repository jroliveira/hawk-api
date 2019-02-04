namespace Hawk.WebApi.Features.Shared.ErrorModels
{
    using System;

    using Hawk.Domain.Shared.Exceptions;

    using Microsoft.AspNetCore.Hosting;

    public sealed class GenericErrorModel
    {
        public GenericErrorModel(HawkException exception) => this.Message = exception.Message;

        public GenericErrorModel(Exception exception)
            : this(exception, default)
        {
        }

        public GenericErrorModel(Exception exception, IHostingEnvironment environment)
        {
            this.Message = "An error has occurred.";

            if (environment != null && environment.IsDevelopment())
            {
                this.DeveloperError = exception;
            }
        }

        public string Message { get; }

        public DeveloperErrorModel DeveloperError { get; }

        internal static object HandleError(Exception exception)
        {
            switch (exception)
            {
                case NotFoundException notFound: return new GenericErrorModel(notFound);
                case InvalidObjectException somePropertyIsNull: return new ConflictErrorModel(somePropertyIsNull);
                case AlreadyExistsException alreadyExists: return new GenericErrorModel(alreadyExists);
                default: return new GenericErrorModel(exception);
            }
        }
    }
}
