MATCH 
  (account:Account)
WHERE
  account.email = {email}

MERGE 
  (transaction:Transaction { 
    value: {value},
    date: {date},
    email: {email}
  })
ON CREATE 
  SET
    transaction: Debit,
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
  (transaction)-[:HAS]-(tag)

MERGE 
  (store:Store { 
    name: {store}
  })
CREATE UNIQUE
  (transaction)-[:IN]->(store)

MERGE 
  (paymentMethod:PaymentMethod { 
    name: {paymentMethod}
  })
CREATE UNIQUE
  (transaction)-[:PAY]->(paymentMethod)

RETURN 
  id(transaction) as id;