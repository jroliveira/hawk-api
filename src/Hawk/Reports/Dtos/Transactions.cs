﻿namespace Hawk.Reports.Dtos
{
    public sealed class Transactions
    {
        public double Amount { get; set; }

        public int Quantity { get; set; }

        public string Period { get; set; }

        public string Type { get; set; }
    }
}