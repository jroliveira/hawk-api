namespace Finance.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Report = Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy;

    [Authorize]
    [Route("reports")]
    public class ReportsController : Controller
    {
        private readonly Report.Store.GetQuery getByStore;
        private readonly Report.Tag.GetQuery getByTag;

        public ReportsController(
            Report.Store.GetQuery getByStore,
            Report.Tag.GetQuery getByTag)
        {
            this.getByStore = getByStore;
            this.getByTag = getByTag;
        }

        [HttpGet("amount-group-by-store")]
        public IActionResult GetByStore()
        {
            var model = this.getByStore.GetResult("junolive@gmail.com", this.Request.QueryString.Value);

            return this.Ok(model);
        }

        [HttpGet("amount-group-by-tag")]
        public IActionResult Get()
        {
            var model = this.getByTag.GetResult("junolive@gmail.com", this.Request.QueryString.Value);

            return this.Ok(model);
        }
    }
}