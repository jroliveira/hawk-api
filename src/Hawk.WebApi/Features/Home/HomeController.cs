namespace Hawk.WebApi.Features.Home
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("1")]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly IHostingEnvironment environment;

        public HomeController(IHostingEnvironment environment) => this.environment = environment;

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get()
        {
            var response = new
            {
                name = this.environment.ApplicationName,
                env = this.environment.EnvironmentName,
                version = "1.0.0",
            };

            return this.Ok(response);
        }
    }
}
