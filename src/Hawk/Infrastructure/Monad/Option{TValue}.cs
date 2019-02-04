namespace Hawk.Infrastructure.Monad
{
    using System;

    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public readonly struct Option<TValue>
    {
        private static readonly Option<TValue> None = default;

        private readonly TValue value;

        internal Option(TValue value, bool isDefined)
        {
            this.value = value;
            this.IsDefined = isDefined;
        }

        public bool IsDefined { get; }

        public static implicit operator Option<TValue>(TValue value) => Some(value);

        public static implicit operator Option<TValue>(None none) => None;

        public static implicit operator Option<TValue>(Try<TValue> @try) => @try.ToOption();

        public static implicit operator bool(Option<TValue> option) => option.IsDefined;

        public TReturn Match<TReturn>(Func<TValue, TReturn> some, Func<TReturn> none) => this.IsDefined
            ? some(this.value)
            : none();

        public TValue Get() => this.value;
    }
}
