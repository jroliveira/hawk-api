namespace Hawk.Domain.Shared.Exceptions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using FluentValidation.Results;

    public sealed class InvalidObjectException : HawkException
    {
        public InvalidObjectException(string message)
            : this(message, new InvalidProperties())
        {
        }

        public InvalidObjectException(string message, InvalidProperties invalidProperties)
            : base(message) => this.Properties = invalidProperties;

        public InvalidProperties Properties { get; }

        public static implicit operator InvalidObjectException(ValidationResult validated) => new InvalidObjectException(
            "Invalid object.",
            validated);

        public class InvalidProperties : ReadOnlyCollection<InvalidProperty>
        {
            public InvalidProperties()
                : this(new List<InvalidProperty>())
            {
            }

            public InvalidProperties(IList<InvalidProperty> invalidProperties)
                : base(invalidProperties)
            {
            }

            public static implicit operator InvalidProperties(ValidationResult validated) => new InvalidProperties(validated
                .Errors
                .Select(error => new InvalidProperty(error.PropertyName, error.ErrorMessage))
                .ToList());

            public static implicit operator InvalidProperties(List<(string PropertyName, string ErrorMessage)> errors) => new InvalidProperties(errors
                .Select(error => new InvalidProperty(error.PropertyName, error.ErrorMessage))
                .ToList());
        }

        public class InvalidProperty
        {
            public InvalidProperty(string name, string message)
            {
                this.Name = name;
                this.Message = message;
            }

            public string Name { get; }

            public string Message { get; }
        }
    }
}
