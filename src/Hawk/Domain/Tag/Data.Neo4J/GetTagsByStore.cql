MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH
  (transaction)-[:IN]->(store:Store)-[:BELONGS]->(account)
MATCH 
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND store.name = $store
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
