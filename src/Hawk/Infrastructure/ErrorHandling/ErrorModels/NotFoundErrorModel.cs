namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public sealed class NotFoundErrorModel : ErrorModel
    {
        public NotFoundErrorModel(NotFoundException exception)
            : base(exception.Message)
        {
        }
    }
}
