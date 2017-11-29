namespace Hawk.WebApi.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [Route("")]
    public class HomeController : BaseController
    {
        private readonly IHostingEnvironment environment;

        public HomeController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = new
            {
                version = "0.0.1",
                name = this.environment.ApplicationName,
                env = this.environment.EnvironmentName
            };

            return this.Ok(response);
        }
    }
}
