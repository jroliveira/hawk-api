namespace Finance.WebApi.Lib.Mappings
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Finance.Entities;
    using Finance.Entities.Transaction;
    using Finance.Entities.Transaction.Installment;
    using Finance.Entities.Transaction.Payment;

    using Model = Finance.WebApi.Models.Transaction;

    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            this.CreateMap<Model.Post.Transaction, Transaction>()
                .ConstructUsing(ConstructWith);

            this.CreateMap<Transaction, Model.Get.Transaction>()
                .ForMember(destination => destination.Type, origin => origin.MapFrom(source => source.GetType().Name))
                .ForMember(destination => destination.Store, origin => origin.MapFrom(source => source.Store))
                .ForMember(destination => destination.Tags, origin => origin.MapFrom(source => source.Tags));
        }

        private static Transaction ConstructWith(Model.Post.Transaction model)
        {
            var account = new Account(string.Empty, string.Empty);
            var payment = new Payment(model.Payment.Value, model.Payment.Date)
            {
                Method = model.Payment.Method
            };

            var type = Type.GetType($"Finance.Entities.Transaction.{model.Type}, Finance");
            if (type == null)
            {
                throw new NullReferenceException("O tipo da transação esta nula.");
            }

            var args = new object[] { payment, account };
            var transaction = Activator.CreateInstance(type, args) as Transaction;
            if (transaction == null)
            {
                throw new NullReferenceException("A transação esta nula.");
            }

            model.Tags.ToList().ForEach(tag => transaction.AddTag(tag));
            transaction.UpdateStore(model.Store);

            if (model.Parcel == null)
            {
                return transaction;
            }

            var parcel = new Parcel(model.Parcel.Total, model.Parcel.Number);
            transaction.SplittedIn(parcel);

            return transaction;
        }
    }
}