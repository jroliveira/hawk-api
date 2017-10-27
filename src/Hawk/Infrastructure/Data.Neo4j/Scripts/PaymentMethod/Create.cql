MATCH 
  (transaction:Transaction)
WHERE
  transaction.id = {transaction}

MERGE 
  (paymentMethod:PaymentMethod { 
    name: {method}
  })
CREATE UNIQUE
  (transaction)-[:PAID_WITH]->(paymentMethod)