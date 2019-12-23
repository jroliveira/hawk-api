namespace Hawk.Infrastructure.ErrorHandling
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.ErrorModels;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.Infrastructure.Monad;

    public sealed class ExceptionHandler
    {
        private static ExceptionHandler? errorHandler;

        private readonly bool isDevelopment;

        private ExceptionHandler(bool isDevelopment) => this.isDevelopment = isDevelopment;

        public static void NewExceptionHandler(bool isDevelopment) => errorHandler = new ExceptionHandler(isDevelopment);

        public static TryModel<Unit> HandleException(Exception exception) => HandleException<Unit>(exception);

        public static TryModel<TModel> HandleException<TModel>(Exception exception) => HandleException<TModel>(exception, false);

        public static TryModel<TModel> HandleException<TModel>(Exception exception, bool forceIsDevelopment) => exception switch
        {
            AlreadyExistsException alreadyExists => new ConflictErrorModel(alreadyExists),
            ForbiddenException forbidden => new ForbiddenErrorModel(forbidden),
            InvalidObjectException invalidObject => new UnprocessableEntityErrorModel(invalidObject),
            InvalidRequestException invalidRequest => new BadRequestErrorModel(invalidRequest),
            NotFoundException notFound => new NotFoundErrorModel(notFound),
            UnauthorizedException unauthorized => new UnauthorizedErrorModel(unauthorized),
            _ => new GenericErrorModel(exception, forceIsDevelopment || (errorHandler != null && errorHandler.isDevelopment)),
        };
    }
}
