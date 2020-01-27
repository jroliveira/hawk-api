MATCH 
  (category:Category)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND category.name = $name

DETACH DELETE category;
