namespace Hawk.WebApi.Models.Transaction
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Payment
    {
        /// <summary>
        /// 
        /// </summary>
        public Payment(double value, DateTime date, string method, string currency)
        {
            this.Value = value;
            this.Date = date;
            this.Method = method;
            this.Currency = currency;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Method { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; }
    }
}