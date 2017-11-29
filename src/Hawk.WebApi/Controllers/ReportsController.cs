namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.WebApi.Lib.Extensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Report = Hawk.Infrastructure.Data.Neo4j.Reports.GetAmountGroupBy;

    [Authorize]
    [Route("reports")]
    public class ReportsController : BaseController
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
        public async Task<IActionResult> GetByStore()
        {
            var model = await this.getByStore.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);

            return this.Ok(model);
        }

        [HttpGet("amount-group-by-tag")]
        public async Task<IActionResult> Get()
        {
            var model = await this.getByTag.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);

            return this.Ok(model);
        }
    }
}