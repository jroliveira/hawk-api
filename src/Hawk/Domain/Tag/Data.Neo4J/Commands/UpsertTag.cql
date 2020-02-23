// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Tag)

MERGE 
  (tag:Tag { 
    name: $name
  })
SET
  tag.name = $newName
MERGE
  (tag)-[:BELONGS]->(account)
WITH
  tag

RETURN
{ 
  name: tag.name,
  transactions: SIZE((:Transaction)-[:HAS]->(tag))
} AS data;
