namespace Hawk.Domain.Shared.Commands
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class DeleteParam<TId> : Param
    {
        private DeleteParam(
            Email email,
            TId id)
            : base(email) => this.Id = id;

        public TId Id { get; }

        public static Try<DeleteParam<TId>> NewDeleteParam(
            Option<Email> email,
            Option<TId> id) =>
                email
                && id
                    ? new DeleteParam<TId>(
                        email.Get(),
                        id.Get())
                    : Failure<DeleteParam<TId>>(new InvalidObjectException("Invalid delete param."));
    }
}
