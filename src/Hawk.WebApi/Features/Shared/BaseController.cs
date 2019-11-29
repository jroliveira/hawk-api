namespace Hawk.WebApi.Features.Shared
{
    using System;

    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using static System.String;

    using static Hawk.WebApi.Infrastructure.ErrorHandling.ErrorHandler;

    public abstract class BaseController : ControllerBase
    {
        internal IActionResult Created(object value) => this.Created(Empty, value);

        internal IActionResult Created(object id, object value)
        {
            var uri = $"{this.Request.GetDisplayUrl()}{(id == null ? Empty : $"/{id}")}";

            return base.Created(uri, value);
        }

        internal new IActionResult NoContent() => base.NoContent();

        internal IActionResult Error<TModel>(Exception exception) => ErrorResult<TModel>(exception);
    }
}
