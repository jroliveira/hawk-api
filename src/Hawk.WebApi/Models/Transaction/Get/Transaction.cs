namespace Hawk.WebApi.Models.Transaction.Get
{
    using System.Collections.Generic;

    public class Transaction
    {
        public virtual string Id { get; set; }

        public virtual string Type { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual Parcel Parcel { get; set; }

        public virtual string Store { get; set; }

        public virtual IEnumerable<string> Tags { get; set; }
    }
}
