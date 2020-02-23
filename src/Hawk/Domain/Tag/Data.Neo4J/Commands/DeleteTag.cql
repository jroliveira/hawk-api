MATCH 
  (tag:Tag)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND tag.name = $name

DETACH DELETE tag;
