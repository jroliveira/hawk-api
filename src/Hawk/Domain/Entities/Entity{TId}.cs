namespace Hawk.Domain.Entities
{
    using System;

    using static Hawk.Infrastructure.Clock;

    public abstract class Entity<TId>
    {
        protected Entity() => this.CreationAt = UtcNow();

        public TId Id { get; protected set; }

        public DateTime CreationAt { get; }
    }
}
