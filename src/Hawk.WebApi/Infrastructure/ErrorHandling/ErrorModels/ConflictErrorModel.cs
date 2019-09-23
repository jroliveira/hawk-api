namespace Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Domain.Shared.Exceptions;

    public sealed class ConflictErrorModel : ErrorModel
    {
        public ConflictErrorModel(InvalidObjectException exception)
            : base(exception) => this.Properties = exception?.Properties;

        public object Properties { get; }
    }
}
