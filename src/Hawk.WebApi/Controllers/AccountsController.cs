namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Infrastructure.Data.Neo4j.Commands.Account;
    using Hawk.Infrastructure.Data.Neo4j.Queries.Account;
    using Hawk.Infrastructure.Exceptions;
    using Hawk.WebApi.Lib.Exceptions;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models.Account.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("accounts")]
    public class AccountsController : BaseController
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
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var entity = await this.getByEmail.GetResult(email).ConfigureAwait(false);
            if (entity == null)
            {
                throw new NotFoundException($"Resource 'accounts' with email {email} could not be found");
            }

            var model = this.mapper.Map<Account>(entity);

            return this.Ok(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] Models.Account.Post.Account request)
        {
            var validateResult = await this.validator.ValidateAsync(request).ConfigureAwait(false);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Entities.Account>(request);
            var inserted = await this.create.Execute(entity).ConfigureAwait(false);
            var response = this.mapper.Map<Account>(inserted);
            
            return this.Created(response.Email, response);
        }
    }
}