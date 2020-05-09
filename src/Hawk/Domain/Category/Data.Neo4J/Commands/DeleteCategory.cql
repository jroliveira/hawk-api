MATCH 
  (category:Category)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND category.name = $name
  AND NOT ()-->(category)

DETACH DELETE category;
