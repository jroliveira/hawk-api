MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:IN]->(store:Store)-[:BELONGS]->(account)
MATCH
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND store.name = $store
WITH
  DISTINCT paymentMethod

RETURN
{ 
  name: paymentMethod.name,
  transactions: SIZE((:Transaction)-[:PAID_WITH]->(paymentMethod))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
