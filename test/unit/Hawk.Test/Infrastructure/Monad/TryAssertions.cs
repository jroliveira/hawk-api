namespace Hawk.Test.Infrastructure.Monad
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Primitives;
    using FluentAssertions.Specialized;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.Linq.Enumerable;

    internal sealed class TryAssertions<TSuccess>
    {
        private readonly Try<TSuccess> @try;
        private readonly BooleanAssertions boolean;

        private TryAssertions(Try<TSuccess> @try)
        {
            this.@try = @try;
            this.boolean = new BooleanAssertions(@try);
        }

        public static implicit operator TryAssertions<TSuccess>(Try<TSuccess> @try) => new TryAssertions<TSuccess>(@try);

        internal ExceptionAssertions<TException> BeException<TException>()
            where TException : BaseException => this.@try.Match(
                failure => failure switch
                {
                    TException exception => new ExceptionAssertions<TException>(new List<TException> { exception }),
                    _ => new ExceptionAssertions<TException>(Empty<TException>()),
                },
                _ => new ExceptionAssertions<TException>(Empty<TException>()));

        internal AndConstraint<BooleanAssertions> BeFalse() => this.boolean.BeFalse();

        internal AndConstraint<BooleanAssertions> BeTrue() => this.boolean.BeTrue();
    }
}
