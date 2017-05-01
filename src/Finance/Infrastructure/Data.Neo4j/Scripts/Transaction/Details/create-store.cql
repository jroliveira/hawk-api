MATCH 
  (transaction:Transaction)
WHERE
  id(transaction) = {transaction}

MERGE 
  (store:Store { 
    name: {store}
  })
CREATE UNIQUE
  (transaction)-[:IN]->(store)