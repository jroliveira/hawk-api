MATCH 
  (tag:Tag)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND tag.name = $name
  AND NOT ()-->(tag)

DETACH DELETE tag;
