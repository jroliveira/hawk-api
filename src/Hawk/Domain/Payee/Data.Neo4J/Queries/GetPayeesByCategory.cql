MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:IN]->(payee:Payee)-[:BELONGS]->(account)
MATCH
  (transaction)-[:WITH]->(category:Category)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND category.name = $category
WITH
  DISTINCT payee

RETURN
{ 
  name: payee.name,
  transactions: SIZE((:Transaction)-[:IN]->(payee))
} AS data

ORDER BY
  payee.name

SKIP 
  $skip 

LIMIT 
  $limit;
