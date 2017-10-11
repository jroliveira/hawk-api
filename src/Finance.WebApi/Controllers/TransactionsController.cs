namespace Finance.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Commands.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Queries.Transaction;
    using Finance.Infrastructure.Exceptions;
    using Finance.WebApi.Lib.Exceptions;
    using Finance.WebApi.Lib.Validators;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Model = Finance.WebApi.Models.Transaction;

    [Authorize]
    [Route("transactions")]
    public class TransactionsController : Controller
    {
        private readonly PartialUpdater partialUpdater;
        private readonly GetAllQuery getAll;
        private readonly GetByIdQuery getById;
        private readonly CreateCommand create;
        private readonly ExcludeCommand exclude;
        private readonly TransactionValidator validator;
        private readonly IMapper mapper;

        public TransactionsController(
            PartialUpdater partialUpdater,
            GetAllQuery getAll,
            GetByIdQuery getById,
            CreateCommand create,
            ExcludeCommand exclude,
            TransactionValidator validator,
            IMapper mapper)
        {
            this.partialUpdater = partialUpdater;
            this.getAll = getAll;
            this.getById = getById;
            this.create = create;
            this.exclude = exclude;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var entity = await this.getById.GetResultAsync(id, "junolive@gmail.com").ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            var model = this.mapper.Map<Model.Get.Transaction>(entity);

            return this.Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var entities = await this.getAll.GetResultAsync("junolive@gmail.com", this.Request.QueryString.Value).ConfigureAwait(false);
            var model = this.mapper.Map<Paged<Model.Get.Transaction>>(entities);

            return this.Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Model.Post.Transaction request)
        {
            var validateResult = await this.validator.ValidateAsync(request).ConfigureAwait(false);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Entities.Transaction.Transaction>(request);
            var inserted = await this.create.ExecuteAsync(entity).ConfigureAwait(false);
            var response = this.mapper.Map<Model.Get.Transaction>(inserted);

            return this.StatusCode(201, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] string id,
            [FromBody] dynamic request)
        {
            var entity = await this.getById.GetResultAsync(id, "junolive@gmail.com").ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            var model = this.mapper.Map<Model.Get.Transaction>(entity);
            this.partialUpdater.Apply(request, model);
            entity = this.mapper.Map<Entities.Transaction.Transaction>(model);
            await this.create.ExecuteAsync(entity).ConfigureAwait(false);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcludeAsync([FromRoute] string id)
        {
            var entity = await this.getById.GetResultAsync(id, "junolive@gmail.com").ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            await this.exclude.ExecuteAsync(entity).ConfigureAwait(false);

            return this.NoContent();
        }
    }
}