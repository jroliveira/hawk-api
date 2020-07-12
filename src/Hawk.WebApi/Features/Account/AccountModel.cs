namespace Hawk.WebApi.Features.Account
{
    using Hawk.Domain.Account;

    public sealed class AccountModel
    {
        private AccountModel(Account entity) => this.Email = entity.Email;

        public string Email { get; }

        internal static AccountModel NewAccountModel(in Account entity) => new AccountModel(entity);
    }
}
