MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: currency.name,
  transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
