namespace Finance.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Commands.Transaction;
    using Finance.Infrastructure.Data.Queries.Transaction;
    using Finance.Infrastructure.Exceptions;
    using Finance.WebApi.Lib.Exceptions;
    using Finance.WebApi.Lib.Validators;

    using Microsoft.AspNetCore.Mvc;

    using Model = Finance.WebApi.Models.Transaction;

    [Route("/transactions")]
    public class TransactionsController : Controller
    {
        private readonly GetAllQuery getAll;
        private readonly GetByIdQuery getById;
        private readonly CreateCommand create;
        private readonly ExcludeCommand exclude;
        private readonly TransactionValidator validator;
        private readonly IMapper mapper;

        public TransactionsController(
            GetAllQuery getAll,
            GetByIdQuery getById,
            CreateCommand create,
            ExcludeCommand exclude,
            TransactionValidator validator,
            IMapper mapper)
        {
            this.getAll = getAll;
            this.getById = getById;
            this.create = create;
            this.exclude = exclude;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var entity = await this.getById.GetResultAsync(id);
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
            var entities = await this.getAll.GetResultAsync(this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Model.Get.Transaction>>(entities);

            return this.Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Model.Post.Transaction model)
        {
            var validateResult = this.validator.Validate(model);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Entities.Transaction.Transaction>(model);
            await this.create.ExecuteAsync(entity);

            return this.StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task ExcludeAsync([FromRoute] int id)
        {
            var entity = await this.getById.GetResultAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            await this.exclude.ExecuteAsync(entity);
        }
    }
}