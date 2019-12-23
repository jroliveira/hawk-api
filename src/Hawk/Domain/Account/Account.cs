namespace Hawk.Domain.Account
{
    using System;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.Guid;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Account : Entity<Guid>
    {
        private Account(Guid id, Email email)
            : base(id) => this.Email = email;

        public Email Email { get; }

        public static Try<Account> NewAccount(Option<string> name) => NewAccount(NewGuid(), name);

        public static Try<Account> NewAccount(Option<Guid> id, Option<string> name) =>
            id
            && name
            ? new Account(id.Get(), name.Get())
            : Failure<Account>(new InvalidObjectException("Invalid account."));
    }
}
