namespace Hawk.WebApi.Models.Transaction.Get
{
    using System.Collections.Generic;

    internal sealed class Transaction
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public Payment Payment { get; set; }

        public Parcel Parcel { get; set; }

        public string Store { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public Account Account { get; set; }
    }
}
