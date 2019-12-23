namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public sealed class ConflictErrorModel : ErrorModel
    {
        public ConflictErrorModel(AlreadyExistsException exception)
            : base(exception.Message)
        {
        }
    }
}
