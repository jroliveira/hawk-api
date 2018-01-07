MATCH 
  (account:Account)-[:HAS]->(transaction:Transaction)-[:PAID_IN]->(currency:Currency)
WHERE
  account.email = {email}
  AND #where#
WITH
  account,
  currency,
  transaction

OPTIONAL MATCH
  (transaction)-[:IN]->(store:Store)
WITH
  account,
  currency,
  transaction,
  store

OPTIONAL MATCH 
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)
WITH
  account,
  currency,
  transaction,
  store,
  paymentMethod

OPTIONAL MATCH
  (transaction)<-[:BELONGS]-(tag:Tag)
WITH
  account,
  currency,
  transaction,
  store,
  paymentMethod,
  tag

RETURN 
{ 
  id: transaction.id,
  type: labels(transaction),
  account: {
    id: account.id,
	email: account.email
  },
  parcel: CASE WHEN transaction.parcels IS null THEN NULL ELSE {
    number: transaction.parcel,
    total: transaction.parcels
  } END,
  store: CASE WHEN store IS null THEN NULL ELSE {
    name: store.name
  } END,
  payment: {
    value: transaction.value,
    year: transaction.year,
    month: transaction.month,
    day: transaction.day,
	currency: {
	  name: currency.name
	},
    method: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	  name: paymentMethod.name
	} END
  },
  tags: collect(tag.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};