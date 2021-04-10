namespace Hawk.Domain.Account
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Account.Money;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Setting
    {
        private Setting(in Money money) => this.Money = money;

        public static Setting DefaultSetting => new Setting(DefaultMoney);

        public Money Money { get; }

        public static Try<Setting> NewSetting(in Option<Money> moneyOption) =>
            moneyOption
                ? new Setting(moneyOption.Get())
                : Failure<Setting>(new InvalidObjectException("Invalid setting."));
    }
}
