// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Currency)

MERGE 
  (currency:Currency { 
    code: $code
  })
SET
  currency.code = $newCode,
  currency.symbol = $symbol
MERGE
  (currency)-[:BELONGS]->(account)
WITH
  currency

RETURN
{ 
  code: currency.code,
  symbol: currency.symbol,
  transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
} AS data;
