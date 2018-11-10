namespace Hawk.WebApi.Models.Transaction
{
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Account;

    public sealed class Account
    {
        public Account(string email) => this.Email = email;

        public string Email { get; }

        public static implicit operator Account(Domain.Account.Account entity) => new Account(entity.Email);

        public static implicit operator Option<Domain.Account.Account>(Account model) => CreateWith(model.Email);
    }
}
