namespace Hawk.WebApi.Features.Shared
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using static Hawk.WebApi.Infrastructure.ErrorHandling.ErrorHandler;

    public abstract class BaseController : ControllerBase
    {
        internal IActionResult Created(object id, object value) => base.Created(
            $"{this.Request.GetDisplayUrl()}/{id}",
            value);

        internal new IActionResult NoContent() => base.NoContent();

        internal IActionResult Error<TModel>(BaseException exception) => ErrorResult<TModel>(exception);
    }
}
