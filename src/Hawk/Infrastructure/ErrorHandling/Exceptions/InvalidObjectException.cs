namespace Hawk.Infrastructure.ErrorHandling.Exceptions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using FluentValidation.Results;

    public sealed class InvalidObjectException : BaseException
    {
        public InvalidObjectException(in string message)
            : this(message, default)
        {
        }

        public InvalidObjectException(in string message, in InvalidProperties? invalidProperties)
            : base(message) => this.Properties = invalidProperties;

        public InvalidProperties? Properties { get; }

        public static implicit operator InvalidObjectException(in ValidationResult validated) => new InvalidObjectException(
            "Invalid object.",
            validated);

        public class InvalidProperties : ReadOnlyCollection<InvalidProperty>
        {
            public InvalidProperties(in IList<InvalidProperty> invalidProperties)
                : base(invalidProperties)
            {
            }

            public static implicit operator InvalidProperties(in ValidationResult validated) => new InvalidProperties(validated
                .Errors
                .Select(error => new InvalidProperty(error.PropertyName, error.ErrorMessage))
                .ToList());

            public static implicit operator InvalidProperties(in List<(string PropertyName, string ErrorMessage)> errors) => new InvalidProperties(errors
                .Select(error => new InvalidProperty(error.PropertyName, error.ErrorMessage))
                .ToList());
        }

        public class InvalidProperty
        {
            public InvalidProperty(in string name, in string message)
            {
                this.Name = name;
                this.Message = message;
            }

            public string Name { get; }

            public string Message { get; }
        }
    }
}
