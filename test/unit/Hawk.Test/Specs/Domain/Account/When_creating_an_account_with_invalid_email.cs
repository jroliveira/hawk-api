namespace Hawk.Test.Specs.Domain.Account
{
    using Hawk.Domain.Account;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Test.Infrastructure.Monad.Extensions;

    using Machine.Specifications;

    using static Hawk.Domain.Account.Account;
    using static Hawk.Domain.Shared.Email;

    [Tags("domain")]
    [Subject(typeof(Account))]
    class When_creating_an_account_with_invalid_email
    {
        static Try<Account> Subject;

        Because of = () =>
            Subject = NewAccount(NewEmail("junior"));

        It should_be_failed = () =>
            Subject.Must().BeFalse();

        It should_have_an_exception = () =>
            Subject.Must().BeException<InvalidObjectException>().WithMessage("Invalid account.");
    }
}
