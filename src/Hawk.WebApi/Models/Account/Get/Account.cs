namespace Hawk.WebApi.Models.Account.Get
{
    internal sealed class Account
    {
        public Account(string email)
        {
            this.Email = email;
        }

        public string Email { get; }
    }
}
