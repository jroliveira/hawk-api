namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Domain.Shared.Exceptions;

    public sealed class ConflictErrorModel : ErrorModel
    {
        public ConflictErrorModel(AlreadyExistsException exception)
            : base(exception.Message)
        {
        }
    }
}
