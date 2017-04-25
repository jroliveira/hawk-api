namespace Finance.WebApi.Lib.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation.Results;

    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationFailure> validationFailures)
            : base(string.Join(Environment.NewLine, validationFailures.Select(c => c.ErrorMessage)))
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
