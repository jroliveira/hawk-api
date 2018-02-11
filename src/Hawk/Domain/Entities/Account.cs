namespace Hawk.Domain.Entities
{
    using System;

    using Hawk.Infrastructure;

    public sealed class Account : Entity<Guid>
    {
        public Account(string email)
            : this(Guid.NewGuid(), email)
        {
        }

        internal Account(Guid id, string email)
        {
            Guard.NotNullNorEmpty(email, nameof(email), "Account's e-mail cannot be null or empty.");

            this.Id = id;
            this.Email = email;
        }

        public string Email { get; }
    }
}
