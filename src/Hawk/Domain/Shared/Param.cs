namespace Hawk.Domain.Shared
{
    public abstract class Param
    {
        protected Param(Email email) => this.Email = email;

        public Email Email { get; }
    }
}
