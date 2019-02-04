MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:IN]->(store:Store)-[:BELONGS]->(account)
MATCH
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WHERE
  account.email = {email}
  AND store.name = {store}

RETURN
{ 
  name: paymentMethod.name,
  total: count(paymentMethod.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};
