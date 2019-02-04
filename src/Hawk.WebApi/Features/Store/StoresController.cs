namespace Hawk.WebApi.Features.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static StoreModel;

    [Authorize]
    [ApiVersion("1")]
    [Route("stores")]
    public class StoresController : BaseController
    {
        private readonly IGetStores getStores;
        private readonly IGetStoreByName getStoreByName;

        public StoresController(
            IGetStores getStores,
            IGetStoreByName getStoreByName,
            IHostingEnvironment environment)
            : base(environment)
        {
            this.getStores = getStores;
            this.getStoreByName = getStoreByName;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paged<StoreModel>), 200)]
        public async Task<IActionResult> Get()
        {
            var entities = await this.getStores.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError,
                paged => this.Ok(MapFrom(paged)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(StoreModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var entity = await this.getStoreByName.GetResult(this.GetUser(), name);

            return entity.Match(
                this.HandleError,
                store => this.Ok(new StoreModel(store)));
        }
    }
}
