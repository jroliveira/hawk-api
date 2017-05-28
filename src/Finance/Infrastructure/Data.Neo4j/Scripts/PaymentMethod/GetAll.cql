MATCH 
  (account:Account)-[:HAS]->(:Transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)
WHERE
  account.email = {email}
RETURN
{ 
  name: paymentMethod.name,
  total: count(paymentMethod.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};