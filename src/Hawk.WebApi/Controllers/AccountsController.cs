namespace Hawk.WebApi.Controllers
{
    using System.Threading.Tasks;

    using Hawk.Domain.Account;
    using Hawk.WebApi.Lib.Validators;
    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiVersion("1")]
    [Route("accounts")]
    public class AccountsController : BaseController
    {
        private readonly IGetAccountByEmail getAccountByEmail;
        private readonly IUpsertAccount upsertAccount;
        private readonly AccountValidator validator;

        public AccountsController(
            IGetAccountByEmail getAccountByEmail,
            IUpsertAccount upsertAccount)
            : this(getAccountByEmail, upsertAccount, new AccountValidator())
        {
        }

        internal AccountsController(
            IGetAccountByEmail getAccountByEmail,
            IUpsertAccount upsertAccount,
            AccountValidator validator)
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
        [ProducesResponseType(typeof(Models.Account.Get.Account), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var entity = await this.getAccountByEmail.GetResult(email);

            return entity.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                success => success.Match<IActionResult>(
                    account => this.Ok(new Models.Account.Get.Account(account.Email)),
                    () => this.NotFound($"Resource 'accounts' with email {email} could not be found")));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Models.Account.Get.Account), 201)]
        public async Task<IActionResult> Create([FromBody] Models.Account.Post.Account request)
        {
            var validateResult = await this.validator.ValidateAsync(request);
            if (!validateResult.IsValid)
            {
                return this.StatusCode(409, validateResult.Errors);
            }

            var inserted = await this.upsertAccount.Execute(request);

            return inserted.Match(
                failure => this.StatusCode(500, new Error(failure.Message)),
                account => this.Created(account.Email, new Models.Account.Get.Account(account.Email)));
        }
    }
}
