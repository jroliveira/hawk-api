// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Payee)

MERGE 
  (payee:Payee { 
    name: $name
  })
SET
  payee.name = $newName
MERGE
  (payee)-[:BELONGS]->(account)
WITH
  payee

RETURN
{ 
  name: payee.name,
  transactions: SIZE((:Transaction)-[:IN]->(payee))
} AS data;
