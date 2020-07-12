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
        private Account(in Guid id, in Email email)
            : base(id) => this.Email = email;

        public Email Email { get; }

        public static Try<Account> NewAccount(in Option<Email> emailOption) => NewAccount(NewGuid(), emailOption);

        public static Try<Account> NewAccount(in Option<Guid> idOption, in Option<Email> emailOption) =>
            idOption
            && emailOption
                ? new Account(
                    idOption.Get(),
                    emailOption.Get())
                : Failure<Account>(new InvalidObjectException("Invalid account."));
    }
}
