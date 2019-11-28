namespace Hawk.WebApi.Features.Shared
{
    using Hawk.WebApi.Infrastructure.ErrorHandling;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using static System.String;

    public abstract class BaseController : ErrorController
    {
        protected BaseController(IWebHostEnvironment environment)
            : base(environment)
        {
        }

        internal IActionResult Created(object value) => this.Created(Empty, value);

        internal IActionResult Created(object id, object value)
        {
            var uri = $"{this.Request.GetDisplayUrl()}{(id == null ? Empty : $"/{id}")}";

            return base.Created(uri, value);
        }

        internal new IActionResult NoContent() => base.NoContent();
    }
}
