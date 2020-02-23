namespace Hawk.Domain.Shared.Commands
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class UpsertParam<TId, TEntity> : Param
        where TEntity : Entity<TId>
    {
        private UpsertParam(
            Email email,
            TId id,
            TEntity entity)
            : base(email)
        {
            this.Id = id;
            this.Entity = entity;
        }

        public TId Id { get; }

        public TEntity Entity { get; }

        public static Try<UpsertParam<TId, TEntity>> NewUpsertParam(
            Option<Email> email,
            Option<TEntity> entity) => entity.Match(
                some => NewUpsertParam(
                    email,
                    some.Id,
                    entity),
                () => Failure<UpsertParam<TId, TEntity>>(new InvalidObjectException("Invalid upsert param.")));

        public static Try<UpsertParam<TId, TEntity>> NewUpsertParam(
            Option<Email> email,
            Option<TId> id,
            Option<TEntity> entity) =>
                email
                && id
                && entity
                    ? new UpsertParam<TId, TEntity>(
                        email.Get(),
                        id.Get(),
                        entity.Get())
                    : Failure<UpsertParam<TId, TEntity>>(new InvalidObjectException("Invalid upsert param."));
    }
}
