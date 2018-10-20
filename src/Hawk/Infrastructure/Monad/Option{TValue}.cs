namespace Hawk.Infrastructure.Monad
{
    using System;

    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public readonly struct Option<TValue>
    {
        public static readonly Option<TValue> None = default;

        private readonly bool isDefined;
        private readonly TValue value;

        internal Option(TValue value, bool isDefined)
        {
            this.value = value;
            this.isDefined = isDefined;
        }

        public static implicit operator Option<TValue>(TValue value) => Some(value);

        public static implicit operator Option<TValue>(NoneType none) => None;

        public static implicit operator Option<TValue>(Try<TValue> @try) => @try.ToOption();

        public TReturn Match<TReturn>(Func<TValue, TReturn> some, Func<TReturn> none) => this.isDefined
            ? some(this.value)
            : none();
    }
}
