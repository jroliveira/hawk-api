namespace Hawk.WebApi.Models
{
    internal sealed class PaymentMethod
    {
        public PaymentMethod(string name, int total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public int Total { get; }
    }
}
