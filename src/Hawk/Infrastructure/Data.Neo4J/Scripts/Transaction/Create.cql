MATCH 
  (account:Account)
WHERE
  account.email = {email}

MERGE
  (transaction:Transaction { 
    id: {id}
  })
REMOVE 
  transaction:Credit:Debit
SET
  transaction:#type#,
  transaction.value = {value},
  transaction.year = {year},
  transaction.month = {month},
  transaction.day = {day},
  transaction.email = {email},
  transaction.parcels = {parcels},
  transaction.parcel = {parcel}
CREATE UNIQUE
  (account)-[:HAS]->(transaction)
WITH
  account,
  transaction

UNWIND
  {tags} AS tagName
MERGE
  (tag:Tag { 
    name: tagName
  })
CREATE UNIQUE
  (transaction)<-[:BELONGS]-(tag)

MERGE 
  (currency:Currency { 
    name: {currency}
  })
CREATE UNIQUE
  (transaction)-[:PAID_IN]->(currency)

MERGE 
  (paymentMethod:PaymentMethod { 
    name: {method}
  })
CREATE UNIQUE
  (transaction)-[:PAID_WITH]->(paymentMethod)

MERGE 
  (store:Store { 
    name: {store}
  })
CREATE UNIQUE
  (transaction)-[:IN]->(store)

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
} as data;