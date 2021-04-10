namespace Hawk.WebApi.Features.Account
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.Domain.Account.Commands;
    using Hawk.Domain.Account.Queries;
    using Hawk.Infrastructure.Caching;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static Hawk.Domain.Account.Queries.GetAccountByEmailParam;
    using static Hawk.Domain.Shared.Commands.UpsertParam<System.Guid, Hawk.Domain.Account.Account>;
    using static Hawk.Domain.Shared.Email;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Account.AccountModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("accounts")]
    public class AccountsController : BaseController
    {
        private readonly IGetAccountByEmail getAccountByEmail;
        private readonly IUpsertAccount upsertAccount;
        private readonly IMemoryCache memoryCache;
        private readonly CreateAccountModelValidator validator;

        public AccountsController(
            IGetAccountByEmail getAccountByEmail,
            IUpsertAccount upsertAccount,
            IMemoryCache memoryCache)
        {
            this.getAccountByEmail = getAccountByEmail;
            this.upsertAccount = upsertAccount;
            this.memoryCache = memoryCache;
            this.validator = new CreateAccountModelValidator();
        }

        /// <summary>
        /// Get by email.
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [ProducesResponseType(typeof(Try<AccountModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMe()
        {
            var entity = await this.memoryCache.GetOrCreateCache(
                this.GetUser(),
                () => this.getAccountByEmail.GetResult(NewGetAccountByEmailParam(this.GetUser())));

            return entity.Match(
                this.Error<AccountModel>,
                account => this.Ok(Success(NewAccountModel(account))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Try<AccountModel>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<AccountModel>(new InvalidObjectException("Invalid account.", validated));
            }

            if (await this.getAccountByEmail.GetResult(NewGetAccountByEmailParam(NewEmail(request.Email))))
            {
                return this.Error<AccountModel>(new AlreadyExistsException("Account already exists."));
            }

            Option<Account> entity = request;
            var @try = await this.upsertAccount.Execute(NewUpsertParam(NewEmail(request.Email), entity));

            return @try.Match(
                this.Error<AccountModel>,
                _ => this.Created(entity.Get().Id, Success(NewAccountModel(entity.Get()))));
        }
    }
}
