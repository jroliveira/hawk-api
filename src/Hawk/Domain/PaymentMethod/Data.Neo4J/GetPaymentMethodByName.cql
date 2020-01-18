MATCH 
  (paymentMethod:PaymentMethod)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND paymentMethod.name = $name

RETURN
{ 
  name: paymentMethod.name,
  transactions: SIZE((:Transaction)-[:HAS]->(paymentMethod))
} AS data;
