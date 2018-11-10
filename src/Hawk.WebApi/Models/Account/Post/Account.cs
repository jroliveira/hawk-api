namespace Hawk.WebApi.Models.Account.Post
{
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Account.Account;

    public sealed class Account
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public static implicit operator Option<Domain.Account.Account>(Account model) => CreateWith(model.Email);

        public static implicit operator Domain.Account.Account(Account model) => CreateWith(model.Email).GetOrElse(default);
    }
}
