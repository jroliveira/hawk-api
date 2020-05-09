MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND currency.code = $code
  AND NOT ()-->(currency)

DETACH DELETE currency;
