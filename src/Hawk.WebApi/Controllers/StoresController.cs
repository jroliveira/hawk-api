namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Domain.Queries.Store;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [Authorize]
    [ApiVersion("1")]
    [Route("stores")]
    public class StoresController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public StoresController(
            IGetAllQuery getAll,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<Store>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.User.GetClientId(), this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Store>>(entities);

            return this.Ok(model);
        }
    }
}