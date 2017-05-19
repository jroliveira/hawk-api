MATCH 
  (account:Account)-[:HAS]->(:Transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)
WHERE
  account.email = {email}
WITH
{ 
  name: paymentMethod.name,
  total: count(paymentMethod.name)
} as paymentMethod

RETURN 
  paymentMethod as data

SKIP 
  {skip} 

LIMIT 
  {limit};