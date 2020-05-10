namespace Hawk.WebApi.Features.Shared
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using static System.Net.WebUtility;
    using static System.String;

    using static Hawk.WebApi.Infrastructure.ErrorHandling.ErrorHandler;

    public abstract class BaseController : ControllerBase
    {
        internal IActionResult Created(object id, object value)
        {
            var uri = this.Request.GetDisplayUrl().Contains(id?.ToString() ?? Empty)
                ? $"{this.Request.GetDisplayUrl()}"
                : $"{this.Request.GetDisplayUrl()}/{UrlEncode(id?.ToString())}";

            return base.Created(uri, value);
        }

        internal new IActionResult NoContent() => base.NoContent();

        internal IActionResult Error<TModel>(BaseException exception) => ErrorResult<TModel>(exception);
    }
}
