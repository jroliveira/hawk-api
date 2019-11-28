namespace Hawk.WebApi.Features.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Store;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.ErrorHandling;
    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static StoreModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("stores")]
    public class StoresController : BaseController
    {
        private readonly IGetStores getStores;
        private readonly IGetStoreByName getStoreByName;

        public StoresController(
            IGetStores getStores,
            IGetStoreByName getStoreByName,
            IWebHostEnvironment environment)
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
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<StoreModel>>>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStores()
        {
            var entities = await this.getStores.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.HandleError<PageModel<TryModel<StoreModel>>>,
                page => this.Ok(MapStore(page)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(TryModel<StoreModel>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStoreByName([FromRoute] string name)
        {
            var entity = await this.getStoreByName.GetResult(this.GetUser(), name);

            return entity.Match(
                this.HandleError<StoreModel>,
                store => this.Ok(new TryModel<StoreModel>(new StoreModel(store))));
        }
    }
}
