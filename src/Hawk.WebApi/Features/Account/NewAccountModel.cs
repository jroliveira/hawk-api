namespace Hawk.WebApi.Features.Account
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Account;

    public sealed class NewAccountModel
    {
        public NewAccountModel(string email) => this.Email = email;

        [Required]
        public string Email { get; }

        public static implicit operator Option<Account>(NewAccountModel model) => NewAccount(model.Email);
    }
}
