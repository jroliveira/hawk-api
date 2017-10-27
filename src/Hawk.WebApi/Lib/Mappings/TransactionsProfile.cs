namespace Hawk.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Hawk.Entities.Transaction;

    public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            this.CreateMap<Transactions, GraphQl.Sources.Transactions>();
        }
    }
}