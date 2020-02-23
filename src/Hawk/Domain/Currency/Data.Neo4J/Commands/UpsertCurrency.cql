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
  currency

RETURN
{ 
  name: currency.name,
  transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
} AS data;
