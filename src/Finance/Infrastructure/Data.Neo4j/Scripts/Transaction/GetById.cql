MATCH 
  (account:Account)-[:HAS]->(transaction:Transaction)-[:PAID_IN]->(currency:Currency)
WHERE
  id(transaction) = {id}
  AND account.email = {email}
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
  (transaction)<-[:HAS]-(tag:Tag)
WITH
  account,
  currency,
  transaction,
  store,
  paymentMethod,
  tag

RETURN 
{ 
  id: id(transaction),
  type: labels(transaction),
  account: {
    id: id(account),
	email: account.email,
	password: account.password
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
    date: transaction.date,
	currency: {
	  name: currency.name
	},
    method: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	  name: paymentMethod.name
	} END
  },
  tags: collect(tag.name)
} as data;