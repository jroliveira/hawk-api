namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Domain.Shared.Exceptions;

    public sealed class BadRequestErrorModel : ErrorModel
    {
        public BadRequestErrorModel(InvalidRequestException exception)
            : base(exception.Message)
        {
        }
    }
}
