namespace Hawk.Domain.Entities
{
    using System;

    public sealed class Account : Entity<Guid>
    {
        public Account(string email)
            : this(Guid.NewGuid(), email)
        {
        }

        internal Account(Guid id, string email)
        {
            this.Id = id;
            this.Email = email;
        }

        public string Email { get; }
    }
}
