namespace Hawk.Infrastructure.ErrorHandling
{
    using System;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.Infrastructure.ErrorHandling.ErrorModels;
    using Hawk.Infrastructure.ErrorHandling.TryModel;

    public sealed class ExceptionHandler
    {
        private static ExceptionHandler? errorHandler;

        private readonly bool isDevelopment;

        private ExceptionHandler(bool isDevelopment)
        {
            this.isDevelopment = isDevelopment;

            errorHandler = this;
        }

        public static void NewExceptionHandler(bool isDevelopment) => errorHandler = new ExceptionHandler(isDevelopment);

        public static TryModel<Unit> HandleException(Exception exception) => HandleException<Unit>(exception);

        public static TryModel<TModel> HandleException<TModel>(Exception exception) => exception switch
        {
            AlreadyExistsException alreadyExists => new ConflictErrorModel(alreadyExists),
            InvalidObjectException invalidObject => new UnprocessableEntityErrorModel(invalidObject),
            InvalidRequestException invalidRequest => new BadRequestErrorModel(invalidRequest),
            NotFoundException notFound => new NotFoundErrorModel(notFound),
            _ => new GenericErrorModel(exception, errorHandler.isDevelopment),
        };

        public sealed class Unit
        {
        }
    }
}
