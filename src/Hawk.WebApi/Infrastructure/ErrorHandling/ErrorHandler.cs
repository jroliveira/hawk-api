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
        internal static IActionResult HandleError<TModel>(this ErrorController @this, Exception exception)
        {
            switch (exception)
            {
                case NotFoundException _: return @this.NotFound(HandleError<TModel>(exception, @this.Environment));
                case InvalidObjectException _: return @this.StatusCode(409, HandleError<TModel>(exception, @this.Environment));
                case AlreadyExistsException _: return @this.StatusCode(409, HandleError<TModel>(exception, @this.Environment));
                default: return @this.StatusCode(500, HandleError<TModel>(exception, @this.Environment));
            }
        }

        internal static TryModel<TModel> HandleError<TModel>(Exception exception) => HandleError<TModel>(exception, default);

        internal static TryModel<TModel> HandleError<TModel>(Exception exception, IHostingEnvironment environment)
        {
            LogError(exception.Message, exception);

            switch (exception)
            {
                case NotFoundException _: return new GenericErrorModel(exception);
                case InvalidObjectException invalidObject: return new ConflictErrorModel(invalidObject);
                case AlreadyExistsException _: return new GenericErrorModel(exception);
                default: return new GenericErrorModel(exception, environment);
            }
        }
    }
}
