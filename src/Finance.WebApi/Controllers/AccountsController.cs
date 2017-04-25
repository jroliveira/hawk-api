namespace Finance.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Finance.Infrastructure.Data.Commands.Account;
    using Finance.Infrastructure.Data.Queries.Account;
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
        private readonly UpdateCommand update;
        private readonly ExcludeCommand exclude;
        private readonly AccountValidator validator;
        private readonly IMapper mapper;

        public AccountsController(
            GetByEmailQuery getByEmail,
            CreateCommand create,
            UpdateCommand update,
            ExcludeCommand exclude,
            AccountValidator validator,
            IMapper mapper)
        {
            this.getByEmail = getByEmail;
            this.create = create;
            this.update = update;
            this.exclude = exclude;
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
        public async Task<IActionResult> CreateAsync([FromBody] Model.Post.Account model)
        {
            var validateResult = this.validator.Validate(model);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Entities.Account>(model);
            await this.create.ExecuteAsync(entity);

            return this.StatusCode(201);
        }

        [HttpPut("{email}")]
        public async Task UpdateAsync([FromRoute] string email, [FromBody] dynamic model)
        {
            var entity = await this.getByEmail.GetResultAsync(email);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'accounts' with email {email} could not be found");
            }

            await this.update.ExecuteAsync(entity, model);
        }

        [HttpDelete("{email}")]
        public async Task ExcludeAsync([FromRoute] string email)
        {
            var entity = await this.getByEmail.GetResultAsync(email);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'accounts' with email {email} could not be found");
            }

            await this.exclude.ExecuteAsync(entity);
        }
    }
}