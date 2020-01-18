MATCH
  (paymentMethod:PaymentMethod)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: paymentMethod.name,
  transactions: SIZE((:Transaction)-[:PAID_WITH]->(paymentMethod))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
