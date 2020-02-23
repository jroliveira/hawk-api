MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND currency.name = $name

RETURN
{ 
  name: currency.name,
  transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
} AS data;
