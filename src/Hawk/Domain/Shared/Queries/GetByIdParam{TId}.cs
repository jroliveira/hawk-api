namespace Hawk.Domain.Shared.Queries
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class GetByIdParam<TId> : Param
    {
        private GetByIdParam(
            Email email,
            TId id)
            : base(email) => this.Id = id;

        public TId Id { get; }

        public static Try<GetByIdParam<TId>> NewGetByIdParam(
            Option<Email> email,
            Option<TId> id) =>
                email
                && id
                    ? new GetByIdParam<TId>(
                        email.Get(),
                        id.Get())
                    : Failure<GetByIdParam<TId>>(new InvalidObjectException("Invalid get by id param."));
    }
}
