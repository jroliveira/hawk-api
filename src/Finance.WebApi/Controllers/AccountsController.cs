namespace Finance.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Infrastructure.Data.Neo4j.Commands.Account;
    using Finance.Infrastructure.Data.Neo4j.Queries.Account;
    using Finance.Infrastructure.Exceptions;
    using Finance.WebApi.Lib.Exceptions;
    using Finance.WebApi.Lib.Validators;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Model = Finance.WebApi.Models.Account;

    [Route("/accounts")]
    public class AccountsController : Controller
    {
        private readonly GetByEmailQuery getByEmail;
        private readonly CreateCommand create;
        private readonly AccountValidator validator;
        private readonly IMapper mapper;

        public AccountsController(
            GetByEmailQuery getByEmail,
            CreateCommand create,
            AccountValidator validator,
            IMapper mapper)
        {
            this.getByEmail = getByEmail;
            this.create = create;
            this.validator = validator;
            this.mapper = mapper;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
        {
            var entity = await this.getByEmail.GetResultAsync(email);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'accounts' with email {email} could not be found");
            }

            var model = this.mapper.Map<Model.Get.Account>(entity);

            return this.Ok(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody] Model.Post.Account request)
        {
            var validateResult = this.validator.Validate(request);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Entities.Account>(request);
            var inserted = await this.create.ExecuteAsync(entity);
            var response = this.mapper.Map<Model.Get.Account>(inserted);

            return this.StatusCode(201, response);
        }
    }
}