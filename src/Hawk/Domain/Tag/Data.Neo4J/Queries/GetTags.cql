MATCH 
  (tag:Tag)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: tag.name,
  transactions: SIZE((:Transaction)-[:HAS]->(tag))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
