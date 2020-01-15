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
  account,
  tag

RETURN
{ 
  name: tag.name,
  total: count(tag.name)
} as data;

