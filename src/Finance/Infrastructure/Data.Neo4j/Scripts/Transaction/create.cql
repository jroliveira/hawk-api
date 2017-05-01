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

RETURN 
  id(transaction) as id;