namespace Hawk.WebApi.Features.Home
{
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [AllowAnonymous]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public HomeController(IWebHostEnvironment environment) => this.environment = environment;

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(typeof(TryModel<HomeModel>), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get() => this.Ok(new TryModel<HomeModel>(new HomeModel(
            this.environment.ApplicationName,
            this.environment.EnvironmentName,
            "1.0.0")));
    }
}
