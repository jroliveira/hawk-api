namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Domain.Shared.Exceptions;

    public sealed class UnprocessableEntityErrorModel : ErrorModel
    {
        public UnprocessableEntityErrorModel(InvalidObjectException exception)
            : base(exception.Message) => this.Properties = exception.Properties;

        public object? Properties { get; }
    }
}
