// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Currency)

MERGE 
  (currency:Currency { 
    name: $name
  })
SET
  currency.name = $newName
MERGE
  (currency)-[:BELONGS]->(account)
WITH
  account,
  currency

RETURN
{ 
  name: currency.name,
  total: count(currency.name)
} as data;

