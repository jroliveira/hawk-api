namespace Hawk.Domain.Shared
{
    using System;

    using static Hawk.Infrastructure.Clock;

    public abstract class Entity
    {
        protected Entity() => this.CreationAt = UtcNow();

        public DateTime CreationAt { get; }
    }
}
