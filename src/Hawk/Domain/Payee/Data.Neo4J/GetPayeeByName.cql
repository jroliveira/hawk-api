MATCH 
  (payee:Payee)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND payee.name = $name

RETURN
{ 
  name: payee.name,
  transactions: SIZE((:Transaction)-[:IN]->(payee))
} AS data;
