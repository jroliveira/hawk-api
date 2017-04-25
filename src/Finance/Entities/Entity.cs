namespace Finance.Entities
{
    using System;

    using Finance.Infrastructure;

    public class Entity<TId>
    {
        public Entity()
        {
            this.CreationDate = Clock.Now();
        }

        public TId Id { get; protected set; }

        public DateTime CreationDate { get; }

        public bool Deleted { get; private set; }

        public virtual void MarkAsDeleted()
        {
            this.Deleted = true;
        }
    }
}