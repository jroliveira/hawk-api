namespace Hawk.WebApi.Features.Store
{
    using System.Threading.Tasks;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.ErrorHandling.TryModel;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;
    using Hawk.WebApi.Infrastructure.Pagination;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static StoreModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("stores")]
    public class StoresController : BaseController
    {
        private readonly IGetStores getStores;
        private readonly IGetStoreByName getStoreByName;
        private readonly IUpsertStore upsertStore;
        private readonly IDeleteStore deleteStore;
        private readonly NewStoreModelValidator validator;

        public StoresController(
            IGetStores getStores,
            IGetStoreByName getStoreByName,
            IUpsertStore upsertStore,
            IDeleteStore deleteStore)
        {
            this.getStores = getStores;
            this.getStoreByName = getStoreByName;
            this.upsertStore = upsertStore;
            this.deleteStore = deleteStore;
            this.validator = new NewStoreModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TryModel<PageModel<TryModel<StoreModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStores()
        {
            var entities = await this.getStores.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<PageModel<TryModel<StoreModel>>>,
                page => this.Ok(MapStore(page)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(TryModel<StoreModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStoreByName([FromRoute] string name)
        {
            var entity = await this.getStoreByName.GetResult(this.GetUser(), name);

            return entity.Match(
                this.Error<StoreModel>,
                store => this.Ok(new TryModel<StoreModel>(new StoreModel(store))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TryModel<StoreModel>), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateStore([FromBody] NewStoreModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<StoreModel>(new InvalidObjectException("Invalid store.", validated));
            }

            var entity = await this.getStoreByName.GetResult(this.GetUser(), request.Name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertStore.Execute(this.GetUser(), request.Name, request);

                    return inserted.Match(
                        this.Error<StoreModel>,
                        store => this.Created(store.Name, new TryModel<StoreModel>(new StoreModel(store))));
                },
                _ => Task(this.Error<StoreModel>(new AlreadyExistsException("Store already exists."))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{name}")]
        [ProducesResponseType(typeof(TryModel<StoreModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateStore(
            [FromRoute] string name,
            [FromBody] NewStoreModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<StoreModel>(new InvalidObjectException("Invalid store.", validated));
            }

            var entity = await this.getStoreByName.GetResult(this.GetUser(), name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertStore.Execute(this.GetUser(), name, request);

                    return inserted.Match(
                        this.Error<StoreModel>,
                        store => this.Created(new TryModel<StoreModel>(new StoreModel(store))));
                },
                async _ =>
                {
                    var updated = await this.upsertStore.Execute(this.GetUser(), name, request);

                    return updated.Match(
                        this.Error<StoreModel>,
                        store => this.NoContent());
                });
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteStore([FromRoute] string name)
        {
            var deleted = await this.deleteStore.Execute(this.GetUser(), name);

            return deleted.Match(
                this.Error<StoreModel>,
                _ => this.NoContent());
        }
    }
}
