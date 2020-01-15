// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Store)

MERGE 
  (store:Store { 
    name: $name
  })
SET
  store.name = $newName
MERGE
  (store)-[:BELONGS]->(account)
WITH
  account,
  store

OPTIONAL MATCH
  (store)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)

RETURN
{ 
  name: store.name,
  tags: collect(distinct tag.name),
  total: count(store.name)
} as data;

