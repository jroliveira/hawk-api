namespace Hawk.Reports.AmountGroupByTag
{
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Filter;
    using Hawk.Reports;
    using Hawk.Reports.Mappings;

    using Http.Query.Filter;

    using GetScript = Hawk.Reports.GetScript;

    internal class GetQuery : GetQueryBase, IGetQuery
    {
        public GetQuery(
            Database database, 
            ItemMapping mapping, 
            GetScript file, 
            ILimit<int, Filter> limit, 
            ISkip<int, Filter> skip, 
            IWhere<string, Filter> where)
            : base(database, mapping, file, limit, skip, where)
        {
        }

        protected override string GetQueryString()
        {
            return this.File.ReadAllText(@"AmountGroupByTag.Query.cql");
        }
    }
}