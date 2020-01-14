MATCH 
  (store:Store)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND store.name = $name

DETACH DELETE store;
