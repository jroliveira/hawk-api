namespace Hawk.Domain.Account
{
    using System;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Setting;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.Infrastructure.Uid;

    public sealed class Account : Entity<Guid>
    {
        private Account(
            in Guid id,
            in Email email,
            in Setting setting)
            : base(id)
        {
            this.Email = email;
            this.Setting = setting;
        }

        public Email Email { get; }

        public Setting Setting { get; }

        public static Try<Account> NewAccount(in Option<Email> emailOption) => NewAccount(
            idOption: NewGuid(),
            emailOption,
            settingOption: DefaultSetting);

        public static Try<Account> NewAccount(
            in Option<Guid> idOption,
            in Option<Email> emailOption,
            in Option<Setting> settingOption) =>
                idOption
                && emailOption
                && settingOption
                    ? new Account(
                        idOption.Get(),
                        emailOption.Get(),
                        settingOption.Get())
                    : Failure<Account>(new InvalidObjectException("Invalid account."));
    }
}
