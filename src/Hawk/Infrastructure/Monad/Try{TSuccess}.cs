namespace Hawk.Infrastructure.Monad
{
    using System;

    public readonly struct Try<TSuccess>
    {
        private readonly Exception failure;
        private readonly TSuccess success;

        internal Try(Exception failure)
        {
            this.IsFailure = true;
            this.failure = failure;
            this.success = default;
        }

        internal Try(TSuccess success)
        {
            this.IsFailure = false;
            this.failure = default;
            this.success = success;
        }

        public bool IsFailure { get; }

        public static implicit operator Try<TSuccess>(Exception failure) => new Try<TSuccess>(failure);

        public static implicit operator Try<TSuccess>(TSuccess success) => new Try<TSuccess>(success);

        public TReturn Match<TReturn>(Func<Exception, TReturn> failure, Func<TSuccess, TReturn> success) => this.IsFailure
            ? failure(this.failure)
            : success(this.success);

        public TSuccess Get() => this.success;
    }
}
