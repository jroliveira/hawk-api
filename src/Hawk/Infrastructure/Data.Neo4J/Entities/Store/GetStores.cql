MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:IN]->(store:Store)-[:BELONGS]->(account)
WHERE
  account.email = {email}
WITH
  account,
  store

OPTIONAL MATCH
  (store)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)

RETURN
{ 
  name: store.name,
  tags: collect(tag.name),
  total: count(store.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};