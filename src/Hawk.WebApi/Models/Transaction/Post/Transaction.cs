namespace Hawk.WebApi.Models.Transaction.Post
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Payment Payment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Parcel Parcel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Store { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Account Account { get; set; }
    }
}
