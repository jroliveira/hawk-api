// (:Account)
MATCH 
  (account:Account)
WHERE
  account.email = {email}

// (:Store)

MERGE 
  (store:Store { 
    name: {store}
  })
CREATE UNIQUE
  (store)-[:BELONGS]->(account)

// (:Currency)

MERGE 
  (currency:Currency { 
    name: {currency}
  })
CREATE UNIQUE
  (currency)-[:BELONGS]->(account)

// (:PaymentMethod)

MERGE 
  (paymentMethod:PaymentMethod { 
    name: {method}
  })
CREATE UNIQUE
  (paymentMethod)-[:BELONGS]->(account)

// (:Transaction)

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
  transaction.day = {day}
CREATE UNIQUE
  (transaction)-[:BELONGS]->(account)
CREATE UNIQUE
  (transaction)-[:IN]->(store)
CREATE UNIQUE
  (transaction)-[:PAID_IN]->(currency)
CREATE UNIQUE
  (transaction)-[:PAID_WITH]->(paymentMethod)
WITH
  account,
  store,
  currency,
  paymentMethod,
  transaction

// (:Tag)

UNWIND
  {tags} AS tagName
MERGE
  (tag:Tag { 
    name: tagName
  })
CREATE UNIQUE
  (tag)-[:BELONGS]->(account)
CREATE UNIQUE
  (store)-[:HAS]->(tag)
CREATE UNIQUE
  (transaction)-[:HAS]->(tag)

RETURN 
{ 
  id: transaction.id,
  type: labels(transaction),
  account: {
    id: account.id,
	email: account.email
  },
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
  tags: collect(distinct tag.name)
} as data;
