namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.Queries.Store;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Lib.Extensions;
    using Hawk.WebApi.Lib.Mappings;
    using Hawk.WebApi.Models;
    using Hawk.WebApi.Models.Store.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiVersion("1")]
    [Route("stores")]
    public class StoresController : BaseController
    {
        private readonly IGetAllQuery getAll;
        private readonly IGetByNameQuery getByName;

        public StoresController(
            IGetAllQuery getAll,
            IGetByNameQuery getByName)
        {
            this.getAll = getAll;
            this.getByName = getByName;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<Store>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getAll.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                paged => this.Ok(paged.ToModel()));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Store), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var entity = await this.getByName.GetResult(name, this.GetUser());

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                success => success.Match<IActionResult>(
                    store => this.Ok(new Store(store)),
                    () => this.NotFound($"Resource 'stores' with name {name} could not be found")));
        }
    }
}
