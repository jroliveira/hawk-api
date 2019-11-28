namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using System;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Logging.Logger;

    internal static class ErrorHandler
    {
        internal static IActionResult HandleError<TModel>(this ErrorController @this, Exception exception) => exception switch
        {
            NotFoundException _ => @this.NotFound(HandleError<TModel>(exception, @this.Environment)),
            InvalidObjectException _ => @this.StatusCode(409, HandleError<TModel>(exception, @this.Environment)),
            AlreadyExistsException _ => @this.StatusCode(409, HandleError<TModel>(exception, @this.Environment)),
            _ => @this.StatusCode(500, HandleError<TModel>(exception, @this.Environment))
        };

        internal static TryModel<TModel> HandleError<TModel>(Exception exception) => HandleError<TModel>(exception, default);

        internal static TryModel<TModel> HandleError<TModel>(Exception exception, IWebHostEnvironment? environment)
        {
            var error = exception switch
            {
                NotFoundException _ => (ErrorModel)new GenericErrorModel(exception),
                InvalidObjectException invalidObject => new ConflictErrorModel(invalidObject),
                AlreadyExistsException _ => new GenericErrorModel(exception),
                _ => new GenericErrorModel(exception, environment)
            };

            LogError(error.Message ?? "An error has occurred.", error);

            return error;
        }
    }
}
