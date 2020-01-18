MATCH 
  (payee:Payee)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: payee.name,
  transactions: SIZE((:Transaction)-[:IN]->(payee))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
