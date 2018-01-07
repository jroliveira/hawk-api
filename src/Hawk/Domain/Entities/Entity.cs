namespace Hawk.Domain.Entities
{
    using System;

    using Hawk.Infrastructure;

    public abstract class Entity<TId>
    {
        protected Entity()
        {
            this.CreationAt = Clock.Now();
        }

        public TId Id { get; protected set; }

        public DateTime CreationAt { get; }
    }
}