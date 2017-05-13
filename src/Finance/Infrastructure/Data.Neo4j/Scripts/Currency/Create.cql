MATCH 
  (transaction:Transaction)
WHERE
  id(transaction) = {transaction}

MERGE 
  (currency:Currency { 
    name: {currency}
  })
CREATE UNIQUE
  (transaction)-[:PAY]->(currency)