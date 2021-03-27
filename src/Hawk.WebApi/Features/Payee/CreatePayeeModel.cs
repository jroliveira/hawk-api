namespace Hawk.WebApi.Features.Payee
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Payee.Payee;

    public class CreatePayeeModel
    {
        public CreatePayeeModel(
            string name,
            LocationModel? location)
        {
            this.Name = name;
            this.Location = location;
        }

        [Required]
        public string Name { get; }

        public LocationModel? Location { get; }

        public static implicit operator Option<Payee>(in CreatePayeeModel model) => NewPayee(
            model.Name,
            model.Location);
    }
}
