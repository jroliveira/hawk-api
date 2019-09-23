namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public abstract class ErrorController : ControllerBase
    {
        protected ErrorController(IHostingEnvironment environment) => this.Environment = environment;

        internal IHostingEnvironment Environment { get; }
    }
}
