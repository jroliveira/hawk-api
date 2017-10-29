namespace Hawk.Entities
{
    using System;

    using Hawk.Infrastructure;

    public class Entity<TId>
    {
        public Entity()
        {
            this.CreationAt = Clock.Now();
        }

        public TId Id { get; protected set; }

        public DateTime CreationAt { get; }
    }
}