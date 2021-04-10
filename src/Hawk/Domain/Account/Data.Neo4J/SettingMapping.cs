namespace Hawk.Domain.Account.Data.Neo4J
{
    using System.Linq;

    using Hawk.Domain.Account;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static Hawk.Domain.Account.Data.Neo4J.MoneyMapping;
    using static Hawk.Domain.Account.Money;
    using static Hawk.Domain.Account.Setting;
    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class SettingMapping
    {
        internal static Try<Setting> MapSetting(in Option<Neo4JRecord> recordOption) => recordOption
            .Fold(Failure<Setting>(NotFound(nameof(Setting))))(record => NewSetting(MapMoney(record.GetRecord("money")).GetOrElse(DefaultMoney)));
    }
}
