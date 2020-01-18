MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:IN]->(payee:Payee)-[:BELONGS]->(account)
MATCH
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND payee.name = $payee
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
