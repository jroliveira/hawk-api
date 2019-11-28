namespace Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels
{
    using System;

    using Microsoft.Extensions.Hosting;

    public sealed class GenericErrorModel : ErrorModel
    {
        public GenericErrorModel(Exception exception)
            : this(exception, default)
        {
        }

        public GenericErrorModel(Exception exception, IHostEnvironment? environment)
            : base("An error has occurred.")
        {
            if (environment != null && environment.IsDevelopment())
            {
                this.DeveloperError = new DeveloperErrorModel(exception);
            }
        }

        public DeveloperErrorModel? DeveloperError { get; }
    }
}
