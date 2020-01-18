MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND transaction.id = $id
WITH
  account,
  transaction

OPTIONAL MATCH
  (transaction)-[:IN]->(payee:Payee)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee

OPTIONAL MATCH 
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  paymentMethod
  
OPTIONAL MATCH 
  (transaction)-[:PAID_IN]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  paymentMethod,
  currency

OPTIONAL MATCH
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
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
