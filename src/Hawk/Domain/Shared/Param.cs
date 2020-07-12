namespace Hawk.Domain.Shared
{
    public abstract class Param
    {
        protected Param(in Email email) => this.Email = email;

        public Email Email { get; }
    }
}
