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
  category.name = $newName,
  category.icon = $icon
MERGE
  (category)-[:BELONGS]->(account)
WITH
  category

RETURN
{ 
  name: category.name,
  icon: category.icon,
  transactions: SIZE((:Transaction)-[:WITH]->(category))
} AS data;
