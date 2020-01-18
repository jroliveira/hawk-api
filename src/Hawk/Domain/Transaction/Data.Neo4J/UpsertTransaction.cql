// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Payee)

MERGE 
  (payee:Payee { 
    name: $payee
  })
MERGE
  (payee)-[:BELONGS]->(account)

// (:Currency)

MERGE 
  (currency:Currency { 
    name: $currency
  })
MERGE
  (currency)-[:BELONGS]->(account)

// (:PaymentMethod)

MERGE 
  (paymentMethod:PaymentMethod { 
    name: $method
  })
MERGE
  (paymentMethod)-[:BELONGS]->(account)

// (:Transaction)

MERGE
  (transaction:Transaction { 
    id: $id
  })
REMOVE 
  transaction:Income:Expense
SET
  transaction:#type#,
  transaction.value = $value,
  transaction.year = $year,
  transaction.month = $month,
  transaction.day = $day
MERGE
  (transaction)-[:BELONGS]->(account)
MERGE
  (transaction)-[:IN]->(payee)
MERGE
  (transaction)-[:PAID_IN]->(currency)
MERGE
  (transaction)-[:PAID_WITH]->(paymentMethod)
WITH
  account,
  payee,
  currency,
  paymentMethod,
  transaction

// (:Tag)

UNWIND
  $tags AS tagName
MERGE
  (tag:Tag { 
    name: tagName
  })
MERGE
  (tag)-[:BELONGS]->(account)
MERGE
  (payee)-[:HAS]->(tag)
MERGE
  (transaction)-[:HAS]->(tag)
WITH
  account,
  transaction,
  payee,
  paymentMethod,
  currency,
  COLLECT(DISTINCT tag) AS tags

RETURN 
{ 
  id: transaction.id,
  type: LABELS(transaction),
  account: {
    id: account.id,
	email: account.email
  },
  payee: CASE WHEN payee IS null THEN NULL ELSE {
    name: payee.name,
	transactions: SIZE((:Transaction)-[:IN]->(payee))
  } END,
  payment: {
    value: transaction.value,
    year: transaction.year,
    month: transaction.month,
    day: transaction.day,
	currency: {
	  name: currency.name,
      transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
	},
    method: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	  name: paymentMethod.name,
      transactions: SIZE((:Transaction)-[:PAID_WITH]->(paymentMethod))
	} END
  },
  tags: [tag IN tags | {
    name: tag.name,
    transactions: SIZE((:Transaction)-[:HAS]->(tag))
  }]
} AS data;
