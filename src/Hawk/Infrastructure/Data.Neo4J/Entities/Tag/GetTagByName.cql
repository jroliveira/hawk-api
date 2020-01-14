MATCH 
  (tag:Tag)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND tag.name = $name
WITH
  account,
  tag

RETURN
{ 
  name: tag.name,
  total: count(tag.name)
} as data;
