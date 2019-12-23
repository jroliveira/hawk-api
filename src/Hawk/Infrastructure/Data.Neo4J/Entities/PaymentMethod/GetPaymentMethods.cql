MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WHERE
  account.email = $email

RETURN
{ 
  name: paymentMethod.name,
  total: count(paymentMethod.name)
} as data

SKIP 
  $skip 

LIMIT 
  $limit;
