namespace Hawk.Test.Specs.Infrastructure.Extensions.String
{
    using FluentAssertions;

    using Hawk.Infrastructure.Extensions;

    using Machine.Specifications;

    [Tags("infrastructure", "extensions")]
    [Subject(typeof(StringExtension))]
    class When_using_string_extensions
    {
        static string Subject;

        Establish context = () =>
            Subject = " JunIor OliveIra ";

        class When_executing_to_lower_case
        {
            It should_be = () =>
                Subject.ToLowerCase().Should().Be("junior oliveira");
        }

        class When_executing_to_upper_case
        {
            It should_be = () =>
                Subject.ToUpperCase().Should().Be("JUNIOR OLIVEIRA");
        }

        class When_executing_to_pascal_case
        {
            It should_be = () =>
                Subject.ToPascalCase().Should().Be("Junior Oliveira");
        }

        class When_executing_to_kebab_case
        {
            It should_be = () =>
                Subject.ToKebabCase().Should().Be("junior-oliveira");
        }
    }
}
