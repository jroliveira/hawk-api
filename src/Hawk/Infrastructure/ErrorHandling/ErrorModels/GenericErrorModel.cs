namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using System;

    public sealed class GenericErrorModel : ErrorModel
    {
        public GenericErrorModel(Exception exception, bool isDevelopment)
            : base("An error has occurred.")
        {
            if (isDevelopment)
            {
                this.DeveloperError = new DeveloperErrorModel(exception);
            }
        }

        public DeveloperErrorModel? DeveloperError { get; }
    }
}
