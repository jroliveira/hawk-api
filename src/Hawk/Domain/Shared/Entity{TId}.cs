namespace Hawk.Domain.Shared
{
    public abstract class Entity<TId> : Entity
    {
        protected Entity(in TId id) => this.Id = id;

        public TId Id { get; }

        public static implicit operator TId(in Entity<TId> entity) => entity.Id;

        public override string? ToString() => this.Id?.ToString();
    }
}
