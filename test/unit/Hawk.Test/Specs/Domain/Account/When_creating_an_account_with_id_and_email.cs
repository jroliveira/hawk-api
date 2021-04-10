namespace Hawk.Test.Specs.Domain.Account
{
    using Hawk.Domain.Account;

    using Machine.Specifications;

    using static Hawk.Domain.Account.Account;

    [Tags("domain")]
    [Subject(typeof(Account))]
    class When_creating_an_account_with_id_and_email : Given_a_valid_account_information
    {
        Because of = () =>
            Subject = NewAccount(Id, Email, Setting);

        Behaves_like<AnAccountThatHasBeenCreated> an_account_that_has_been_created;
    }
}
