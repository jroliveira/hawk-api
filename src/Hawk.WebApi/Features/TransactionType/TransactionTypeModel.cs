namespace Hawk.WebApi.Features.TransactionType
{
    using System.Collections.Generic;

    using Hawk.Domain.Shared.Transaction;

    using static Hawk.Domain.Shared.Transaction.TransactionType;

    public sealed class TransactionTypeModel
    {
        private static readonly IReadOnlyDictionary<TransactionType, char> Symbols = new Dictionary<TransactionType, char>
        {
            { Expense, '-' },
            { Income, '+' },
        };

        private TransactionTypeModel(TransactionType entity)
        {
            this.Name = entity.ToString();
            this.Symbol = Symbols[entity];
        }

        public string Name { get; }

        public char Symbol { get; }

        internal static TransactionTypeModel NewTransactionTypeModel(in TransactionType entity) => new TransactionTypeModel(entity);
    }
}
