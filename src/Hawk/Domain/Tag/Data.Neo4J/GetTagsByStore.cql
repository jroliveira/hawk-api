MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH
  (transaction)-[:IN]->(payee:Payee)-[:BELONGS]->(account)
MATCH 
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND payee.name = $payee
WITH
  DISTINCT tag

RETURN
{ 
  name: tag.name,
  transactions: SIZE((:Transaction)-[:HAS]->(tag))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
