namespace Finance.Infrastructure.Data.Neo4j.Commands.Adjust._2017._07._02
{
    using System;
    using System.Globalization;
    using System.Linq;

    using global::Neo4j.Driver.V1;

    public class AdjustCommand
    {
        private const string GetAllQuery = "MATCH (transaction:Transaction) RETURN ID(transaction) as id, transaction.date as date";
        private const string AdjustQuery = @"
            MATCH (transaction:Transaction)
            WHERE ID(transaction) = {id}
            SET 
              transaction.id = {newId},
              transaction.year = {year},
              transaction.month = {month},
              transaction.day = {day}";

        private readonly Database database;

        public AdjustCommand(Database database)
        {
            this.database = database;
        }

        public virtual void Execute()
        {
            this.database.Execute(session =>
            {
                using (var trans = session.BeginTransaction())
                {
                    trans
                        .Run(GetAllQuery)
                        .Select(record => new
                        {
                            id = record["id"].As<int>(),
                            date = DateTime.ParseExact(record["date"].As<string>(), "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture)
                        })
                        .ToList()
                        .ForEach(item =>
                        {
                            var @params = new
                            {
                                item.id,
                                year = item.date.Year,
                                month = item.date.Month,
                                day = item.date.Day,
                                newId = Guid.NewGuid().ToString()
                            };

                            trans.Run(AdjustQuery, @params);
                        });

                    trans.Success();
                    return string.Empty;
                }
            });
        }
    }
}