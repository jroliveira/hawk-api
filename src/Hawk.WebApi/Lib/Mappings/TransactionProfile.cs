namespace Hawk.WebApi.Lib.Mappings
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Hawk.Entities;
    using Hawk.Entities.Transaction;
    using Hawk.Entities.Transaction.Installment;
    using Hawk.Entities.Transaction.Payment;

    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            this.CreateMap<Models.Transaction.Post.Transaction, Transaction>()
                .ConstructUsing(ConstructWith);

            this.CreateMap<Models.Transaction.Get.Transaction, Transaction>()
                .ConstructUsing(ConstructWith);

            this.CreateMap<Transaction, Models.Transaction.Get.Transaction>()
                .ForMember(destination => destination.Type, origin => origin.MapFrom(source => source.GetType().Name))
                .ForMember(destination => destination.Store, origin => origin.MapFrom(source => source.Store.Name))
                .ForMember(destination => destination.Tags, origin => origin.MapFrom(source => source.Tags.Select(tag => tag.Name)));

            this.CreateMap<Transaction, GraphQl.Sources.Transaction>()
                .ForMember(destination => destination.Type, origin => origin.MapFrom(source => source.GetType().Name))
                .ForMember(destination => destination.Tags, origin => origin.MapFrom(source => source.Tags.Select(tag => tag.Name)));
        }

        private static Transaction ConstructWith(Models.Transaction.Post.Transaction model)
        {
            var account = new Account(model.Account.Email, null);
            var payment = new Payment(model.Payment.Value, model.Payment.Date, model.Payment.Currency)
            {
                Method = model.Payment.Method
            };

            var type = Type.GetType($"Hawk.Entities.Transaction.{model.Type}, Hawk");
            if (type == null)
            {
                throw new NullReferenceException("O tipo da transação esta nula.");
            }

            var args = new object[] { payment, account };
            if (!(Activator.CreateInstance(type, args) is Transaction transaction))
            {
                throw new NullReferenceException("A transação esta nula.");
            }

            model.Tags.ToList().ForEach(tag => transaction.AddTag(tag));
            transaction.Store = model.Store;

            if (model.Parcel == null)
            {
                return transaction;
            }

            var parcel = new Parcel(model.Parcel.Total, model.Parcel.Number);
            transaction.SplittedIn(parcel);

            return transaction;
        }

        private static Transaction ConstructWith(Models.Transaction.Get.Transaction model)
        {
            var account = new Account(model.Account.Email, null);
            var payment = new Payment(model.Payment.Value, model.Payment.Date, model.Payment.Currency)
            {
                Method = model.Payment.Method
            };

            var type = Type.GetType($"Hawk.Entities.Transaction.{model.Type}, Hawk");
            if (type == null)
            {
                throw new NullReferenceException("O tipo da transação esta nula.");
            }

            var args = new object[] { payment, account };
            if (!(Activator.CreateInstance(type, args) is Transaction transaction))
            {
                throw new NullReferenceException("A transação esta nula.");
            }

            transaction.SetId(new Guid(model.Id));
            model.Tags.ToList().ForEach(tag => transaction.AddTag(tag));
            transaction.Store = model.Store;

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