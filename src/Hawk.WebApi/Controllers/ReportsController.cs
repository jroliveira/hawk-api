namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.WebApi.Lib.Extensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Reports = Hawk.Reports;

    /// <inheritdoc />
    [Authorize]
    [ApiVersion("1")]
    [Route("reports")]
    public class ReportsController : BaseController
    {
        private readonly Reports.AmountGroupByStore.IGetQuery getByStore;
        private readonly Reports.AmountGroupByTag.IGetQuery getByTag;

        /// <inheritdoc />
        public ReportsController(
            Reports.AmountGroupByStore.IGetQuery getByStore,
            Reports.AmountGroupByTag.IGetQuery getByTag)
        {
            this.getByStore = getByStore;
            this.getByTag = getByTag;
        }

        /// <summary>
        /// Get by store
        /// </summary>
        /// <returns></returns>
        [HttpGet("amount-group-by-store")]
        public async Task<IActionResult> GetByStore()
        {
            var model = await this.getByStore.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);

            return this.Ok(model);
        }

        /// <summary>
        /// Get by tag
        /// </summary>
        /// <returns></returns>
        [HttpGet("amount-group-by-tag")]
        public async Task<IActionResult> GetByTag()
        {
            var model = await this.getByTag.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);

            return this.Ok(model);
        }
    }
}