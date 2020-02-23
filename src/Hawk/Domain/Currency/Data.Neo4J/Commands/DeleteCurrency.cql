MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND currency.name = $name

DETACH DELETE currency;
