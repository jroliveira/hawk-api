namespace Hawk.WebApi.Features.Budget
{
    using System;
    using System.Linq;

    using Hawk.Domain.Budget;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;
    using Hawk.WebApi.Features.Shared.Money;

    public sealed class PeriodModel
    {
        private PeriodModel(
            DateTime? from,
            DateTime? to,
            MoneyModel? amount,
            uint transactions)
        {
            this.From = from;
            this.To = to;
            this.Amount = amount;
            this.Transactions = transactions;
        }

        public DateTime? From { get; }

        public DateTime? To { get; }

        public MoneyModel? Amount { get; }

        public uint Transactions { get; }

        public static implicit operator PeriodModel(Period entity) => new PeriodModel(
            entity.FromOption.GetOrElse(default),
            entity.ToOption.GetOrElse(default),
            entity.Amount,
            entity.Transactions);

        public static implicit operator PeriodModel?(in Option<Period> entityOption) => entityOption
            .Fold(default(PeriodModel))(entity => new PeriodModel(
                entity.FromOption.GetOrElse(default),
                entity.ToOption.GetOrElse(default),
                entity.Amount,
                entity.Transactions));
    }
}
