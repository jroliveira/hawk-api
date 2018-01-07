namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities;

    internal sealed class AccountProfile : Profile
    {
        public AccountProfile()
        {
            this.CreateMap<Models.Account.Post.Account, Account>()
                .ConstructUsing(model => new Account(model.Email));

            this.CreateMap<Account, Models.Account.Get.Account>()
                .ConstructUsing(entity => new Models.Account.Get.Account(entity.Email));

            this.CreateMap<Account, Models.Transaction.Account>()
                .ConstructUsing(entity => new Models.Transaction.Account(entity.Email));
        }
    }
}
