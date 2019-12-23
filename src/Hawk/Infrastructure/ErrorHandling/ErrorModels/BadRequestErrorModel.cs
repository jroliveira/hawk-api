namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public sealed class BadRequestErrorModel : ErrorModel
    {
        public BadRequestErrorModel(InvalidRequestException exception)
            : base(exception.Message)
        {
        }
    }
}
