namespace Hawk.WebApi.Models.Account.Post
{
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public sealed class Account
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public static implicit operator Option<Domain.Entities.Account>(Account model) => Domain.Entities.Account.CreateWith(model.Email);

        public static implicit operator Domain.Entities.Account(Account model) => Domain.Entities.Account.CreateWith(model.Email).GetOrElse(default);
    }
}
