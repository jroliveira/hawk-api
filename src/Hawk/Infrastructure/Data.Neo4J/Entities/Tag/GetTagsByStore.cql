MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH
  (transaction)-[:IN]->(store:Store)-[:BELONGS]->(account)
MATCH 
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
WHERE
  account.email = {email}
  AND store.name = {store}

RETURN
{ 
  name: tag.name,
  total: count(tag.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};
