namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Hawk.Domain.Commands.Account;
    using Hawk.Domain.Exceptions;
    using Hawk.Domain.Queries.Account;
    using Hawk.WebApi.Lib.Exceptions;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models.Account.Get;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    [Authorize]
    [ApiVersion("1")]
    [Route("accounts")]
    public class AccountsController : BaseController
    {
        private readonly IGetByEmailQuery getByEmail;
        private readonly ICreateCommand create;
        private readonly AccountValidator validator;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public AccountsController(
            IGetByEmailQuery getByEmail,
            ICreateCommand create,
            IMapper mapper)
            : this(getByEmail, create, mapper, new AccountValidator())
        {
        }

        internal AccountsController(
            IGetByEmailQuery getByEmail,
            ICreateCommand create,
            IMapper mapper,
            AccountValidator validator)
        {
            this.getByEmail = getByEmail;
            this.create = create;
            this.validator = validator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(Account), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Account), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Account.Post.Account request)
        {
            var validateResult = await this.validator.ValidateAsync(request).ConfigureAwait(false);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var entity = this.mapper.Map<Domain.Entities.Account>(request);
            var inserted = await this.create.Execute(entity).ConfigureAwait(false);
            var response = this.mapper.Map<Account>(inserted);

            return this.Created(response.Email, response);
        }
    }
}