MATCH 
  (store:Store)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND store.name = $name
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
