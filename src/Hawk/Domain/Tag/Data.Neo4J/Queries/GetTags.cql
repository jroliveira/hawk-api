MATCH 
  (tag:Tag)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: tag.name,
  transactions: SIZE((:Transaction)-[:HAS]->(tag))
} AS data

ORDER BY
  tag.name

SKIP 
  $skip 

LIMIT 
  $limit;
