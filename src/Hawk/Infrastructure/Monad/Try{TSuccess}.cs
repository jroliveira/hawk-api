namespace Hawk.Infrastructure.Monad
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public readonly struct Try<TSuccess> : ISerializable
    {
        private readonly Exception? failure;
        private readonly TSuccess success;

        internal Try(Exception failure)
        {
            this.failure = failure;
            this.success = default;
        }

        internal Try(TSuccess success)
        {
            this.failure = default;
            this.success = success;
        }

        public static implicit operator Try<TSuccess>(Exception failure) => new Try<TSuccess>(failure);

        public static implicit operator Try<TSuccess>(TSuccess success) => new Try<TSuccess>(success);

        public static implicit operator bool(Try<TSuccess> @try) => @try.ToBoolean();

        public TReturn Match<TReturn>(Func<Exception, TReturn> failureFunc, Func<TSuccess, TReturn> successFunc) => this.failure != default
            ? failureFunc(this.failure)
            : successFunc(this.success);

        public TSuccess Get() => this.success;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (this.failure != default)
            {
                info.AddValue(nameof(this.failure), this.failure.Message);
            }
            else
            {
                info.AddValue(nameof(this.success), this.success);
            }
        }

        public bool ToBoolean() => this.failure == default;
    }
}
