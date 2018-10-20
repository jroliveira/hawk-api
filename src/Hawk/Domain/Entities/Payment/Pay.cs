namespace Hawk.Domain.Entities.Payment
{
    using System;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public sealed class Pay
    {
        private Pay(Price price, DateTime date, Method method)
        {
            this.Price = price;
            this.Date = date;
            this.Method = method;
        }

        public Price Price { get; }

        public DateTime Date { get; }

        public Method Method { get; }

        public static Try<Pay> CreateWith(Option<Price> priceOption, in Option<DateTime> dateOption, Option<Method> methodOption)
        {
            var price = priceOption.GetOrElse(default);
            if (price == null)
            {
                return new ArgumentNullException(nameof(price), "Pay's price cannot be null.");
            }

            var method = methodOption.GetOrElse(default);
            if (method == null)
            {
                return new ArgumentNullException(nameof(methodOption), "Pay's payment method cannot be null.");
            }

            return dateOption.Match<Try<Pay>>(
                date => new Pay(price, date, method),
                () => new ArgumentNullException(nameof(dateOption), "Pay's date cannot be null."));
        }
    }
}
