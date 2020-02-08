namespace Hawk.Domain.Account
{
    using System;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.Infrastructure.Uid;

    public sealed class Account : Entity<Guid>
    {
        private Account(Guid id, Email email)
            : base(id) => this.Email = email;

        public Email Email { get; }

        public static Try<Account> NewAccount(Option<Email> email) => NewAccount(NewGuid(), email);

        public static Try<Account> NewAccount(Option<Guid> id, Option<Email> email) =>
            id
            && email
            ? new Account(
                id.Get(),
                email.Get())
            : Failure<Account>(new InvalidObjectException("Invalid account."));
    }
}
