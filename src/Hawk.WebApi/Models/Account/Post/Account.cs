namespace Hawk.WebApi.Models.Account.Post
{
    public class Account
    {
        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual string ConfirmPassword { get; set; }
    }
}
