MATCH 
  (budget:Budget)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND #where#
WITH
  account,
  budget

OPTIONAL MATCH
  (budget)-[:HAS]->(category:Category)-[:BELONGS]->(account)
WITH
  account,
  budget,
  category

OPTIONAL MATCH 
  (budget)-[:HAS]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  budget,
  category,
  currency

CALL {
  WITH
    account,
    category
  MATCH 
    (category)<-[:WITH]-(transaction:Transaction)-[:BELONGS]->(account)
  WHERE
    #where_transaction#
  RETURN
    [transaction IN COLLECT(DISTINCT transaction) | {
      id: transaction.id,
      type: LABELS(transaction),
      year: transaction.year,
      month: transaction.month,
      day: transaction.day,
      cost: {
        value: transaction.value,
        currency: {
          code: "EUR",
          symbol: "€",
          transactions: 0
        }
      }
    }] as transactions
}

RETURN 
{ 
  id: budget.id,
  description: budget.description,
  account: {
    id: account.id,
    email: account.email
  },
  limit: {
    value: budget.limit,
    currency: {
      code: currency.code,
      symbol: currency.symbol,
      transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
    }
  },
  recurrence: {
    year: budget.year,
    month: budget.month,
    day: budget.day,
    frequency: budget.frequency,
    interval: budget.interval
  },
  period: {
    limit: {
      value: budget.limit,
      currency: {
        code: currency.code,
        symbol: currency.symbol,
        transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
      }
    },
    transactions: transactions
  },
  category: CASE WHEN category IS null THEN NULL ELSE {
    name: category.name,
    icon: category.icon,
    transactions: SIZE((:Transaction)-[:WITH]->(category))
  } END
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
