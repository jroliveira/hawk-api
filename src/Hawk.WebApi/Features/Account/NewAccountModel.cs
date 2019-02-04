namespace Hawk.WebApi.Features.Account
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Account;

    public sealed class NewAccountModel
    {
        public NewAccountModel(string email, string password, string confirmPassword)
        {
            this.Email = email;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }

        [Required]
        public string Email { get; }

        [Required]
        public string Password { get; }

        [Required]
        public string ConfirmPassword { get; }

        public static implicit operator Option<Account>(NewAccountModel model) => CreateWith(model.Email);
    }
}
