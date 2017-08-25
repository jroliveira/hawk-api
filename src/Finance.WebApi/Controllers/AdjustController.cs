namespace Finance.WebApi.Controllers
{
    using Finance.Infrastructure.Data.Neo4j.Commands.Adjust._2017._07._03;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("adjust")]
    public class AdjustController : Controller
    {
        private readonly AdjustCommand adjust;

        public AdjustController(AdjustCommand adjust)
        {
            this.adjust = adjust;
        }

        [HttpPost]
        public IActionResult Adjust()
        {
            return this.Ok();
        }
    }
}