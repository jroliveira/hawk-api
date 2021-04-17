MATCH 
  (budget:Budget)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND budget.id = $id

DETACH DELETE budget;
