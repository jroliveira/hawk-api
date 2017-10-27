MATCH 
  (transaction:Transaction)
WHERE
  transaction.id = {transaction}

MERGE 
  (store:Store { 
    name: {store}
  })
CREATE UNIQUE
  (transaction)-[:IN]->(store)