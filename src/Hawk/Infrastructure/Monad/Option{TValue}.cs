namespace Hawk.Infrastructure.Monad
{
    using System;

    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public readonly struct Option<TValue>
    {
        private readonly TValue value;

        internal Option(in TValue value, in bool isDefined)
        {
            this.value = value;
            this.IsDefined = isDefined;
        }

        public bool IsDefined { get; }

        public static implicit operator Option<TValue>(in TValue value) => Some(value);

        public static implicit operator Option<TValue>(in None none) => default;

        public static implicit operator Option<TValue>(in Try<TValue> @try) => @try.ToOption();

        public static implicit operator bool(in Option<TValue> option) => option.ToBoolean();

        public TReturn Match<TReturn>(in Func<TValue, TReturn> some, in Func<TReturn> none) => this.IsDefined
            ? some(this.value)
            : none();

        public TValue Get() => this.value;

        public bool ToBoolean() => this.IsDefined;
    }
}
