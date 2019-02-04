namespace Hawk.WebApi.Features.Shared
{
    using System;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.WebApi.Features.Shared.ErrorModels;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : ControllerBase
    {
        private readonly IHostingEnvironment environment;

        public BaseController(IHostingEnvironment environment) => this.environment = environment;

        protected IActionResult Created(object id, object value)
        {
            var uri = $"{this.Request.GetDisplayUrl()}{(id == null ? string.Empty : $"/{id}")}";

            return base.Created(uri, value);
        }

        protected IActionResult HandleError(HawkException exception)
        {
            switch (exception)
            {
                case NotFoundException notFound: return this.NotFound(new GenericErrorModel(notFound));
                case InvalidObjectException somePropertyIsNull: return this.StatusCode(409, new ConflictErrorModel(somePropertyIsNull));
                case AlreadyExistsException alreadyExists: return this.StatusCode(409, new GenericErrorModel(alreadyExists));
                default: return this.HandleError(new InvalidOperationException("Exception does not exist."));
            }
        }

        protected IActionResult HandleError(Exception exception) => exception is HawkException hawkException
            ? this.HandleError(hawkException)
            : this.StatusCode(500, new GenericErrorModel(exception, this.environment));

        protected new IActionResult NoContent() => base.NoContent();
    }
}
