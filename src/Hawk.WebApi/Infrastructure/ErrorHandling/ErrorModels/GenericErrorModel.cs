namespace Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels
{
    using System;

    using Hawk.Domain.Shared.Exceptions;

    using Microsoft.AspNetCore.Hosting;

    public sealed class GenericErrorModel : ErrorModel
    {
        public GenericErrorModel(HawkException exception)
            : base(exception)
        {
        }

        public GenericErrorModel(Exception exception)
            : this(exception, default)
        {
        }

        public GenericErrorModel(Exception exception, IHostingEnvironment environment)
            : base("An error has occurred.")
        {
            if (environment != null && environment.IsDevelopment())
            {
                this.DeveloperError = new DeveloperErrorModel(exception);
            }
        }

        public DeveloperErrorModel DeveloperError { get; }
    }
}
