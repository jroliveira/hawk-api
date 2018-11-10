namespace Hawk.WebApi.Models.Transaction.Post
{
    using System.Collections.Generic;

    using Hawk.WebApi.Lib.Mappings;

    public class Transaction
    {
        public string Type { get; set; }

        public Payment Payment { get; set; }

        public Parcel Parcel { get; set; }

        public string Store { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public Account Account { get; set; }

        public static implicit operator Domain.Transaction.Transaction(Transaction model) => model.ToEntity();
    }
}
