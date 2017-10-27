namespace Hawk.WebApi.GraphQl.Sources
{
    public class Store
    {
        public virtual string Name { get; set; }

        public virtual int Total { get; set; }

        public virtual Transactions Transactions { get; set; }
    }
}