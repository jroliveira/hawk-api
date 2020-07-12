namespace Hawk.WebApi.Features.Account
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Account;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Account;
    using static Hawk.Domain.Shared.Email;

    public sealed class CreateAccountModel
    {
        public CreateAccountModel(string email) => this.Email = email;

        [Required]
        public string Email { get; }

        public static implicit operator Option<Account>(in CreateAccountModel model) => NewAccount(NewEmail(model.Email));

        public static implicit operator Option<Email>(in CreateAccountModel model) => NewEmail(model.Email);
    }
}
