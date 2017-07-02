MATCH 
  (account:Account)
WHERE
  account.email = {email}

MERGE
  (transaction:Transaction:#type# { 
    id: {id}
  })
SET
  transaction.value = {value},
  transaction.year = {year},
  transaction.month = {month},
  transaction.day = {day},
  transaction.email = {email},
  transaction.parcels = {parcels},
  transaction.parcel = {parcel}

CREATE UNIQUE
  (account)-[:HAS]->(transaction)

RETURN 
  transaction.id as id;