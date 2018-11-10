MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
WHERE
  account.email = {email}

RETURN
{ 
  name: tag.name,
  total: count(tag.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};