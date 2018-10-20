namespace Hawk.Domain.Entities
{
    using System;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public sealed class Parcel
    {
        private Parcel(uint total, uint number)
        {
            this.Number = number;
            this.Total = total;
        }

        public uint Number { get; }

        public uint Total { get; }

        public static implicit operator uint(Parcel parcel) => parcel.Number;

        public static Try<Parcel> CreateWith(Option<uint> totalOption, Option<uint> numberOption)
        {
            var total = totalOption.GetOrElse(0u);
            if (total < 1)
            {
                return new ArgumentException($"Parcel's total {total} should be greather than 1", nameof(total));
            }

            var number = numberOption.GetOrElse(1u);

            return new Parcel(total, number);
        }
    }
}
