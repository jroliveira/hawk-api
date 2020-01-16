MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
WITH
  account,
  currency

RETURN
{ 
  name: currency.name,
  total: count(currency.name)
} as data

SKIP 
  $skip 

LIMIT 
  $limit;
