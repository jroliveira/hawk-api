namespace Hawk.WebApi.GraphQl.Sources
{
    public class Transactions
    {
        public virtual double Amount { get; set; }
               
        public virtual int Quantity { get; set; }
               
        public virtual string Period { get; set; }
               
        public virtual string Type { get; set; }
    }
}