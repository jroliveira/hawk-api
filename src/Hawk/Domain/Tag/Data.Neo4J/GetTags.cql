MATCH 
  (tag:Tag)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: tag.name,
  total: count(tag.name)
} as data

SKIP 
  $skip 

LIMIT 
  $limit;
