namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public abstract class ErrorController : ControllerBase
    {
        protected ErrorController(IWebHostEnvironment environment) => this.Environment = environment;

        internal IWebHostEnvironment Environment { get; }
    }
}
