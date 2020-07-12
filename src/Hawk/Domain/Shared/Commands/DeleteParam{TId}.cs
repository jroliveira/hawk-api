namespace Hawk.Domain.Shared.Commands
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class DeleteParam<TId> : Param
    {
        private DeleteParam(in Email email, in TId id)
            : base(email) => this.Id = id;

        public TId Id { get; }

        public static Try<DeleteParam<TId>> NewDeleteParam(in Option<Email> emailOption, in Option<TId> idOption) =>
            emailOption
            && idOption
                ? new DeleteParam<TId>(
                    emailOption.Get(),
                    idOption.Get())
                : Failure<DeleteParam<TId>>(new InvalidObjectException("Invalid delete param."));
    }
}
