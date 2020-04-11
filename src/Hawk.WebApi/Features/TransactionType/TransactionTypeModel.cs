namespace Hawk.WebApi.Features.TransactionType
{
    using System.Collections.Generic;

    using Hawk.Domain.Transaction;

    using static Hawk.Domain.Transaction.TransactionType;

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

        internal static TransactionTypeModel NewTransactionTypeModel(TransactionType entity) => new TransactionTypeModel(entity);
    }
}
