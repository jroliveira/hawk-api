namespace Hawk.Domain.Shared
{
    public abstract class Entity<TId> : Entity
    {
        public TId Id { get; protected set; }
    }
}
