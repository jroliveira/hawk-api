namespace Finance.WebApi.Controllers
{
    using Finance.Infrastructure.Data.Neo4j.Reports.GetAmountGroupByTag;

    using Microsoft.AspNetCore.Mvc;

    public class ReportsController : Controller
    {
        private readonly GetQuery get;

        public ReportsController(GetQuery get)
        {
            this.get = get;
        }

        [HttpGet("reports/amount-group-by-tag")]
        public IActionResult Get()
        {
            var model = this.get.GetResult("junolive@gmail.com", this.Request.QueryString.Value);

            return this.Ok(model);
        }
    }
}