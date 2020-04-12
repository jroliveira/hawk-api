MATCH 
  (payee:Payee)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: payee.name,
  transactions: SIZE((:Transaction)-[:IN]->(payee))
} AS data

ORDER BY
  payee.name

SKIP 
  $skip 

LIMIT 
  $limit;
