MATCH 
  (payee:Payee)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND payee.name = $name

DETACH DELETE payee;
