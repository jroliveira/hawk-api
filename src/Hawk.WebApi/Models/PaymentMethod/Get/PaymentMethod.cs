namespace Hawk.WebApi.Models.PaymentMethod.Get
{
    public sealed class PaymentMethod
    {
        public PaymentMethod(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }
    }
}
