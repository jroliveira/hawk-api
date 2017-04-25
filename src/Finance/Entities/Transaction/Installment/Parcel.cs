namespace Finance.Entities.Transaction.Installment
{
    using System;

    public class Parcel
    {
        public Parcel(int total, int number = 1)
        {
            if (number < 1)
            {
                throw new ArgumentException("Parcela deve ser maior que 1", nameof(number));
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
