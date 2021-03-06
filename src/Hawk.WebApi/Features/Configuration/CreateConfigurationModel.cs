﻿namespace Hawk.WebApi.Features.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Hawk.Domain.Configuration;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.WebApi.Features.Category;
    using Hawk.WebApi.Features.Payee;
    using Hawk.WebApi.Features.Shared.Money;

    using static Hawk.Domain.Configuration.Configuration;
    using static Hawk.Domain.PaymentMethod.PaymentMethod;
    using static Hawk.Domain.Tag.Tag;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class CreateConfigurationModel
    {
        public CreateConfigurationModel(
            string description,
            string type,
            string paymentMethod,
            CurrencyModel currency,
            PayeeModel payee,
            CategoryModel category,
            IEnumerable<string> tags)
        {
            this.Description = description;
            this.Type = type;
            this.PaymentMethod = paymentMethod;
            this.Currency = currency;
            this.Payee = payee;
            this.Category = category;
            this.Tags = tags;
        }

        [Required]
        public string Description { get; }

        [Required]
        public string Type { get; }

        [Required]
        public string PaymentMethod { get; }

        [Required]
        public CurrencyModel Currency { get; }

        [Required]
        public PayeeModel Payee { get; }

        [Required]
        public CategoryModel Category { get; }

        [Required]
        public IEnumerable<string> Tags { get; }

        public static implicit operator Option<Configuration>(in CreateConfigurationModel model) => NewConfiguration(
            model.Type,
            model.Description,
            NewPaymentMethod(model.PaymentMethod),
            model.Currency,
            model.Payee,
            model.Category,
            Some(model.Tags.Select(tag => NewTag(tag).ToOption())));
    }
}
