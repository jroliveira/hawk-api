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
            in Email email,
            in TId id,
            in TEntity entity)
            : base(email)
        {
            this.Id = id;
            this.Entity = entity;
        }

        public TId Id { get; }

        public TEntity Entity { get; }

        public static Try<UpsertParam<TId, TEntity>> NewUpsertParam(in Option<Email> emailOption, in Option<TEntity> entityOption) =>
            entityOption
                ? NewUpsertParam(
                    emailOption,
                    entityOption.Get().Id,
                    entityOption)
                : Failure<UpsertParam<TId, TEntity>>(new InvalidObjectException("Invalid upsert param."));

        public static Try<UpsertParam<TId, TEntity>> NewUpsertParam(
            in Option<Email> emailOption,
            in Option<TId> idOption,
            in Option<TEntity> entityOption) =>
                emailOption
                && idOption
                && entityOption
                    ? new UpsertParam<TId, TEntity>(
                        emailOption.Get(),
                        idOption.Get(),
                        entityOption.Get())
                    : Failure<UpsertParam<TId, TEntity>>(new InvalidObjectException("Invalid upsert param."));
    }
}
