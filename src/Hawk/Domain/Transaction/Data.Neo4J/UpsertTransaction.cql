// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Store)

MERGE 
  (store:Store { 
    name: $store
  })
MERGE
  (store)-[:BELONGS]->(account)

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
  transaction:Credit:Debit
SET
  transaction:#type#,
  transaction.value = $value,
  transaction.year = $year,
  transaction.month = $month,
  transaction.day = $day
MERGE
  (transaction)-[:BELONGS]->(account)
MERGE
  (transaction)-[:IN]->(store)
MERGE
  (transaction)-[:PAID_IN]->(currency)
MERGE
  (transaction)-[:PAID_WITH]->(paymentMethod)
WITH
  account,
  store,
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
  (store)-[:HAS]->(tag)
MERGE
  (transaction)-[:HAS]->(tag)
WITH
  account,
  transaction,
  store,
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
  store: CASE WHEN store IS null THEN NULL ELSE {
    name: store.name,
	transactions: SIZE((:Transaction)-[:IN]->(store))
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
