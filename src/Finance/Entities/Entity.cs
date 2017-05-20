namespace Finance.Entities
{
    using System;

    using Finance.Infrastructure;

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