namespace Hawk.WebApi.Models.Account.Get
{
    public sealed class Account
    {
        public Account(string email) => this.Email = email;

        public string Email { get; }
    }
}
