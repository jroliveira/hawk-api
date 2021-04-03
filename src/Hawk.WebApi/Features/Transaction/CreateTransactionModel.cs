namespace Hawk.WebApi.Features.Transaction
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Shared.Transaction;
    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.WebApi.Features.Category;
    using Hawk.WebApi.Features.Payee;

    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Domain.Transaction.Transaction;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class CreateTransactionModel
    {
        public CreateTransactionModel(
            string type,
            string status,
            string description,
            PaymentModel payment,
            PayeeModel payee,
            CategoryModel category,
            IEnumerable<string> tags)
        {
            this.Type = type;
            this.Status = status;
            this.Description = description;
            this.Payment = payment;
            this.Payee = payee;
            this.Category = category;
            this.Tags = tags;
        }

        [Required]
        public string Type { get; }

        [Required]
        public string Status { get; }

        public string Description { get; }

        [Required]
        public PaymentModel Payment { get; }

        [Required]
        public PayeeModel Payee { get; }

        [Required]
        public CategoryModel Category { get; }

        [Required]
        public IEnumerable<string> Tags { get; }

        public static implicit operator Option<Transaction>(in CreateTransactionModel model) => NewTransaction(
            model.Type.ToEnum<TransactionType>(),
            model.Status.ToEnum<TransactionStatus>(),
            model.Description,
            model.Payment,
            model.Payee,
            model.Category,
            Some(model.Tags.Select(tag => NewTag(tag).ToOption())));
    }
}
