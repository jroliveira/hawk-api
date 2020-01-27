MATCH 
  (category:Category)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: category.name,
  transactions: SIZE((:Transaction)-[:WITH]->(category))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
