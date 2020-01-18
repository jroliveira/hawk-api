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
  store

RETURN
{ 
  name: store.name,
  transactions: SIZE((:Transaction)-[:IN]->(store))
} AS data;
