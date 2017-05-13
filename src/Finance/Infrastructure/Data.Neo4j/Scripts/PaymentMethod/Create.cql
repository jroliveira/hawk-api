MATCH 
  (transaction:Transaction)
WHERE
  id(transaction) = {transaction}

MERGE 
  (paymentMethod:PaymentMethod { 
    name: {method}
  })
CREATE UNIQUE
  (transaction)-[:PAY]->(paymentMethod)