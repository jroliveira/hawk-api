namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Domain.Shared.Exceptions;

    public sealed class NotFoundErrorModel : ErrorModel
    {
        public NotFoundErrorModel(NotFoundException exception)
            : base(exception.Message)
        {
        }
    }
}
