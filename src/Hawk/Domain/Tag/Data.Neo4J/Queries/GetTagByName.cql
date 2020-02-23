MATCH 
  (tag:Tag)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND tag.name = $name

RETURN
{ 
  name: tag.name,
  transactions: SIZE((:Transaction)-[:HAS]->(tag))
} AS data;
