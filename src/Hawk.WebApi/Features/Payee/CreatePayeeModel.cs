namespace Hawk.WebApi.Features.Payee
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Payee.Payee;

    public class CreatePayeeModel
    {
        public CreatePayeeModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Payee>(CreatePayeeModel model) => NewPayee(model.Name);
    }
}
