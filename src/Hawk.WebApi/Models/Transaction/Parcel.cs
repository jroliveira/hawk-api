namespace Hawk.WebApi.Models.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Parcel
    {
        /// <summary>
        /// 
        /// </summary>
        public Parcel(int number, int total)
        {
            this.Number = number;
            this.Total = total;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Total { get; }
    }
}