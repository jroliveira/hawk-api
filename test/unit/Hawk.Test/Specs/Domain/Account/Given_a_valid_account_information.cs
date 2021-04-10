namespace Hawk.Test.Specs.Domain.Account
{
    using System;

    using Hawk.Domain.Account;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Setting;
    using static Hawk.Domain.Shared.Email;

    class Given_a_valid_account_information
    {
        protected static Try<Account> Subject;
        protected static Guid Id = new Guid("b297d2e3-7ec6-4838-a25f-5af58dacc625");
        protected static Email Email = NewEmail("test@test.com").Get();
        protected static Setting Setting = DefaultSetting;
    }
}
