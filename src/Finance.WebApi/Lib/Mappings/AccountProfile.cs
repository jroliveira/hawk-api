namespace Finance.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Finance.Entities;
    using Model = Finance.WebApi.Models.Account.Post;

    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            this.CreateMap<Model.Account, Account>()
                .ConstructUsing(model => new Account(model.Email, model.Password));

            this.CreateMap<Account, Models.Account.Get.Account>();
        }
    }
}
