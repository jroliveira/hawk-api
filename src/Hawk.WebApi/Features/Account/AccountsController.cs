namespace Hawk.WebApi.Features.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    [Authorize]
    [ApiVersion("1")]
    [Route("accounts")]
    public class AccountsController : BaseController
    {
        private readonly IGetAccountByEmail getAccountByEmail;
        private readonly IUpsertAccount upsertAccount;
        private readonly NewAccountModelValidator validator;

        public AccountsController(
            IGetAccountByEmail getAccountByEmail,
            IUpsertAccount upsertAccount,
            IHostingEnvironment environment)
            : this(getAccountByEmail, upsertAccount, new NewAccountModelValidator(), environment)
        {
        }

        internal AccountsController(
            IGetAccountByEmail getAccountByEmail,
            IUpsertAccount upsertAccount,
            NewAccountModelValidator validator,
            IHostingEnvironment environment)
            : base(environment)
        {
            this.getAccountByEmail = getAccountByEmail;
            this.upsertAccount = upsertAccount;
            this.validator = validator;
        }

        /// <summary>
        /// Get by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(AccountModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var entity = await this.getAccountByEmail.GetResult(email);

            return entity.Match(
                this.HandleError,
                account => this.Ok(new AccountModel(account)));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateSchema(typeof(NewAccountModel))]
        [ProducesResponseType(typeof(AccountModel), 201)]
        public async Task<IActionResult> Create([FromBody] NewAccountModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.HandleError(new InvalidObjectException("Invalid account.", validated));
            }

            var entity = await this.getAccountByEmail.GetResult(request.Email);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertAccount.Execute(request);

                    return inserted.Match(
                        this.HandleError,
                        account => this.Created(account.Email, new AccountModel(account)));
                },
                success => Task(this.HandleError(new AlreadyExistsException("Account already exists."))));
        }
    }
}
