namespace Hawk.WebApi.Features.Account
{
    using Hawk.Domain.Account;

    public sealed class AccountModel
    {
        private AccountModel(Account entity)
        {
            this.Email = entity.Email;
            this.Setting = entity.Setting;
        }

        public string Email { get; }

        public SettingModel Setting { get; }

        internal static AccountModel NewAccountModel(in Account entity) => new AccountModel(entity);
    }
}
