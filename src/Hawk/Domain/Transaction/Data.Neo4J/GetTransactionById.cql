MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND transaction.id = $id
WITH
  account,
  transaction

OPTIONAL MATCH
  (transaction)-[:IN]->(store:Store)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  store

OPTIONAL MATCH 
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  store,
  paymentMethod
  
OPTIONAL MATCH 
  (transaction)-[:PAID_IN]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  store,
  paymentMethod,
  currency

OPTIONAL MATCH
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
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
