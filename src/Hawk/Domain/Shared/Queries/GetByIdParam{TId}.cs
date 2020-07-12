namespace Hawk.Domain.Shared.Queries
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class GetByIdParam<TId> : Param
    {
        private GetByIdParam(in Email email, in TId id)
            : base(email) => this.Id = id;

        public TId Id { get; }

        public static Try<GetByIdParam<TId>> NewGetByIdParam(in Option<Email> emailOption, in Option<TId> idOption) =>
            emailOption
            && idOption
                ? new GetByIdParam<TId>(
                    emailOption.Get(),
                    idOption.Get())
                : Failure<GetByIdParam<TId>>(new InvalidObjectException("Invalid get by id param."));
    }
}
