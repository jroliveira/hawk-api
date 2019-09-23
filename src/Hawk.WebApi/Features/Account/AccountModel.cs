namespace Hawk.WebApi.Features.Account
{
    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Account;

    public sealed class AccountModel
    {
        public AccountModel(Account entity)
            : this(entity.Email)
        {
        }

        public AccountModel(string email) => this.Email = email;

        public string Email { get; }

        public static implicit operator Option<Account>(AccountModel model) => CreateWith(model.Email);
    }
}
