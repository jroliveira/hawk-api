namespace Hawk.Domain.Tag
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Tag : ValueObject<Tag, string>
    {
        private Tag(string name, uint transactions)
            : base(name.ToKebabCase()) => this.Transactions = transactions;

        public uint Transactions { get; }

        public static Try<Tag> NewTag(
            Option<string> name,
            Option<uint> transactions = default) =>
                name
                ? new Tag(name.Get(), transactions.GetOrElse(default))
                : Failure<Tag>(new InvalidObjectException("Invalid tag."));
    }
}
