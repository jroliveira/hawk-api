// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:PaymentMethod)

MERGE 
  (paymentMethod:PaymentMethod { 
    name: $name
  })
SET
  paymentMethod.name = $newName
MERGE
  (paymentMethod)-[:BELONGS]->(account)
WITH
  paymentMethod

RETURN
{ 
  name: paymentMethod.name,
  transactions: SIZE((:Transaction)-[:HAS]->(paymentMethod))
} AS data;
