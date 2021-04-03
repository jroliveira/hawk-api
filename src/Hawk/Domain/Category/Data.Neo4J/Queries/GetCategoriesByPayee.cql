MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:IN]->(payee:Payee)-[:BELONGS]->(account)
MATCH
  (transaction)-[:WITH]->(category:Category)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND payee.name = $payee
WITH
  DISTINCT category

RETURN
{ 
  name: category.name,
  icon: category.icon,
  transactions: SIZE((:Transaction)-[:WITH]->(category))
} AS data

ORDER BY
  category.name

SKIP 
  $skip 

LIMIT 
  $limit;
