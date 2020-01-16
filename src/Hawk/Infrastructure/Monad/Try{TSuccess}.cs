namespace Hawk.Infrastructure.Monad
{
    using System;
    using System.Runtime.Serialization;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    [Serializable]
    public sealed class Try<TSuccess> : ITry<TSuccess>, ISerializable
    {
        private readonly BaseException? failure;
        private readonly TSuccess success;

        public Try(BaseException failure)
        {
            this.failure = failure;
            this.success = default;
        }

        public Try(TSuccess success)
        {
            this.failure = default;
            this.success = success;
        }

        public static implicit operator Try<TSuccess>(BaseException failure) => Failure<TSuccess>(failure);

        public static implicit operator Try<TSuccess>(TSuccess success) => Success(success);

        public static implicit operator bool(Try<TSuccess> @try) => @try.ToBoolean();

        public TReturn Match<TReturn>(Func<BaseException, TReturn> failureFunc, Func<TSuccess, TReturn> successFunc) => this.failure != default
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
