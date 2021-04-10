namespace Hawk.WebApi.Features.Account
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Account;

    public sealed class SettingModel
    {
        private SettingModel(MoneyModel money) => this.Money = money;

        [Required]
        public MoneyModel Money { get; }

        public static implicit operator SettingModel(in Setting entity) => new SettingModel(entity.Money);
    }
}
