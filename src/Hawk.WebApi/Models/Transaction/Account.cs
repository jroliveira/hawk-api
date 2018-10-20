namespace Hawk.WebApi.Models.Transaction
{
    using Hawk.Infrastructure.Monad;

    public sealed class Account
    {
        public Account(string email) => this.Email = email;

        public string Email { get; }

        public static implicit operator Account(Domain.Entities.Account entity) => new Account(entity.Email);

        public static implicit operator Option<Domain.Entities.Account>(Account model) => Domain.Entities.Account.CreateWith(model.Email);
    }
}
