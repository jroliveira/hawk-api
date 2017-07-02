MATCH 
  (transaction:Transaction)
WHERE
  transaction.id = {transaction}

MERGE 
  (currency:Currency { 
    name: {currency}
  })
CREATE UNIQUE
  (transaction)-[:PAID_IN]->(currency)