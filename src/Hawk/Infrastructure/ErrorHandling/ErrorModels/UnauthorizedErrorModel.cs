namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    public sealed class UnauthorizedErrorModel : ErrorModel
    {
        public UnauthorizedErrorModel(UnauthorizedException exception)
            : base(exception.Message)
        {
        }
    }
}
