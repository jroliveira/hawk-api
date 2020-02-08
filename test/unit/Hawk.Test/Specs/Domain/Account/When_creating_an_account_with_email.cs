namespace Hawk.Test.Specs.Domain.Account
{
    using Hawk.Domain.Account;

    using Machine.Specifications;

    using static Hawk.Domain.Account.Account;
    using static Hawk.Infrastructure.Uid;

    [Tags("domain")]
    [Subject(typeof(Account))]
    class When_creating_an_account_with_email : Given_a_valid_account_information
    {
        Establish context = () =>
            NewGuid = () => Id;

        Because of = () =>
            Subject = NewAccount(Email);

        Behaves_like<AnAccountThatHasBeenCreated> an_account_that_has_been_created;
    }
}
