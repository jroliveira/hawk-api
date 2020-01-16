namespace Hawk.WebApi.Features.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Caching;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class AccountsController : BaseController
    {
        private readonly IGetAccountByEmail getAccountByEmail;
        private readonly IUpsertAccount upsertAccount;
        private readonly IMemoryCache memoryCache;
        private readonly NewAccountModelValidator validator;

        public AccountsController(
            IGetAccountByEmail getAccountByEmail,
            IUpsertAccount upsertAccount,
            IMemoryCache memoryCache)
        {
            this.getAccountByEmail = getAccountByEmail;
            this.upsertAccount = upsertAccount;
            this.memoryCache = memoryCache;
            this.validator = new NewAccountModelValidator();
        }

        /// <summary>
        /// Get by email.
        /// </summary>
        /// <returns></returns>
        [HttpGet("account")]
        [ProducesResponseType(typeof(Try<AccountModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAccountByEmail()
        {
            var entity = await this.memoryCache.GetOrCreateCache(
                this.GetUser(),
                () => this.getAccountByEmail.GetResult(this.GetUser()));

            return entity.Match(
                this.Error<AccountModel>,
                account => this.Ok(Success(new AccountModel(account))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("accounts")]
        [ProducesResponseType(typeof(Try<AccountModel>), 201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAccount([FromBody] NewAccountModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<AccountModel>(new InvalidObjectException("Invalid account.", validated));
            }

            var entity = await this.getAccountByEmail.GetResult(request.Email);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertAccount.Execute(request);

                    return inserted.Match(
                        this.Error<AccountModel>,
                        account => this.Created(account.Email, Success(new AccountModel(account))));
                },
                _ => Task(this.Error<AccountModel>(new AlreadyExistsException("Account already exists."))));
        }
    }
}
