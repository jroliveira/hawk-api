MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  code: currency.code,
  symbol: currency.symbol,
  transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
} AS data

ORDER BY
  currency.code

SKIP 
  $skip 

LIMIT 
  $limit;
