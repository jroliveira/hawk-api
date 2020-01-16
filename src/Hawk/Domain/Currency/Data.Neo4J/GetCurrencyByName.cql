MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND currency.name = $name
WITH
  account,
  currency

RETURN
{ 
  name: currency.name,
  total: count(currency.name)
} as data;
