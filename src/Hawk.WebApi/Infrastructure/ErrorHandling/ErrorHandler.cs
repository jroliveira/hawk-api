namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using System;

    using Hawk.Infrastructure.ErrorHandling.ErrorModels;
    using Hawk.Infrastructure.Monad;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;
    using static Hawk.Infrastructure.Logging.Logger;

    internal static class ErrorHandler
    {
        internal static void NewErrorHandler(IWebHostEnvironment environment) => NewExceptionHandler(environment.IsDevelopment());

        internal static IActionResult ErrorResult(Exception exception) => ErrorResult<Unit>(exception);

        internal static IActionResult ErrorResult<TModel>(Exception exception)
        {
            LogError<TModel>("An error has occurred.", exception);

            var tryModel = HandleException<TModel>(exception);

            return tryModel.Match(
                errorModel => errorModel switch
                {
                    BadRequestErrorModel _ => new BadRequestObjectResult(tryModel),
                    ForbiddenErrorModel _ => new ObjectResult(tryModel) { StatusCode = 403 },
                    NotFoundErrorModel _ => new NotFoundObjectResult(tryModel),
                    ConflictErrorModel _ => new ConflictObjectResult(tryModel),
                    UnauthorizedErrorModel _ => new UnauthorizedObjectResult(tryModel),
                    UnprocessableEntityErrorModel _ => new UnprocessableEntityObjectResult(tryModel),
                    _ => new ObjectResult(tryModel) { StatusCode = 500 },
                },
                model => new OkObjectResult(model));
        }
    }
}
