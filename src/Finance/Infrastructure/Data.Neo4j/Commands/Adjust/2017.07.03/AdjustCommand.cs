namespace Finance.Infrastructure.Data.Neo4j.Commands.Adjust._2017._07._03
{
    using System;

    public class AdjustCommand
    {
        private const string AdjustQuery = @"
            MATCH (account:Account)
            WHERE ID(account) = 0
            SET 
              account.id = {newId}";

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
                    var @params = new
                    {
                        newId = Guid.NewGuid().ToString()
                    };

                    trans.Run(AdjustQuery, @params);
                    trans.Success();
                    return string.Empty;
                }
            });
        }
    }
}