namespace Hawk.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Hawk.Entities;

    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            this.CreateMap<Models.Account.Post.Account, Account>()
                .ConstructUsing(model => new Account(model.Email, model.Password));

            this.CreateMap<Account, Models.Account.Get.Account>();
            this.CreateMap<Account, Models.Transaction.Account>();
        }
    }
}
