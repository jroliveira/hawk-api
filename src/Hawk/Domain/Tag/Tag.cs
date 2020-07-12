namespace Hawk.Domain.Tag
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Tag : Entity<string>
    {
        private Tag(in string name, in uint transactions)
            : base(name.ToKebabCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Tag> NewTag(in Option<string> nameOption, in Option<uint> transactionsOption = default) =>
            nameOption
                ? new Tag(
                    nameOption.Get(),
                    transactionsOption.GetOrElse(default))
                : Failure<Tag>(new InvalidObjectException("Invalid tag."));
    }
}
