MATCH 
  (payee:Payee)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND payee.name = $name
  AND NOT ()-->(payee)

DETACH DELETE payee;
