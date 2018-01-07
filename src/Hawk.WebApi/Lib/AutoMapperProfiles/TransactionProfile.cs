namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Hawk.Domain.Entities;
    using Hawk.Domain.Entities.Payment;

    internal sealed class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            this.CreateMap<Models.Transaction.Post.Transaction, Transaction>()
                .ConstructUsing(model => ConstructWith(
                    model.Account,
                    model.Payment,
                    model.Parcel,
                    model.Tags,
                    model.Store,
                    model.Type));

            this.CreateMap<Models.Transaction.Get.Transaction, Transaction>()
                .ConstructUsing(model => ConstructWith(
                    model.Account,
                    model.Payment,
                    model.Parcel,
                    model.Tags,
                    model.Store,
                    model.Type,
                    model.Id));

            this.CreateMap<Transaction, Models.Transaction.Get.Transaction>()
                .ForMember(destination => destination.Type, origin => origin.MapFrom(source => source.GetType().Name))
                .ForMember(destination => destination.Payment, origin => origin.MapFrom(source => source.Pay))
                .ForMember(destination => destination.Store, origin => origin.MapFrom(source => source.Store.Name))
                .ForMember(destination => destination.Tags, origin => origin.MapFrom(source => source.Tags.Select(tag => tag.Name)));
        }

        private static Transaction ConstructWith(
            Models.Transaction.Account accountModel,
            Models.Transaction.Payment paymentModel,
            Models.Transaction.Parcel parcelModel,
            IEnumerable<string> tags,
            string store,
            string type,
            string id = null)
        {
            var account = new Account(accountModel.Email);
            var payment = new Pay(new Price(paymentModel.Value, paymentModel.Currency), paymentModel.Date, paymentModel.Method);

            var transactionType = Type.GetType($"Hawk.Domain.Entities.Transactions.{type}, Hawk");
            if (transactionType == null)
            {
                throw new NullReferenceException("O tipo da transação esta nula.");
            }

            var args = string.IsNullOrEmpty(id)
                ? new object[] { payment, account }
                : new object[] { new Guid(id), payment, account };

            if (!(Activator.CreateInstance(transactionType, args) is Transaction transaction))
            {
                throw new NullReferenceException("A transação esta nula.");
            }

            foreach (var tag in tags)
            {
                transaction.AddTag(tag);
            }

            transaction.UpdateStore(store);

            if (parcelModel == null)
            {
                return transaction;
            }

            var parcel = new Parcel(parcelModel.Total, parcelModel.Number);
            transaction.SplittedIn(parcel);

            return transaction;
        }
    }
}