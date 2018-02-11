namespace Hawk.Domain.Entities
{
    using System;

    public sealed class Parcel
    {
        public Parcel(int total, int number = 1)
        {
            if (total < 1)
            {
                throw new ArgumentException($"Parcel's total {total} should be greather than 1", nameof(total));
            }

            this.Number = number;
            this.Total = total;
        }

        public int Number { get; }

        public int Total { get; }

        public static implicit operator int(Parcel parcel)
        {
            return parcel.Number;
        }
    }
}
