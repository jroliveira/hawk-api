// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Category)

MERGE 
  (category:Category { 
    name: $name
  })
SET
  category.name = $newName
MERGE
  (category)-[:BELONGS]->(account)
WITH
  category

RETURN
{ 
  name: category.name,
  transactions: SIZE((:Transaction)-[:WITH]->(category))
} AS data;
