MATCH 
  (category:Category)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND category.name = $name

RETURN
{ 
  name: category.name,
  icon: category.icon,
  transactions: SIZE((:Transaction)-[:WITH]->(category))
} AS data;
