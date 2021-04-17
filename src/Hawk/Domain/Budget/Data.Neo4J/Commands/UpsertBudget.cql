// (:Account)

MATCH
  (account:Account)
WHERE
  account.email = $email

// (:Currency)

MATCH
  (currency:Currency)-[:BELONGS]->(account)
WHERE
  currency.code = $currency

// (:Budget)

MERGE
  (budget:Budget { 
    id: $id
  })
SET
  budget.description = $description,
  budget.limit = $limit,
  budget.year = $year,
  budget.month = $month,
  budget.day = $day,
  budget.frequency = $frequency,
  budget.interval = $interval
WITH
  account,
  currency,
  budget

OPTIONAL MATCH
  (budget)-[relationships]->()
DELETE
  relationships
MERGE
  (budget)-[:BELONGS]->(account)
MERGE
  (budget)-[:HAS]->(currency)
WITH
  account,
  currency,
  budget

// (:Category)

MATCH
  (category:Category)-[:BELONGS]->(account)
WHERE
  category.name = $category
MERGE
  (budget)-[:HAS]->(category)
WITH
  account,
  currency,
  budget,
  category

CALL {
  WITH
    account,
    category
  MATCH 
    (category)<-[:WITH]-(transaction:Transaction)-[:BELONGS]->(account)
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
} AS data;
