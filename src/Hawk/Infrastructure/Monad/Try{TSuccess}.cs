namespace Hawk.Infrastructure.Monad
{
    using System;

    public readonly struct Try<TSuccess>
    {
        private readonly bool isFailure;
        private readonly Exception failure;
        private readonly TSuccess success;

        internal Try(Exception failure)
        {
            this.isFailure = true;
            this.failure = failure;
            this.success = default;
        }

        internal Try(TSuccess success)
        {
            this.isFailure = false;
            this.failure = default;
            this.success = success;
        }

        public static implicit operator Try<TSuccess>(Exception failure) => new Try<TSuccess>(failure);

        public static implicit operator Try<TSuccess>(TSuccess success) => new Try<TSuccess>(success);

        public TReturn Match<TReturn>(Func<Exception, TReturn> failure, Func<TSuccess, TReturn> success) => this.isFailure
            ? failure(this.failure)
            : success(this.success);

        public Try<TReturn> Map<TReturn>(Func<TSuccess, TReturn> mapper) => !this.isFailure
            ? mapper(this.success)
            : new Try<TReturn>(this.failure);

        public Try<TReturn> Bind<TReturn>(Func<TSuccess, Try<TReturn>> binder) => !this.isFailure
            ? binder(this.success)
            : new Try<TReturn>(this.failure);
    }
}
