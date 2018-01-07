MATCH 
  (account:Account)-[:HAS]->(transaction:Transaction)-[:IN]->(store:Store)
WHERE
  account.email = {email}
  AND store.name = {store}
WITH
  transaction

MATCH
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)
RETURN
{ 
  name: paymentMethod.name,
  total: count(paymentMethod.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};