MATCH 
  (currency:Currency)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND currency.code = $code

RETURN
{ 
  code: currency.code,
  symbol: currency.symbol,
  transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
} AS data;
