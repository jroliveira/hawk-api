namespace Hawk.Test.Specs.Domain.Account
{
    using FluentAssertions;

    using Hawk.Test.Infrastructure.Monad.Extensions;

    using Machine.Specifications;

    [Behaviors]
    class AnAccountThatHasBeenCreated : Given_a_valid_account_information
    {
        It should_be_successful = () =>
            Subject.Must().BeTrue();

        It should_have_an_id = () =>
            Subject.Get().Id.Should().Be(Id);

        It should_have_an_email = () =>
            Subject.Get().Email.Should().Be(Email);
    }
}
