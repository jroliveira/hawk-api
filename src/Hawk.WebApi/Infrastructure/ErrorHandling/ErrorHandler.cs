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
            ErrorModel error;

            switch (exception)
            {
                case NotFoundException _: error = new GenericErrorModel(exception); break;
                case InvalidObjectException invalidObject: error = new ConflictErrorModel(invalidObject); break;
                case AlreadyExistsException _: error = new GenericErrorModel(exception); break;
                default: error = new GenericErrorModel(exception, environment); break;
            }

            LogError(error.Message, error);

            return error;
        }
    }
}
