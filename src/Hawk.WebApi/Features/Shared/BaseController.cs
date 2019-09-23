namespace Hawk.WebApi.Features.Shared
{
    using Hawk.WebApi.Infrastructure.ErrorHandling;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseController : ErrorController
    {
        protected BaseController(IHostingEnvironment environment)
            : base(environment)
        {
        }

        internal IActionResult Created(object id, object value)
        {
            var uri = $"{this.Request.GetDisplayUrl()}{(id == null ? string.Empty : $"/{id}")}";

            return base.Created(uri, value);
        }

        internal new IActionResult NoContent() => base.NoContent();
    }
}
