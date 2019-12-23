namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public sealed class ForbiddenErrorModel : ErrorModel
    {
        public ForbiddenErrorModel(ForbiddenException exception)
            : base(exception.Message)
        {
        }
    }
}
