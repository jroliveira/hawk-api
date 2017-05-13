namespace Finance.WebApi.Controllers
{
    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Neo4j.Commands.Transaction;
    using Finance.Infrastructure.Data.Neo4j.Queries.Transaction;
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
        public IActionResult GetById([FromRoute] int id)
        {
            var entity = this.getById.GetResult(id, "junolive@gmail.com");
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            var model = this.mapper.Map<Model.Get.Transaction>(entity);

            return this.Ok(model);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entities = this.getAll.GetResult("junolive@gmail.com", this.Request.QueryString.Value);
            var model = this.mapper.Map<Paged<Model.Get.Transaction>>(entities);

            return this.Ok(model);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Model.Post.Transaction request)
        {
            var validateResult = this.validator.Validate(request);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Entities.Transaction.Transaction>(request);
            var inserted = this.create.Execute(entity);
            var response = this.mapper.Map<Model.Get.Transaction>(inserted);

            return this.StatusCode(201, response);
        }

        [HttpDelete("{id}")]
        public IActionResult Exclude([FromRoute] int id)
        {
            var entity = this.getById.GetResult(id, "junolive@gmail.com");
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'transactions' with id {id} could not be found");
            }

            this.exclude.Execute(entity);

            return this.NoContent();
        }
    }
}