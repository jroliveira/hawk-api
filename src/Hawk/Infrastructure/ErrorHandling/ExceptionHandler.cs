namespace Hawk.Infrastructure.ErrorHandling
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    public sealed class ExceptionHandler
    {
        private static ExceptionHandler? errorHandler;

        private readonly bool isDevelopment;

        private ExceptionHandler(in bool isDevelopment) => this.isDevelopment = isDevelopment;

        public static void NewExceptionHandler(in bool isDevelopment) => errorHandler = new ExceptionHandler(isDevelopment);

        public static Try<Unit> HandleException(in Exception exception) => HandleException<Unit>(exception);

        public static Try<TModel> HandleException<TModel>(in Exception exception) => HandleException<TModel>(exception, false);

        public static Try<TModel> HandleException<TModel>(in Exception exception, in bool forceIsDevelopment) => exception switch
        {
            AlreadyExistsException alreadyExists => alreadyExists,
            ForbiddenException forbidden => forbidden,
            InvalidObjectException invalidObject => invalidObject,
            InvalidRequestException invalidRequest => invalidRequest,
            NotFoundException notFound => notFound,
            UnauthorizedException unauthorized => unauthorized,
            _ => new GenericException(exception, forceIsDevelopment || (errorHandler != null && errorHandler.isDevelopment)),
        };
    }
}
