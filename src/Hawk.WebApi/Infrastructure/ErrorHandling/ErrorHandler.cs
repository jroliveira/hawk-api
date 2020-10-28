namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;
    using static Hawk.Infrastructure.Logging.Logger;

    internal static class ErrorHandler
    {
        internal static void NewErrorHandler(IWebHostEnvironment environment) => NewExceptionHandler(environment.IsDevelopment());

        internal static IActionResult ErrorResult(BaseException exception) => ErrorResult<Unit>(exception);

        internal static IActionResult ErrorResult<TModel>(BaseException exception)
        {
            switch (exception)
            {
                case ForbiddenException _:
                case NotFoundException _:
                case UnauthorizedException _:
                    LogWarning<TModel>("A warning has occurred.", exception);
                    break;

                default:
                    LogError<TModel>("An error has occurred.", exception);
                    break;
            }

            var @try = HandleException<TModel>(exception);

            return @try.Match(
                failure => failure switch
                {
                    InvalidRequestException _ => new BadRequestObjectResult(@try),
                    ForbiddenException _ => new ObjectResult(@try) { StatusCode = 403 },
                    NotFoundException _ => new NotFoundObjectResult(@try),
                    AlreadyExistsException _ => new ConflictObjectResult(@try),
                    UnauthorizedException _ => new UnauthorizedObjectResult(@try),
                    InvalidObjectException _ => new UnprocessableEntityObjectResult(@try),
                    _ => new ObjectResult(@try) { StatusCode = 500 },
                },
                model => new OkObjectResult(model));
        }
    }
}
