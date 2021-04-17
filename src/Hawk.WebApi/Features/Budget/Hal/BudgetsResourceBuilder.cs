namespace Hawk.WebApi.Features.Budget.Hal
{
    using System.Collections.Generic;

    using Hawk.WebApi.Features.Shared.Hal;
    using Hawk.WebApi.Infrastructure.Hal.Link;

    using static System.Net.Http.HttpMethod;

    internal sealed class BudgetsResourceBuilder : ResourceBuilder
    {
        internal override IReadOnlyCollection<IResourceConfiguration> Configure() => new List<IResourceConfiguration>
        {
            new ResourceConfiguration<BudgetModel>((_, model) => new List<Link>
            {
                new Link($"/v1/budgets/{model.Id}", "self", Get),
                new Link("/v1/budgets", "all", Get),
                new Link("/v1/budgets", "create-budget", Post),
                new Link($"/v1/budgets/{model.Id}", "update-budget", Put),
                new Link($"/v1/budgets/{model.Id}", "delete-budget", Delete),
            }),

            new PageResourceConfiguration<BudgetModel>(
                getItemsLinks: (_, model) => new List<Link>
                {
                    new Link($"/v1/budgets/{model.Id}", "self", Get),
                    new Link("/v1/budgets", "all", Get),
                    new Link("/v1/budgets", "create-budget", Post),
                    new Link($"/v1/budgets/{model.Id}", "update-budget", Put),
                    new Link($"/v1/budgets/{model.Id}", "delete-budget", Delete),
                },
                getLinks: (_, model) => new List<Link>
                {
                    new Link("/v1/budgets", "self", Get),
                }),
        };
    }
}
